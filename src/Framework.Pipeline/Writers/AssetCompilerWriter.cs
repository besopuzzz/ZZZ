using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ.KNI.Content.Pipeline.Writers
{
    internal class AssetCompilerWriter
    {
        private List<PropertyInfo> _properties = new List<PropertyInfo>();
        private List<FieldInfo> _fields = new List<FieldInfo>();


        private string _runtimeType;
        private ContentCompiler _compiler;
        private static HashSet<MemberInfo> _sharedResources = new HashSet<MemberInfo>();

        protected int _typeVersion;

        public AssetCompilerWriter()
        {
        }

        public void Initialize(ContentCompiler compiler, Type assetType)
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

            _properties.AddRange( GetAllProperties(assetType).Where(IsValidProperty).ToArray());
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


        /// <inheritdoc/>
        public void OnAddedToContentWriter(ContentWriter output)
        {
            var method = output.GetType().GetMethod("GetTypeWriter", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var property in _properties)
                method.Invoke( output, new object[1] { property.PropertyType });

            foreach (var field in _fields)
                method.Invoke(output, new object[1] { field.FieldType });
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
                    if (!_compiler.GetTypeWriter(property.PropertyType).CanDeserializeIntoExistingObject)
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

        private static void Write(object parent, ContentWriter output, MemberInfo member)
        {
            var property = member as PropertyInfo;
            var field = member as FieldInfo;
            Debug.Assert(field != null || property != null);
            
            Type elementType;
            object memberObject;

            if (property != null)
            {
                elementType = property.PropertyType;
                memberObject = property.GetValue(parent, null);
            }
            else
            {
                elementType = field.FieldType;
                memberObject = field.GetValue(parent);
            }

            if (_sharedResources.Contains(member))
                output.WriteSharedResource(memberObject);
            else
            {
                var method = output.GetType().GetMethod("GetTypeWriter", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var  writer = method.Invoke(output, new object[1] { property.PropertyType }) as ContentTypeWriter;

                if (writer == null || elementType == typeof(object) || elementType == typeof(Array))
                    output.WriteObject(memberObject);
                else
                    output.WriteObject(memberObject, writer);
            }
        }

        public string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return _runtimeType;
        }

        public string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Microsoft.Xna.Framework.Content.ReflectiveReader`1[[" +
                        GetRuntimeType(targetPlatform)
                    + "]]";
        }

        public void Write(ContentWriter output, object value)
        {
            foreach (var property in _properties)
                Write(value, output, property);

            foreach (var field in _fields)
                Write(value, output, field);
        }
    }
}
