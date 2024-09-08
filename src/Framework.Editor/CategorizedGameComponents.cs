using Microsoft.Xna.Framework;
using MonoGame.Forms.NET;
using System.Collections;

namespace ZZZ.Framework.Editor
{
    internal sealed class CategorizedGameComponents : IEnumerable<IGameComponent>
    {
        private GameComponentCollection components;

        private readonly Action<IUpdateable, GameTime> _UpdateAction =
            (updateable, gameTime) =>
            {
                updateable.Update(gameTime);
            };

        private readonly Action<IDrawable, GameTime> _DrawAction =
            (drawable, gameTime) =>
            {
                drawable.Draw(gameTime);
            };

        private SortingFilteringCollection<IDrawable> _Drawables =
            new SortingFilteringCollection<IDrawable>(
                d => d.Visible,
                (d, handler) => d.VisibleChanged += handler,
                (d, handler) => d.VisibleChanged -= handler,
                (d1, d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
                (d, handler) => d.DrawOrderChanged += handler,
                (d, handler) => d.DrawOrderChanged -= handler);

        private SortingFilteringCollection<IUpdateable> _Updateables =
            new SortingFilteringCollection<IUpdateable>(
                u => u.Enabled,
                (u, handler) => u.EnabledChanged += handler,
                (u, handler) => u.EnabledChanged -= handler,
                (u1, u2) => Comparer<int>.Default.Compare(u1.UpdateOrder, u2.UpdateOrder),
                (u, handler) => u.UpdateOrderChanged += handler,
                (u, handler) => u.UpdateOrderChanged -= handler);

        public CategorizedGameComponents(GameComponentCollection components)
        {
            this.components = components;
        }

        internal void Initialize()
        {
            foreach (var item in components)
            {
                CategorizeComponent(item);
                item.Initialize();
            }

            components.ComponentAdded += Components_ComponentAdded;
            components.ComponentRemoved += Components_ComponentRemoved;
        }

        private void Components_ComponentAdded(object? sender, GameComponentCollectionEventArgs e)
        {
            e.GameComponent.Initialize();
            CategorizeComponent(e.GameComponent);
        }

        private void Components_ComponentRemoved(object? sender, GameComponentCollectionEventArgs e)
        {
            DecategorizeComponent(e.GameComponent);
        }

        private void CategorizeComponents()
        {
            DecategorizeComponents();
            for (int i = 0; i < components.Count; ++i)
                CategorizeComponent(components[i]);
        }

        private void DecategorizeComponents()
        {
            _Updateables.Clear();
            _Drawables.Clear();
        }

        private void CategorizeComponent(IGameComponent component)
        {
            if (component is IUpdateable)
                _Updateables.Add((IUpdateable)component);
            if (component is IDrawable)
                _Drawables.Add((IDrawable)component);
        }

        private void DecategorizeComponent(IGameComponent component)
        {
            if (component is IUpdateable)
                _Updateables.Remove((IUpdateable)component);
            if (component is IDrawable)
                _Drawables.Remove((IDrawable)component);
        }

        internal void UpdateComponents(GameTime gameTime)
        {
            _Updateables.ForEachFilteredItem(_UpdateAction, gameTime);
        }

        internal void DrawComponents(GameTime gametime)
        {
            _Drawables.ForEachFilteredItem(_DrawAction, gametime);
        }

        public IEnumerator<IGameComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return components.GetEnumerator();
        }
    }
}
