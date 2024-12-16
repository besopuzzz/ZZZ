using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Rendering.Components;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Updating;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Physics.Aether.Components;

namespace ZZZ.Framework.Components.Physics.Aether
{
    public class AetherSystem : System, IUpdateable, IGameComponent
    {
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                if (base.Enabled == value)
                    return;

                base.Enabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int UpdateOrder
        {
            get => order;
            set
            {
                if (order == value)
                    return;

                order = value;

                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> EnabledChanged;

        public World World { get; } = new World(Vector2.Zero);


        private int order = 0;
        private List<BodyComponent> cache = new List<BodyComponent>();

        public void Update(GameTime gameTime)
        {
            cache.ForEach(x => x.UpdatePosition());

            World.Step(gameTime.ElapsedGameTime);

            cache.ForEach(x => x.UpdateTransformer());
        }

        void IGameComponent.Initialize()
        {

        }

        protected override void Input(IEnumerable<Component> components)
        {
            //foreach (var component in components.Where(x => x is BodyComponent).Cast<BodyComponent>())
            //{
            //    SignalMessenger.SendToParents<IGroupRenderer>(((Component)component).Owner, this, (x, y) =>
            //    {
            //        x.Context.AddToQueue(component);

            //        return true;
            //    });
            //}
            foreach (var component in components.Where(x => x is BodyComponent).Cast<BodyComponent>())
            {
                World.Add(component.Body);
                cache.Add(component);
            }

            base.Input(components);
        }

        protected override void Output(Component component)
        {
            if (component is BodyComponent bodyComponent)
            {
                cache.Remove(bodyComponent);
                World.Remove(bodyComponent.Body);
            }

            base.Output(component);
        }
    }
}
