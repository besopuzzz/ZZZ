using System.Reflection;

namespace ZZZ.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class MethodComponentAttribute<TAttribute> : Attribute, IComponentAttribute<TAttribute>
        where TAttribute : IComponentAttribute<TAttribute>
    {
        private MethodInfo method;

        public MethodComponentAttribute()
        {

        }

        protected void Invoke(object obj, params object[] args)
        {
            method.Invoke(obj, args);
        }

        public void Initialize(AttributeInfo<TAttribute> attributeInfo)
        {
            method = attributeInfo.MemberInfo as MethodInfo;
        }
    }
}

