using ZZZ.Framework.Components.Updating;

namespace ZZZ.Framework.Core.Updating
{
    public class UpdateEntityComponent : EntityComponent<UpdateEntity, UpdateEntityComponent, IUpdateComponent>, IComparable<UpdateEntityComponent>
    {
        public int Order => orderType.Order;

        private UpdateOrderType orderType;
        private readonly Comparer<int> comparer = Comparer<int>.Default;

        public UpdateEntityComponent(UpdateOrderList orderTypes, IUpdateComponent component) : base(component)
        {
            orderType = orderTypes.Get(component.GetType());
        }

        public void Update(GameTime gameTime)
        {
            Component.Update(gameTime);
        }

        public int CompareTo(UpdateEntityComponent other)
        {
            return comparer.Compare(Order, other.Order);
        }
    }
}
