namespace ZZZ.Framework.Core.Updating
{
    /// <summary>
    /// Представляет класс, позволяющий менять порядок обновления у указанного типа.
    /// </summary>
    public sealed class UpdateOrderType : IComparable<UpdateOrderType>
    {
        /// <summary>
        /// Получает или устанавливает порядок обновления, относительно остальных компонентов.
        /// </summary>
        public int Order
        {
            get => order;
            set => order = value;
        }

        //[ContentSerializer]
        internal string TypeName
        {
            get => type.FullName;
            set
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(value);

                type = Type.GetType(value, true);
            }
        }

        /// <summary>
        /// Получает тип компонента.
        /// </summary>
        //[ContentSerializerIgnore]
        public Type ComponentType => type;

        private int order = 0;
        private Type type;
        private readonly Comparer<int> comparer = Comparer<int>.Default;

        internal UpdateOrderType()
        {

        }

        internal UpdateOrderType(Type componentType, int order)
        {
            type = componentType;
            Order = order;
        }

        public override string ToString()
        {
            return $"Order: {order}, Type: {ComponentType.Name}";
        }

        public int CompareTo(UpdateOrderType other)
        {
            return comparer.Compare(Order, other.Order);
        }
    }
}
