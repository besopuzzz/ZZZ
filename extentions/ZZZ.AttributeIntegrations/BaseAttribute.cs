
using System.Reflection;

namespace ZZZ.AttributeIntegrations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class BaseAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MethodAttribute : BaseAttribute, IParameterAttribute
    {
        public Type[] Parameters => parameters;

        protected static Type[] parameters;

        static MethodAttribute()
        {
            parameters = new Type[16];
        }
    }

    public class MethodAttribute<T1> : MethodAttribute, IParameterAttribute<T1>
    {
        static MethodAttribute()
        {
            parameters[0] = typeof(T1);
        }
    }

    public class MethodAttribute<T1,T2> : MethodAttribute, IParameterAttribute<T1, T2>
    {
        static MethodAttribute()
        {
            parameters[1] = typeof(T2);
        }
    }
}
