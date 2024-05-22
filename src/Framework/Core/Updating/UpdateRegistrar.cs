using ZZZ.Framework.Core.Registrars;
using ZZZ.Framework.Core.Updating.Components;

namespace ZZZ.Framework.Core.Updating
{
    public class UpdateRegistrar : BaseRegistrar<IUpdateComponent>, IAnyRegistrar<IUpdateComponent>
    {
        public UpdateOrderList OrderTypes { get; }

        private readonly UpdateComponents updateComponentZs;

        public UpdateRegistrar()
        {
            OrderTypes = new UpdateOrderList();
            updateComponentZs = new UpdateComponents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                updateComponentZs.Clear();
            }

            base.Dispose(disposing);
        }

        void IAnyRegistrar<IUpdateComponent>.Reception(IUpdateComponent component)
        {
            updateComponentZs.Add(new UpdateComponent(OrderTypes.Get(component.GetType()), component));
        }

        void IAnyRegistrar<IUpdateComponent>.Departure(IUpdateComponent component)
        {
            updateComponentZs.Remove(updateComponentZs[x => x.Component == component]);
        }

        protected override void Update(GameTime gameTime)
        {
            updateComponentZs.Invalidate();

            foreach (var item in updateComponentZs)
            {
                item.Update(gameTime);
            }
        }
    }
}
