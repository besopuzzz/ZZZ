using System.Collections;

namespace ZZZ.Framework.Core.Updating
{
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

        public UpdateOrderType this[Type type]
        {
            get
            {
                return types.Find(x => x.ComponentType == type);
            }
        }

        public UpdateOrderType Add(Type type, int order)
        {
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

        public void Remove(Type type)
        {
            var orderType = this[type];

            if (orderType == null)
                return;

            types.Remove(orderType);

            orderType.Order = 0;
        }

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
