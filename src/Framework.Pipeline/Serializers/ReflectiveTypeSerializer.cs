using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Reflection;
using System.Xml;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    internal sealed class ReflectiveTypeSerializer
    {
        private struct ElementInfo
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

            var prop = member as PropertyInfo;
            var field = member as FieldInfo;
            var attribute = member.GetCustomAttribute<ContentSerializerAttribute>();

            if (attribute != null)
            {
                info.Attribute = attribute.Clone();
                if (string.IsNullOrEmpty(attribute.ElementName))
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

                    MethodInfo setMethod = prop.GetSetMethod(true);

                    if (setMethod != null && !setMethod.IsPublic)
                    {
                        return false;
                    }

                    if (setMethod == null)
                    {
                        return false;
                    }

                    if (prop.GetIndexParameters().Any())
                    {
                        return false;
                    }
                }
                else if (field != null && !field.IsPublic)
                {
                    return false;
                }

                info.Attribute = new ContentSerializerAttribute();
                info.Attribute.ElementName = member.Name;
            }

            if (prop != null)
            {
                if (prop.CanWrite)
                {
                    info.Setter = delegate (object o, object v)
                    {
                        prop.SetValue(o, v, null);
                    };
                }

                info.Getter = (object o) => prop.GetValue(o, null);
            }
            else if (field != null)
            {
                info.Type = field.FieldType;
                info.Setter = field.SetValue;
                info.Getter = field.GetValue;
            }

            return true;
        }

        public void Initialize(IntermediateSerializer serializer, Type type)
        {
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
        }

        public object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance, Type targetType)
        {
            foreach (ElementInfo info in elements)
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
            foreach (ElementInfo element in elements)
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
