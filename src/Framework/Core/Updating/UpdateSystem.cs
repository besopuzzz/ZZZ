using ZZZ.Framework.Components.Updating;

namespace ZZZ.Framework.Core.Updating
{
    public class UpdateSystem : System<UpdateEntity, UpdateEntityComponent, IUpdateComponent>
    {
        [ContentSerializer]
        public UpdateOrderList OrderTypes => orderTypes;

        private UpdateOrderList orderTypes = new UpdateOrderList();
        private readonly List<UpdateEntityComponent> components = new List<UpdateEntityComponent>();

        protected override UpdateEntity CreateEntity(UpdateEntity owner)
        {
            return new UpdateEntity();
        }

        protected override UpdateEntityComponent CreateEntityComponent(UpdateEntity owner, IUpdateComponent component)
        {
            return new UpdateEntityComponent(orderTypes, component);
        }

        protected override void Update(GameTime gameTime)
        {
            components.Clear();

            ForEveryComponent(component =>
            {
                if (component.Enabled)
                    components.Add(component);
            });

            components.Sort();

            foreach (var component in components)
                component.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
