using ZZZ.Framework.Components;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.Designing.UnityStyle.Systems
{
    public abstract class UpdaterSystem : System
    {
        protected abstract UpdateOrderList UpdateOrderTypes { get; }

        private readonly List<UpdateComponent> updateComponents = new List<UpdateComponent>();
        private readonly List<UpdateComponent> toAddComponents = new List<UpdateComponent>();

        protected void Update(TimeSpan time)
        {
            while (toAddComponents.Count > 0)
            {
                var component = toAddComponents.First();

                updateComponents.Add(component);

                toAddComponents.Remove(component);
            }

            updateComponents.Sort();

            foreach (var component in updateComponents)
                component.Update(time);
        }

        protected override void Input(IEnumerable<Component> components)
        {
            foreach (var updater in components.Where(x => x is IUpdater).Cast<IUpdater>())
                toAddComponents.Add(new UpdateComponent(updater, UpdateOrderTypes.Get(updater.GetType())));

            base.Input(components);
        }

        protected override void Output(Component component)
        {
            if (component is IUpdater updater)
            {
                var updateComponent = new UpdateComponent(updater, UpdateOrderTypes.Get(updater.GetType()));

                if (!toAddComponents.Remove(updateComponent))
                    updateComponents.Remove(updateComponent);
            }

            base.Output(component);
        }

        private class UpdateComponent : IComparable<UpdateComponent>, IEquatable<UpdateComponent>
        {
            public IUpdater Updater => component;
            public int Order => orderType.Order;

            private readonly IUpdater component;
            private readonly UpdateOrderType orderType;
            private readonly Comparer<int> comparer = Comparer<int>.Default;

            public UpdateComponent(IUpdater updater, UpdateOrderType updateOrderType)
            {
                component = updater;
                orderType = updateOrderType;
            }

            public void Update(TimeSpan time)
            {
                component.Update(time);
            }
            public int CompareTo(UpdateComponent other)
            {
                return comparer.Compare(Order, other.Order);
            }

            public bool Equals(UpdateComponent other)
            {
                return component == other.component;
            }
        }
    }
}
