using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredComponentAttribute : Attribute
    {
        public Type Type { get; }

        public bool Remove { get; }

        public AutoReferenceComponent AutoReference => autoReference;

        private readonly AutoReferenceComponent autoReference;
        private static Dictionary<Type,AutoReferenceComponent> cache = new Dictionary<Type, AutoReferenceComponent>();

        public RequiredComponentAttribute(Type type, bool remove = false)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!type.IsAssignableTo(typeof(Component)))
                throw new ArgumentException($"Type {type} is not inherited from {typeof(Component)}!");

            Type = type;
            Remove = remove;
        }

        public RequiredComponentAttribute(Type type, bool remove = false, Type autoReferenceType = null) : this(type, remove)
        {
            if (autoReferenceType == null)
                return;

            if (!cache.TryGetValue(autoReferenceType, out autoReference))
            {
                autoReference = Activator.CreateInstance(autoReferenceType, true) as AutoReferenceComponent;

                cache[autoReferenceType] = autoReference;
            }
        }
    }

    public class RequiredComponentAttribute<T> : RequiredComponentAttribute
        where T : Component, new()
    {
        public RequiredComponentAttribute(bool remove = false) : base(typeof(T), remove)
        {
        }
        public RequiredComponentAttribute(Type typeAutoReference) : base(typeof(T), false, typeAutoReference)
        {
        }
    }

    public class RequiredComponentAttribute<T, A> : RequiredComponentAttribute<T>
        where T : Component, new()
        where A : AutoReferenceComponent<T>, new()
    {
        public RequiredComponentAttribute() : base(typeof(A))
        {
        }
    }
}

