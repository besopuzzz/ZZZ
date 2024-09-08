using ZZZ.Framework.Core.Updating.Components;

namespace ZZZ.Framework.Core.Updating
{
    public class UpdateSystem : System<IUpdateComponent, UpdateEntity>
    {
        public UpdateOrderList OrderTypes => orderTypes;

        private static Comparer<int> comparer = Comparer<int>.Default;
        private UpdateOrderList orderTypes = new UpdateOrderList();

        protected override UpdateEntity OnProcess(IUpdateComponent component)
        {
            return new UpdateEntity(orderTypes, component);
        }

        protected override void Update(GameTime gameTime)
        {
            List<UpdateEntity> entities = new List<UpdateEntity>();

            foreach (var item in Entities)
            {
                if (item.Enabled)
                    entities.Add(item);
            }

            entities.Sort((x, y) => comparer.Compare(x.Order, y.Order));

            foreach (var item in entities)
            {
                item.Update(gameTime);
            }

            base.Update(gameTime);
        }
    }

    public class UpdateEntity : Entity<IUpdateComponent, UpdateEntity>
    {
        public int Order => orderTypes[Component.GetType()]?.Order ?? 0;

        private UpdateOrderList orderTypes;

        public UpdateEntity(UpdateOrderList orderTypes, IUpdateComponent component) : base(component)
        {
            this.orderTypes = orderTypes;
        }

        public virtual void Update(GameTime gameTime)
        {
            Component.Update(gameTime);
        }


    }
}
