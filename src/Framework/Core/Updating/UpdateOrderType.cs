
namespace ZZZ.Framework.Core.Updating
{
    /// <summary>
    /// Представляет класс, позволяющий менять порядок обновления у указанного типа.
    /// </summary>
    public sealed class UpdateOrderType
    {
        /// <summary>
        /// Получает или устанавливает порядок обновления, относительно остальных компонентов.
        /// </summary>
        public int Order
        {
            get => order;
            set
            {
                if (order == value)
                    return;

                order = value;

                OrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Получает тип компонента.
        /// </summary>
        [ContentSerializerIgnore]
        public Type ComponentType { get; }

        internal event EventHandler<EventArgs> OrderChanged;

        private int order = 0;

        internal UpdateOrderType()
        {

        }

        internal UpdateOrderType(Type componentType, int order)
        {
            ComponentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
            Order = order;
        }

        public override string ToString()
        {
            return $"Order: {order}, type: {ComponentType.Name}";
        }
    }
}
