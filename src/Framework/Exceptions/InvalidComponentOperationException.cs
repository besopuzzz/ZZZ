using ZZZ.Framework.Components;

namespace ZZZ.Framework.Exceptions
{
    public class InvalidComponentOperationException : Exception
    {
        public Component Component { get; }

        public InvalidComponentOperationException()
        {

        }

        public InvalidComponentOperationException(Component component, string message) : base(message)
        {
            Component = component;
        }

        public static void ThrowIfOwnerNull(Component component)
        {
            if (component.FactOwner == null)
                throw new InvalidComponentOperationException(component, "Компонент еще не добавлен в родительский контейнер!");
        }

        public static void ThrowIfIsNotComponentOrAbstract(Type type)
        {
            if (type.IsInterface | type.IsAbstract | !type.IsAssignableTo(typeof(Component)))
                throw new InvalidComponentOperationException(null, $"Указанный тип ${type} " +
                    $" является абстрактным/интерфейсным или непроизводным от ${typeof(Component)}!");
        }
    }
}
