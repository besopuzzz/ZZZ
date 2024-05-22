using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ.Framework.Assets.Pipeline.Readers
{
    public class Reader
    {
        private List<PropertyInfo> _properties = new List<PropertyInfo>();
        private List<FieldInfo> _fields = new List<FieldInfo>();


        private string _runtimeType;
        private ContentTypeReaderManager _compiler;
        private static HashSet<MemberInfo> _sharedResources = new HashSet<MemberInfo>();

        protected int _typeVersion;

        public void Initialize(ContentTypeReaderManager compiler, Type assetType)
        {
            _compiler = compiler;

            var type = assetType.BaseType;

            if (type != null && type != typeof(object) && !assetType.IsValueType)
                Initialize(compiler, type);

            var runtimeType = assetType.GetCustomAttributes(typeof(ContentSerializerRuntimeTypeAttribute), false).FirstOrDefault() as ContentSerializerRuntimeTypeAttribute;
            if (runtimeType != null)
                _runtimeType = runtimeType.RuntimeType;

            var typeVersion = assetType.GetCustomAttributes(typeof(ContentSerializerTypeVersionAttribute), false).FirstOrDefault() as ContentSerializerTypeVersionAttribute;
            if (typeVersion != null)
                _typeVersion = typeVersion.TypeVersion;


            _properties.AddRange(GetAllProperties(assetType).Where(IsValidProperty).ToArray());
            _fields.AddRange(GetAllFields(assetType).Where(IsValidField).ToArray());
        }


        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            var attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            return type.GetConstructor(attrs, null, new Type[0], null);
        }

        public static PropertyInfo[] GetAllProperties(Type type)
        {

            const BindingFlags attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var allProps = type.GetProperties(attrs).ToList();
            var props = allProps.FindAll(p => p.GetGetMethod(true) != null && p.GetGetMethod(true) == p.GetGetMethod(true).GetBaseDefinition()).ToArray();
            return props;
        }


        public static FieldInfo[] GetAllFields(Type type)
        {
            var attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            return type.GetFields(attrs);
        }

        public static bool IsClass(Type type)
        {
            return type.IsClass;
        }


        private static bool PropertyIsPublic(PropertyInfo property)
        {
            if (property == null)
            {
                throw new NullReferenceException("Must supply the property parameter");
            }

            var getMethod = property.GetGetMethod();

            if (getMethod == null || !getMethod.IsPublic)
                return false;

            return true;
        }


        private bool IsValidProperty(PropertyInfo property)
        {
            // Properties must have at least a getter.
            if (property.CanRead == false)
                return false;

            // Skip over indexer properties.
            if (property.Name == "Item" && property.GetIndexParameters().Length > 0)
                return false;

            // Are we explicitly asked to ignore this item?
            if (property.GetCustomAttribute<ContentSerializerIgnoreAttribute>() != null)
                return false;

            var contentSerializerAttribute = property.GetCustomAttribute<ContentSerializerAttribute>();
            if (contentSerializerAttribute == null)
            {
                // There is no ContentSerializerAttribute, so non-public
                // properties cannot be serialized.
                if (!PropertyIsPublic(property))
                    return false;

                // Check the type reader to see if it is safe to
                // deserialize into the existing type.
                if (!property.CanWrite)
                {
                    if (!_compiler.GetTypeReader(property.PropertyType).CanDeserializeIntoExistingObject)
                        return false;
                }
            }
            else if (contentSerializerAttribute.SharedResource)
            {
                _sharedResources.Add(property);
            }

            return true;
        }

        private bool IsValidField(FieldInfo field)
        {
            // Are we explicitly asked to ignore this item?
            if (field.GetCustomAttribute<ContentSerializerIgnoreAttribute>() != null)
                return false;

            var contentSerializerAttribute = field.GetCustomAttribute<ContentSerializerAttribute>();
            if (contentSerializerAttribute == null)
            {
                // There is no ContentSerializerAttribute, so non-public
                // fields cannot be deserialized.
                if (!field.IsPublic)
                    return false;

                // evolutional: Added check to skip initialise only fields
                if (field.IsInitOnly)
                    return false;
            }
            else if (contentSerializerAttribute.SharedResource)
            {
                _sharedResources.Add(field);
            }

            return true;
        }

        private void Read(object parent, ContentReader output, MemberInfo member)
        {
            var property = member as PropertyInfo;
            var field = member as FieldInfo;

            Type elementType;
            object memberObject = null;



            if (property != null)
            {
                elementType = property.PropertyType;

                if (_sharedResources.Contains(member))
                {
                    output.ReadSharedResource<Asset>(x => memberObject = x);
                     
                }
                else
                {
                    var reader = _compiler.GetTypeReader(elementType);

                    memberObject = output.ReadObject(reader, memberObject);
                }

                property.SetValue(parent, memberObject);
            }
            else
            {
                elementType = field.FieldType;

                if (_sharedResources.Contains(member))
                {
                    output.ReadSharedResource<Asset>(x => memberObject = x);

                }
                else
                {
                    var reader = _compiler.GetTypeReader(elementType);

                    memberObject = output.ReadObject(reader, memberObject);
                }
                field.SetValue(parent, memberObject);
            }

        }

        public void Read(ContentReader output, object value)
        {
            foreach (var property in _properties)
                Read(value, output, property);

            foreach (var field in _fields)
                Read(value, output, field);

        }

    }

    //    internal class AssetContentManagerSerializer<T>
    //    where T : Asset
    //{
    //    delegate void ReadElement(ContentReader input, object parent);

    //    private List<ReadElement> _readers = new List<ReadElement>();

    //    private ConstructorInfo _constructor;

    //    private ContentTypeReader _baseTypeReader;

    //    public void Initialize(ContentTypeReaderManager manager, Type targetType)
    //    {
    //        _constructor = targetType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);

    //        var baseType = targetType.BaseType;
    //        if (baseType != null && baseType != typeof(object))
    //            Initialize(manager, baseType);


    //        const BindingFlags attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    //        var properties = targetType.GetProperties(attrs).ToList().FindAll(p => p.GetGetMethod(true) != null && p.GetGetMethod(true) == p.GetGetMethod(true).GetBaseDefinition()).ToArray();

    //        var fields = targetType.GetFields(attrs);

    //        // Gather the properties.
    //        foreach (var property in properties)
    //        {
    //            var read = GetElementReader(manager, property);
    //            if (read != null)
    //                _readers.Add(read);
    //        }

    //        // Gather the fields.
    //        foreach (var field in fields)
    //        {
    //            var read = GetElementReader(manager, field);
    //            if (read != null)
    //                _readers.Add(read);
    //        }
    //    }

    //    private static bool PropertyIsPublic(PropertyInfo property)
    //    {
    //        if (property == null)
    //        {
    //            throw new NullReferenceException("Must supply the property parameter");
    //        }

    //        var getMethod = property.GetGetMethod();

    //        if (getMethod == null || !getMethod.IsPublic)
    //            return false;

    //        return true;
    //    }

    //    private static ReadElement GetElementReader(ContentTypeReaderManager manager, MemberInfo member)
    //    {
    //        var property = member as PropertyInfo;
    //        var field = member as FieldInfo;

    //        if (property != null)
    //        {
    //            // Properties must have at least a getter.
    //            if (property.CanRead == false)
    //                return null;

    //            // Skip over indexer properties.
    //            if (property.GetIndexParameters().Any())
    //                return null;
    //        }

    //        // Are we explicitly asked to ignore this item?
    //        if (member.GetCustomAttribute<ContentSerializerIgnoreAttribute>() != null)
    //            return null;

    //        var contentSerializerAttribute = member.GetCustomAttribute<ContentSerializerAttribute>();
    //        if (contentSerializerAttribute == null)
    //        {
    //            if (property != null)
    //            {
    //                // There is no ContentSerializerAttribute, so non-public
    //                // properties cannot be deserialized.
    //                if (!PropertyIsPublic(property))
    //                    return null;

    //                // If the read-only property has a type reader,
    //                // and CanDeserializeIntoExistingObject is true,
    //                // then it is safe to deserialize into the existing object.
    //                if (!property.CanWrite)
    //                {
    //                    var typeReader = manager.GetTypeReader(property.PropertyType);
    //                    if (typeReader == null || !typeReader.CanDeserializeIntoExistingObject)
    //                        return null;
    //                }
    //            }
    //            else
    //            {
    //                // There is no ContentSerializerAttribute, so non-public
    //                // fields cannot be deserialized.
    //                if (!field.IsPublic)
    //                    return null;

    //                // evolutional: Added check to skip initialise only fields
    //                if (field.IsInitOnly)
    //                    return null;
    //            }
    //        }

    //        Action<object, object> setter;
    //        Type elementType;
    //        if (property != null)
    //        {
    //            elementType = property.PropertyType;
    //            if (property.CanWrite)
    //                setter = (o, v) => property.SetValue(o, v, null);
    //            else
    //                setter = (o, v) => { };
    //        }
    //        else
    //        {
    //            elementType = field.FieldType;
    //            setter = field.SetValue;
    //        }

    //        // Shared resources get special treatment.
    //        if (contentSerializerAttribute != null && contentSerializerAttribute.SharedResource)
    //        {
    //            return (input, parent) =>
    //            {
    //                Action<object> action = value => setter(parent, value);
    //                input.ReadSharedResource(action);
    //            };
    //        }

    //        // We need to have a reader at this point.
    //        var reader = manager.GetTypeReader(elementType);
    //        if (reader == null)
    //            if (elementType == typeof(System.Array))
    //            {
    //                var assembly = Assembly.GetAssembly(typeof(ContentTypeReader));

    //                var type = assembly.GetType("Microsoft.Xna.Framework.Content.ArrayReader`1").MakeGenericType(typeof(Array));

    //                reader = Activator.CreateInstance(type) as ContentTypeReader;

    //                //reader = new ArrayReader<Array>();
    //            }
    //            else
    //                throw new ContentLoadException(string.Format("Content reader could not be found for {0} type.", elementType.FullName));

    //        // We use the construct delegate to pick the correct existing 
    //        // object to be the target of deserialization.
    //        Func<object, object> construct = parent => null;
    //        if (property != null && !property.CanWrite)
    //            construct = parent => property.GetValue(parent, null);

    //        return (input, parent) =>
    //        {
    //            var existing = construct(parent);
    //            var obj2 = input.ReadObject(reader, existing);
    //            setter(parent, obj2);
    //        };
    //    }

    //    public object Read(ContentReader input, object existingInstance)
    //    {
    //        T obj;
    //        if (existingInstance != null)
    //            obj = (T)existingInstance;
    //        else
    //            obj = (_constructor == null ? (T)Activator.CreateInstance(typeof(T)) : (T)_constructor.Invoke(null));

    //        //if (_baseTypeReader != null)
    //        //{

    //        //    _baseTypeReader.Read(input, obj);

    //        //}
    //        // Box the type.
    //        var boxed = (object)obj;

    //        foreach (var reader in _readers)
    //            reader(input, boxed);

    //        // Unbox it... required for value types.
    //        obj = (T)boxed;

    //        return obj;
    //    }
    //}


}
