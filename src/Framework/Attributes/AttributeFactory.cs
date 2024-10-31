using System.Reflection;

namespace ZZZ.Framework.Attributes
{
    public abstract class AttributeFactory<TAttribute>
        where TAttribute : IComponentAttribute<TAttribute>
    {
        private readonly Dictionary<Type, Dictionary<AttributeTargets, List<TAttribute>>> cache = new Dictionary<Type, Dictionary<AttributeTargets, List<TAttribute>>>();

        public void Cache<T>()
        {
            var target = typeof(T);

            if (cache.ContainsKey(target))
                return;

            var attributes = new Dictionary<AttributeTargets, List<TAttribute>>();

            cache.Add(target, attributes);

            var attributeUsage = typeof(TAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            var mask = Enum.GetValues<AttributeTargets>().Where(x => attributeUsage.ValidOn.HasFlag(x));

            foreach (var targetMask in mask)
            {
                var attributesInfo = new List<TAttribute>();

                var members = GetMembers<T>(targetMask);

                foreach (var item in members)
                {
                    var attribute = item.Attribute;

                    attribute.Initialize(item);

                    attributesInfo.Add(attribute);
                }

                attributes.Add(targetMask, attributesInfo);
            }
        }

        public IEnumerable<TAttribute> CacheAndGet<T>(AttributeTargets attributeTargets)
        {
            Cache<T>();

            return GetCached<T>(attributeTargets);
        }

        public void ClearCache()
        {
            foreach (var ch in cache)
            {
                foreach (var attributes in ch.Value)
                {
                    attributes.Value.Clear();
                }
            }

            cache.Clear();
        }

        public IEnumerable<TAttribute> GetCached<T>(AttributeTargets attributeTargets)
        {
            if(cache.TryGetValue(typeof(T), out var targets))
                if (targets.TryGetValue(attributeTargets, out var attributes))
                    return attributes;

            return Enumerable.Empty<TAttribute>();
        }


        public IEnumerable<TAttribute> GetAllCached<T>()
        {
            if (cache.TryGetValue(typeof(T), out var targets))
            {
                var list = new List<TAttribute>();

                foreach (var attributes in targets)
                {
                    list.AddRange(attributes.Value);
                }

                return list;
            } 

            return Enumerable.Empty<TAttribute>();
        }

        protected abstract IEnumerable<AttributeInfo<TAttribute>> GetMembers<T>(AttributeTargets attributeTargets);

    }
}

