using System.Diagnostics;
using System.Reflection;
using ZZZ.Framework.Attributes;

namespace ZZZ.Framework.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredComponentAttribute : Attribute, IComponentAttribute<RequiredComponentAttribute>
    {
        public AddingOrderType AddingOrder => addingOrderType;
        public Type Type => type;
        public Type DeclaringType => declaringType;
        public string Name => name;
        public MemberTypes MemberType => memberType;

        private Action<object, object> set;
        private Func<object, object> get;
        private Type type;
        private Type declaringType;
        private string name;
        private AddingOrderType addingOrderType = AddingOrderType.Before;
        private MemberTypes memberType;

        public enum AddingOrderType
        {
            /// <summary>
            /// Обязательный компонент должен быть добавлен после основного компонента.
            /// </summary>
            After,

            /// <summary>
            /// Обязательный компонент должен быть добавлен до основного компонента.
            /// </summary>
            Before
        }

        public RequiredComponentAttribute(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!type.IsAssignableTo(typeof(Component)))
                throw new ArgumentException($"Type {type} is not inherited from {typeof(IComponent)}!");
            
            this.type = type;
        }

        public object Get(object obj)
        {
            return get?.Invoke(obj);
        }

        public void Set(object obj, object value)
        {
            set?.Invoke(obj, value);
        }

        public virtual void Initialize(AttributeInfo<RequiredComponentAttribute> attributeInfo)
        {
            var memberInfo = attributeInfo.MemberInfo;

            name = memberInfo.Name;
            declaringType = memberInfo.DeclaringType;
            memberType = memberInfo.MemberType;

            if (memberInfo.MemberType == MemberTypes.TypeInfo)
                return;

            Type componentType = default;

            if (memberInfo is FieldInfo fieldInfo)
            {
                componentType = fieldInfo.FieldType;
                get = fieldInfo.GetValue;
                set = fieldInfo.SetValue;
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                componentType = propertyInfo.PropertyType;
                get = propertyInfo.GetValue;
                set = propertyInfo.SetValue;
            }
            else Debug.Assert(true);

            if(!typeof(IComponent).IsAssignableFrom(componentType))
                throw new ArgumentException($"Type {type} is not inherited from {typeof(IComponent)}!");

            if (type != null)
            {
                if (type == componentType)
                    return;

                if (!type.IsAssignableTo(componentType))
                    throw new ArgumentException($"Type {type} is not inherited from {componentType}!");

                return;
            }

            type = componentType;
        }
    }

    public class RequiredComponentAttribute<T> : RequiredComponentAttribute
        where T : Component
    {
        public RequiredComponentAttribute() : base(typeof(T))
        {
        }
    }
}

