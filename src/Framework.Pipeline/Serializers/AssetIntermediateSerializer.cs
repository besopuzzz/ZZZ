using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Reflection;
using System.Xml;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    internal class AssetIntermediateSerializer
    {
        public IEnumerable<ElementInfo> Elements => elements;
        public struct ElementInfo
        {
            public ContentSerializerAttribute Attribute;

            public Type Type;

            public Action<object, object> Setter;

            public Func<object, object> Getter;
        }
       
        private readonly List<ElementInfo> elements = new List<ElementInfo>();
        private bool GetElementInfo(MemberInfo member, out ElementInfo info)
        {
            info = default(ElementInfo);

            if (member.GetCustomAttribute<ContentSerializerIgnoreAttribute>() != null)
            {
                return false;
            }

            PropertyInfo prop = member as PropertyInfo;
            FieldInfo fieldInfo = member as FieldInfo;
            ContentSerializerAttribute customAttribute = member.GetCustomAttribute<ContentSerializerAttribute>();

            if (customAttribute != null)
            {
                info.Attribute = customAttribute.Clone();
                if (string.IsNullOrEmpty(customAttribute.ElementName))
                {
                    info.Attribute.ElementName = member.Name;
                }
            }
            else
            {
                if (prop != null)
                {
                    info.Type = prop.PropertyType;

                    if (prop.GetGetMethod() == null)
                    {
                        return false;
                    }

                    MethodInfo setMethod = prop.GetSetMethod(nonPublic: true);
                    if (setMethod != null && !setMethod.IsPublic)
                    {
                        return false;
                    }

                    if (setMethod == null/* && !serializer.GetTypeSerializer(prop.PropertyType).CanDeserializeIntoExistingObject*/)
                    {
                        return false;
                    }

                    if (prop.GetIndexParameters().Any())
                    {
                        return false;
                    }
                }
                else if (fieldInfo != null && !fieldInfo.IsPublic)
                {
                    return false;
                }

                info.Attribute = new ContentSerializerAttribute();
                info.Attribute.ElementName = member.Name;
            }

            if (prop != null)
            {
                //info.Serializer = serializer.GetTypeSerializer(prop.PropertyType);

                if (prop.CanWrite)
                {
                    info.Setter = delegate (object o, object v)
                    {
                        prop.SetValue(o, v, null);
                    };
                }

                info.Getter = (object o) => prop.GetValue(o, null);
            }
            else if (fieldInfo != null)
            {
                //info.Serializer = serializer.GetTypeSerializer(fieldInfo.FieldType);
                info.Type = fieldInfo.FieldType;
                info.Setter = fieldInfo.SetValue;
                info.Getter = fieldInfo.GetValue;
            }

            return true;
        }

        private bool initialized = false;

        public AssetIntermediateSerializer()
        {

        }

        private void Initialize(IntermediateSerializer serializer, Type type)
        {
            if (initialized)
                return;

            if(type.BaseType != null)
                Initialize(serializer, type.BaseType);

            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            PropertyInfo[] properties = type.GetProperties(flags);
            foreach (PropertyInfo member in properties)
            {
                if (GetElementInfo(member, out var info))
                {
                    elements.Add(info);
                }
            }

            FieldInfo[] fields = type.GetFields(flags);
            foreach (FieldInfo member2 in fields)
            {
                if (GetElementInfo(member2, out var info2))
                {
                    elements.Add(info2);
                }
            }

            initialized = true;
        }

        public object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance, Type targetType)
        {
            Initialize(input.Serializer, targetType);

            foreach (ElementInfo info in Elements)
            {
                if (!info.Attribute.FlattenContent && !input.MoveToElement(info.Attribute.ElementName))
                {
                    if (!info.Attribute.Optional)
                    {
                        throw new Exception(string.Format("The Xml element `{0}` is required, but element `{1}` was found at line {2}:{3}.",
                            info.Attribute.ElementName, input.Xml.Name, ((IXmlLineInfo)input.Xml).LineNumber, ((IXmlLineInfo)input.Xml).LinePosition));
                    }
                }
                else if (info.Attribute.SharedResource)
                {
                    Action<object> fixup = delegate (object o)
                    {
                        info.Setter(existingInstance, o);
                    };
                    input.ReadSharedResource(info.Attribute, fixup);
                }
                else
                {
                    var serializer = input.Serializer.GetTypeSerializer(info.Type);

                    if (info.Setter == null)
                    {
                        object existingInstance2 = info.Getter(existingInstance);
                        input.ReadObject(info.Attribute, serializer, existingInstance2);
                    }
                    else
                    {
                        object arg = input.ReadObject<object>(info.Attribute, serializer);
                        info.Setter(existingInstance, arg);
                    }
                }
            }

            return existingInstance;
        }

        public void Serialize(IntermediateWriter output, object value, ContentSerializerAttribute format, Type targetType)
        {
            Initialize(output.Serializer, targetType);

            foreach (ElementInfo element in Elements)
            {

                object value2 = element.Getter(value);
                if (element.Attribute.SharedResource)
                {
                    output.WriteSharedResource(value2, element.Attribute);
                }
                else
                {
                    var obj = element.Getter.Invoke(value);

                    var serializer = output.Serializer.GetTypeSerializer(element.Type);

                    output.WriteObject(obj, element.Attribute, serializer);
                }
            }

        }
    }
}
