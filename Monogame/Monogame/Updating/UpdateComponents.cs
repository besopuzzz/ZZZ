using Microsoft.Xna.Framework;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Updating
{
    internal class UpdateComponents
    {
        private readonly List<IUpdateComponent> _updateableComponents = new List<IUpdateComponent>();
        private readonly List<IUpdateComponent> _enabledComponents = new List<IUpdateComponent>();
        private bool _isEnabledCacheInvalidated = true;

        private readonly List<UpdateableJournalEntry> _addUpdateableJournal = new List<UpdateableJournalEntry>();
        private readonly List<int> _removeUpdateableJournal = new List<int>();
        private int _addUpdateableJournalCount;

        public void Add(IUpdateComponent component)
        {
            _addUpdateableJournal.Add(new UpdateableJournalEntry(component, _addUpdateableJournalCount++));
            _isEnabledCacheInvalidated = true;
        }

        public void Remove(IUpdateComponent component)
        {
            if (_addUpdateableJournal.Remove(new UpdateableJournalEntry(component, -1)))
                return;

            var index = _updateableComponents.IndexOf(component);
            if (index >= 0)
            {
                component.EnabledChanged -= (sender, value) => Component_EnabledChanged(sender, EventArgs.Empty);
                component.UpdateOrderChanged -= Component_UpdateOrderChanged;

                _removeUpdateableJournal.Add(index);
                _isEnabledCacheInvalidated = true;
            }
        }

        public bool Contains(IUpdateComponent component) => _updateableComponents.Contains(component);

        public void Clear()
        {
            for (int i = 0; i < _updateableComponents.Count; i++)
            {
                _updateableComponents[i].EnabledChanged -= (sender, value) => Component_EnabledChanged(sender, EventArgs.Empty);
                _updateableComponents[i].UpdateOrderChanged -= Component_UpdateOrderChanged;
            }

            _addUpdateableJournal.Clear();
            _removeUpdateableJournal.Clear();
            _updateableComponents.Clear();

            _isEnabledCacheInvalidated = true;
        }

        private void Component_EnabledChanged(object sender, EventArgs e)
        {
            _isEnabledCacheInvalidated = true;
        }

        private void Component_UpdateOrderChanged(object sender, EventArgs e)
        {
            var component = (IUpdateComponent)sender;
            var index = _updateableComponents.IndexOf(component);

            _addUpdateableJournal.Add(new UpdateableJournalEntry(component, _addUpdateableJournalCount++));

            component.EnabledChanged -= (sender, value) => Component_EnabledChanged(sender, EventArgs.Empty);
            component.UpdateOrderChanged -= Component_UpdateOrderChanged;
            _removeUpdateableJournal.Add(index);

            _isEnabledCacheInvalidated = true;
        }

        private void ProcessRemoveUpdateableJournal()
        {
            _removeUpdateableJournal.Sort();
            for (int i = _removeUpdateableJournal.Count - 1; i >= 0; i--)
                _updateableComponents.RemoveAt(_removeUpdateableJournal[i]);

            _removeUpdateableJournal.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (_removeUpdateableJournal.Count > 0)
                ProcessRemoveUpdateableJournal();
            if (_addUpdateableJournal.Count > 0)
                ProcessAddUpdateableJournal();

            if (_isEnabledCacheInvalidated)
            {
                _enabledComponents.Clear();
                for (int i = 0; i < _updateableComponents.Count; i++)
                    if (_updateableComponents[i].Enabled)
                        _enabledComponents.Add(_updateableComponents[i]);

                _isEnabledCacheInvalidated = false;
            }

            for (int i = 0; i < _enabledComponents.Count; i++)
                _enabledComponents[i].Update(gameTime);

            if (_isEnabledCacheInvalidated)
                _enabledComponents.Clear();
        }

        private void ProcessAddUpdateableJournal()
        {
            _addUpdateableJournal.Sort(UpdateableJournalEntry.CompareAddJournalEntry);
            _addUpdateableJournalCount = 0;

            int iAddJournal = 0;
            int iItems = 0;

            while (iItems < _updateableComponents.Count && iAddJournal < _addUpdateableJournal.Count)
            {
                var addJournalItem = _addUpdateableJournal[iAddJournal].Component;

                if (Comparer<int>.Default.Compare(addJournalItem.UpdateOrder, _updateableComponents[iItems].UpdateOrder) < 0)
                {
                    addJournalItem.EnabledChanged += (sender, value) => Component_EnabledChanged(sender, EventArgs.Empty);
                    addJournalItem.UpdateOrderChanged += Component_UpdateOrderChanged;
                    _updateableComponents.Insert(iItems, addJournalItem);
                    iAddJournal++;
                }

                iItems++;
            }


            for (; iAddJournal < _addUpdateableJournal.Count; iAddJournal++)
            {
                var addJournalItem = _addUpdateableJournal[iAddJournal].Component;
                addJournalItem.EnabledChanged += (sender, value) => Component_EnabledChanged(sender, EventArgs.Empty);
                addJournalItem.UpdateOrderChanged += Component_UpdateOrderChanged;

                _updateableComponents.Add(addJournalItem);
            }

            _addUpdateableJournal.Clear();
        }

        private struct UpdateableJournalEntry
        {
            private readonly int AddOrder;
            public readonly IUpdateComponent Component;

            public UpdateableJournalEntry(IUpdateComponent component, int addOrder)
            {
                Component = component;
                AddOrder = addOrder;
            }

            public override int GetHashCode()
            {
                return Component.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (!(obj is UpdateableJournalEntry))
                    return false;

                return Equals(Component, ((UpdateableJournalEntry)obj).Component);
            }

            internal static int CompareAddJournalEntry(UpdateableJournalEntry x, UpdateableJournalEntry y)
            {
                int result = Comparer<int>.Default.Compare(x.Component.UpdateOrder, y.Component.UpdateOrder);
                if (result == 0)
                    result = x.AddOrder - y.AddOrder;
                return result;
            }
        }
    }
}
