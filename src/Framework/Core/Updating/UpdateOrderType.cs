
namespace ZZZ.Framework.Core.Updating
{
    public sealed class UpdateOrderType
    {
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
