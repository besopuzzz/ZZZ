using System.Reflection;
using ZZZ.Framework.Components;

namespace ZZZ.Framework.Attributes
{
    public class WaitComponentFactory : AttributeFactory<WaitComponentAttribute>
    {
        private const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        protected override IEnumerable<AttributeInfo<WaitComponentAttribute>> GetMembers<T>(AttributeTargets attributeTargets)
        {
            var info = new List<AttributeInfo<WaitComponentAttribute>>();

            if (attributeTargets.HasFlag(AttributeTargets.Method))
                info.AddRange(FindOnClass<T>());

            return info;
        }

        internal void Process<T>(GameObject gameObject, T component) where T : IComponent
        {
            Cache<T>();

            foreach (var attribute in GetCached<T>(AttributeTargets.Method))
            {
                attribute.Bind(component, gameObject);
            }
        }


        internal void Dispose<T>(GameObject gameObject, T component) where T : IComponent
        {
            foreach (var attribute in GetCached<T>(AttributeTargets.Method))
            {
                attribute.Unbind();
            }
        }

        private IEnumerable<AttributeInfo<WaitComponentAttribute>> FindOnClass<T>()
        {
            var target = typeof(T);

            var attributeCache = new List<AttributeInfo<WaitComponentAttribute>>();

            foreach (var method in target.GetMethods(flags))
            foreach (var attribute in method.GetCustomAttributes<WaitComponentAttribute>())
                attributeCache.Add(new AttributeInfo<WaitComponentAttribute>(attribute, method, AttributeTargets.Method));

            return attributeCache;
        }

    }
}
