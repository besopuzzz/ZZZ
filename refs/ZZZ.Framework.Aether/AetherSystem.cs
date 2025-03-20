using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Physics.Aether.Components;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.Physics.Aether
{
    public class AetherSystem : System, ISystemUpdater
    {
        public World World { get; } = new World(Vector2.Zero);

        private List<BodyComponent> cache = new List<BodyComponent>();
        private List<ILateUpdater> updaters = new List<ILateUpdater>();

        public void Update(TimeSpan gameTime)
        {
            cache.ForEach(x=>x.UpdatePosition());

            World.Step(gameTime);

            cache.ForEach(x => x.UpdateTransformer());

            foreach (var updater in updaters)
            {
                updater.LateUpdate();
            }
        }

        protected override void Input(IEnumerable<Component> components)
        {
            foreach (var component in components.Where(x => x is BodyComponent).Cast<BodyComponent>())
            {
                bool attached = false;

                SignalMessenger.SendToParents<BodyComponent>(((Component)component).Owner, this, (x, y) =>
                {
                    x.Attach(component);

                    attached = true;

                    return true;
                });

                if (!attached)
                    cache.Add(component);

                World.Add(component.Body);
            }

            base.Input(components);
        }

        protected override void Output(Component component)
        {
            if (component is BodyComponent bodyComponent)
            {
                bool detached = false;

                SignalMessenger.SendToParents<BodyComponent>(((Component)component).Owner, this, (x, y) =>
                {
                    x.Detach(bodyComponent);

                    detached = true;

                    return true;
                });

                if (!detached)
                    cache.Remove(bodyComponent);

                World.Remove(bodyComponent.Body);
            }

            base.Output(component);
        }

        //protected override void Input(IEnumerable<Component> components)
        //{
        //    foreach (var component in components.Where(x => x is ILateUpdater).Cast<ILateUpdater>())
        //    {
        //        updaters.Add(component);
        //    }

        //    foreach (var component in components.Where(x => x is BodyComponent).Cast<BodyComponent>())
        //    {
        //        World.Add(component.Body);
        //        cache.Add(component);
        //    }

        //    base.Input(components);
        //}

        //protected override void Output(Component component)
        //{
        //    if (component is BodyComponent bodyComponent)
        //    {
        //        cache.Remove(bodyComponent);
        //        World.Remove(bodyComponent.Body);
        //    }

        //    if(component is ILateUpdater updater)
        //        updaters.Remove(updater);

        //    base.Output(component);
        //}
    }
}
