using System.Reflection;

namespace ZZZ.Framework.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SerializeAttribute : Attribute
    {
        private const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static Dictionary<Type, List<MemberInfo>> cache = new Dictionary<Type, List<MemberInfo>>();

        public SerializeAttribute() 
        {


        }

        public static List<MemberInfo> GetMembers(Type type)
        {
            List<MemberInfo> members = new List<MemberInfo>();

            if (type.GetCustomAttribute<SerializeAttribute>() == null)
                return members;

            if (cache.TryGetValue(type, out var value))
                return value;

            PropertyInfo[] properties = type.GetProperties(flags);

            foreach (PropertyInfo member in properties)
            {
                if (member.GetCustomAttribute<SerializeAttribute>() != null)
                    members.Add(member);
            }

            FieldInfo[] fields = type.GetFields(flags);

            foreach (FieldInfo member in fields)
            {
                if (member.GetCustomAttribute<SerializeAttribute>() != null)
                    members.Add(member);
            }

            cache.Add(type, members);

            return members;
        }


    }
}
