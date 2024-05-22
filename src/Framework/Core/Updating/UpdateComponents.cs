using System.Collections;

namespace ZZZ.Framework.Core.Updating
{
    internal sealed class UpdateComponents : IEnumerable<UpdateComponent>
    {
        private struct UpdateComponentEntry
        {
            public static Comparer<int> Comparer { get; } = Comparer<int>.Default;

            public readonly UpdateComponent Component;

            private readonly int AddOrder;

            public UpdateComponentEntry(UpdateComponent component, int addOrder)
            {
                Component = component;
                AddOrder = addOrder;
            }
            public override bool Equals(object obj)
            {
                if (obj is UpdateComponentEntry compoentEntry)
                    return Equals(Component, compoentEntry.Component);

                return false;
            }
            public int Compare(UpdateComponentEntry other)
            {
                int num = Comparer.Compare(Component.UpdateOrder, other.Component.UpdateOrder);

                if (num == 0)
                    num = AddOrder - other.AddOrder;

                return num;
            }
            public override int GetHashCode()
            {
                return Component.GetHashCode();
            }
        }
        private readonly List<UpdateComponent> components = new List<UpdateComponent>();
        private readonly List<int> notRemovedIndexes = new List<int>();
        private readonly List<UpdateComponentEntry> notAddedComponents = new List<UpdateComponentEntry>();
        private int notAddedCount;

        public UpdateComponent this[Predicate<UpdateComponent> predicate]
        {
            get
            {
                var item = notAddedComponents.Find(x => predicate.Invoke(x.Component)).Component;

                if (item == null)
                    item = components.Find(predicate);

                return item;
            }
        }

        public void Add(UpdateComponent component)
        {
            notAddedComponents.Add(new UpdateComponentEntry(component, notAddedCount++));
        }
        public void Remove(UpdateComponent component)
        {
            if (notAddedComponents.Remove(new UpdateComponentEntry(component, -1)))
                return;

            int num = components.IndexOf(component);

            if (num < 0)
                return;

            UnsubscribEvents(component);

            notRemovedIndexes.Add(num);
        }
        public void Clear()
        {
            foreach (var component in components)
                UnsubscribEvents(component);

            notAddedComponents.Clear();
            notRemovedIndexes.Clear();

            foreach (var component in components)
            {
                component.Unbind();
            }

            components.Clear();
        }

        public void Invalidate()
        {
            if (notRemovedIndexes.Count > 0)
                RemoveNotRemovedComponents();

            if (notAddedComponents.Count > 0)
                AddNotAddedComponents();

        }

        public IEnumerator<UpdateComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        private void Component_OrderChanged(object sender, EventArgs e)
        {
            UpdateComponent component = (UpdateComponent)sender;

            UnsubscribEvents(component);

            notAddedComponents.Add(new UpdateComponentEntry(component, notAddedCount++));

            int item = components.IndexOf(component);

            notRemovedIndexes.Add(item);
        }

        private void SubscribEvents(UpdateComponent component)
        {
            component.UpdateOrderChanged += Component_OrderChanged;
        }
        private void UnsubscribEvents(UpdateComponent component)
        {
            component.UpdateOrderChanged -= Component_OrderChanged;
        }

        private void RemoveNotRemovedComponents()
        {
            notRemovedIndexes.Sort();

            for (int num = notRemovedIndexes.Count - 1; num >= 0; num--)
            {
                components[notRemovedIndexes[num]].Unbind();
                components.RemoveAt(notRemovedIndexes[num]);
            }

            notRemovedIndexes.Clear();
        }
        private void AddNotAddedComponents()
        {
            notAddedComponents.Sort((x, y) => x.Compare(y));

            notAddedCount = 0;

            int i = 0;

            for (int j = 0; j < components.Count; j++)
            {
                if (i >= notAddedComponents.Count)
                    break;

                UpdateComponent component = notAddedComponents[i].Component;

                if (Comparer<int>.Default.Compare(component.UpdateOrder, components[j].UpdateOrder) < 0)
                {
                    SubscribEvents(component);

                    components.Insert(j, component);
                    component.Bind();

                    i++;
                }
            }

            for (; i < notAddedComponents.Count; i++)
            {
                UpdateComponent component = notAddedComponents[i].Component;

                SubscribEvents(component);

                components.Add(component);
                component.Bind();
            }

            notAddedComponents.Clear();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}