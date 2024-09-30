using System.Collections;
using ZZZ.Framework.Components.Updating;

namespace ZZZ.Framework.Core.Updating
{
    /// <summary>
    /// Представляет класс сортировщика обновляемых компонентов.
    /// </summary>
    /// <remarks>Используйте методы <see cref="Add(Type, int)"/> и <see cref="Remove(Type)"/> для указания порядка выполнения обновления компонентов.</remarks>
    public sealed class UpdateOrderList : IEnumerable<UpdateOrderType>
    {
        //[ContentSerializer(ElementName = "Types")]
        private List<UpdateOrderType> types = new List<UpdateOrderType>();

        private List<UpdateOrderType> notAdded = new List<UpdateOrderType>();

        internal UpdateOrderList()
        {

        }

        /// <summary>
        /// Получает экземпляр класса для указания порядка сортировки.
        /// </summary>
        /// <param name="type">Тип компонента.</param>
        /// <returns>Экземпляр класса-сортировки.</returns>
        public UpdateOrderType Get<T>()
            where T : IUpdateComponent => Get(typeof(T));

        /// <summary>
        /// Получает экземпляр класса для указания порядка сортировки.
        /// </summary>
        /// <param name="type">Тип компонента.</param>
        /// <returns>Экземпляр класса-сортировки.</returns>
        public UpdateOrderType Get(Type componentType)
        {
            ArgumentNullException.ThrowIfNull(componentType);

            if (!componentType.IsAssignableTo(typeof(IUpdateComponent)))
                throw new ArgumentException($"Type {componentType} is not inherited from IComponent!");

            var orderType = types.Find(x => x.ComponentType == componentType);

            if (orderType == null)
            {
                orderType = new UpdateOrderType(componentType, 0);
                notAdded.Add(orderType);
            }

            return orderType;
        }

        /// <summary>
        /// Добавляет тип сортировки.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="order">Порядок обновления.</param>
        public void Add<T>(int order)
            where T : IUpdateComponent
        {
            var type = typeof(T);
            var orderType = types.Find(x=>x.ComponentType == type);

            if (orderType == null)
            {
                orderType = notAdded.Find(x => x.ComponentType == type);

                if(orderType == null)
                    orderType = new UpdateOrderType(type, order);
                else
                {
                    notAdded.Remove(orderType);
                }

                types.Add(orderType);
            }

            orderType.Order = order;

            types.Sort();
        }

        /// <summary>
        /// Удаляет тип сортировки.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        public void Remove<T>()
            where T : IUpdateComponent
        {
            var type = typeof(T);
            var orderType = types.Find(x => x.ComponentType == type);

            if (orderType == null)
                return;

            types.Remove(orderType);

            orderType.Order = 0;
        }

        /// <summary>
        /// Очищает все сортировщики.
        /// </summary>
        public void Clear()
        {
            types.Clear();
        }

        public IEnumerator<UpdateOrderType> GetEnumerator()
        {
            return ((IEnumerable<UpdateOrderType>)types).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)types).GetEnumerator();
        }
    }
}
