using System.ComponentModel;
using System.Reflection;
using ZZZ.Framework.Attributes;
using static ZZZ.Framework.Components.Attributes.RequiredComponentAttribute;

namespace ZZZ.Framework.Components.Attributes
{
    internal sealed class RequiredComponentFactory : AttributeFactory<RequiredComponentAttribute>
    {
        private BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        protected override IEnumerable<AttributeInfo<RequiredComponentAttribute>> GetMembers<T>(AttributeTargets attributeTargets)
        {
            var info = new List<AttributeInfo<RequiredComponentAttribute>>();

            if (attributeTargets.HasFlag(AttributeTargets.Class))
                info.AddRange(FindOnClass<T>());

            if (attributeTargets.HasFlag(AttributeTargets.Field))
                info.AddRange(FindFields<T>());

            if (attributeTargets.HasFlag(AttributeTargets.Property))
                info.AddRange(FindProperties<T>());

            return info;
        }

        public IEnumerable<IComponent> Process<T>(GameObject gameObject, IComponent component)
            where T : IComponent
        {
            Cache<T>();

            List<IComponent> addAfterList = new List<IComponent>();

            foreach (var item in GetCached<T>(AttributeTargets.Class))
            {
                if (item?.Type == null)
                    continue; // Empty attribute, do nothing

                if (gameObject.GetComponent(item.Type) != null)
                    continue;

                if (item.Type == component.GetType())
                    continue; // An attempt to add a required component of the same type

                IComponent required = Activator.CreateInstance(item.Type) as IComponent;

                if (item.AddingOrder == AddingOrderType.Before)
                    component.Owner.AddComponent(required); // Add now
                else addAfterList.Add(required); // This adding later
            }

            foreach (var item in GetCached<T>(AttributeTargets.Field))
            {
                if (item.Type == component.GetType())
                    continue; // An attempt to add a required component of the same type

                IComponent required = gameObject.GetComponent(item.Type);

                if (required == null)
                {
                    required = Activator.CreateInstance(item.Type) as IComponent;
                    gameObject.AddComponent(required);
                }

                item.Set(component, required);
            }

            foreach (var item in GetCached<T>(AttributeTargets.Property))
            {
                if (item.Type == component.GetType())
                    continue; // An attempt to add a required component of the same type

                IComponent required = gameObject.GetComponent(item.Type);

                if (required == null)
                {
                    required = Activator.CreateInstance(item.Type) as IComponent;
                    gameObject.AddComponent(required);
                }

                item.Set(component, required);
            }

            return addAfterList;
        }

        private IEnumerable<AttributeInfo<RequiredComponentAttribute>> FindOnClass<T>()
        {
            var target = typeof(T);

            var attributeCache = new List<AttributeInfo<RequiredComponentAttribute>>();

            foreach (var attribute in target.GetCustomAttributes<RequiredComponentAttribute>())
                attributeCache.Add(new AttributeInfo<RequiredComponentAttribute>(attribute, target, AttributeTargets.Class));

            return attributeCache;

        }

        private IEnumerable<AttributeInfo<RequiredComponentAttribute>> FindFields<T>()
        {
            var target = typeof(T);

            var attributeCache = new List<AttributeInfo<RequiredComponentAttribute>>();

            var fields = target.GetFields(flags);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttributes<RequiredComponentAttribute>().FirstOrDefault();

                if (attribute != null)
                    attributeCache.Add(new AttributeInfo<RequiredComponentAttribute>(attribute, field, AttributeTargets.Field)); // Add First

                continue;
            }

            return attributeCache;
        }

        private IEnumerable<AttributeInfo<RequiredComponentAttribute>> FindProperties<T>()
        {
            var target = typeof(T);

            var attributeCache = new List<AttributeInfo<RequiredComponentAttribute>>();

            var properties = target.GetProperties(flags);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes<RequiredComponentAttribute>().FirstOrDefault();

                if (attribute != null)
                    attributeCache.Add(new AttributeInfo<RequiredComponentAttribute>(attribute, property, AttributeTargets.Property)); // Add First

                continue;
            }

            return attributeCache;
        }
    }
}
