using System.Collections;

namespace ZZZ.Framework.Core.Updating
{
    /// <summary>
    /// Представляет класс сортировщика обновляемых компонентов.
    /// </summary>
    /// <remarks>Используйте методы <see cref="Add(Type, int)"/> и <see cref="Remove(Type)"/> для указания порядка выполнения обновления компонентов.</remarks>
    public sealed class UpdateOrderList : IEnumerable<UpdateOrderType>
    {
        [ContentSerializer(ElementName = "Orders")]
        private List<UpdateOrderType> types = new List<UpdateOrderType>();

        private List<UpdateOrderType> defaults = new List<UpdateOrderType>();

        private readonly Comparer<int> comparer = Comparer<int>.Default;

        internal UpdateOrderList()
        {

        }

        internal UpdateOrderType Get(Type type)
        {
            var orderType = this[type];

            if (orderType == null)
            {
                orderType = defaults.Find(x => x.ComponentType == type);

                if (orderType == null)
                {
                    orderType = new UpdateOrderType(type, 0);
                    defaults.Add(orderType);
                }
            }

            return orderType;
        }

        internal void AddDefault(Type type, int order)
        {
            defaults.Add(new UpdateOrderType(type, order));
        }

        internal void ClearDefaults()
        {
            defaults.Clear();
        }

        private void ThrowIfNotValid(Type type)
        {
            if (!type.IsAssignableFrom(typeof(IComponent)))
                throw new ArgumentException($"Type {type} is not inherited from IComponent!");

        }

        /// <summary>
        /// Получает экземпляр класса для указания порядка сортировки.
        /// </summary>
        /// <param name="type">Тип компонента.</param>
        /// <returns>Экземпляр класса-сортировки.</returns>
        public UpdateOrderType this[Type type]
        {
            get
            {
                return types.Find(x => x.ComponentType == type);
            }
        }

        /// <summary>
        /// Добавляет тип к сортировки.
        /// </summary>
        /// <param name="type">Тип компонента.</param>
        /// <param name="order">Порядок обновления.</param>
        /// <returns>Экземпляр класса-сортировки.</returns>
        public UpdateOrderType Add(Type type, int order)
        {
            ThrowIfNotValid(type);

            var orderType = defaults.Find(x=> x.ComponentType == type);

            if(orderType != null)
            {
                types.Add(orderType);
            }
            else
            {
                orderType = this[type];

                if (orderType == null)
                {
                    orderType = new UpdateOrderType(type, order);

                    defaults.Add(orderType);
                    types.Add(orderType);
                }
            }

            orderType.Order = order;

            types.Sort((x, y) => comparer.Compare(x.Order, y.Order));

            return orderType;
        }

        /// <summary>
        /// Удаляет тип компонента из сортировщика.
        /// </summary>
        /// <param name="type">Тип компонента.</param>
        public void Remove(Type type)
        {
            ThrowIfNotValid(type);

            var orderType = this[type];

            if (orderType == null)
                return;

            types.Remove(orderType);

            orderType.Order = 0;
        }

        /// <summary>
        /// Очищает сортировщик от всех типов.
        /// </summary>
        public void Clear()
        {
            foreach (var item in types.ToList())
            {
                types.Remove(item);

                item.Order = 0;
            }
        }

        public IEnumerator<UpdateOrderType> GetEnumerator()
        {
            return types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return types.GetEnumerator();
        }
    }
}
