using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering;

namespace ZZZ.Framework.Rendering
{
    public abstract class RenderContext : Disposable
    {
        public enum RenderMode
        {
            ToOneLayer,
            ToEveryLayer
        }

        protected abstract IRenderProvider RenderProvider { get; }

        protected IReadOnlyList<IRenderer> Queue => renderers;

        private readonly Dictionary<SortLayer, List<IRenderer>> entities;
        private readonly List<IRenderer> renderers;
        private readonly Comparer<int> comparer;

        public RenderContext() 
        {
            entities = new Dictionary<SortLayer, List<IRenderer>>();

            foreach (var sortLayer in Enum.GetValues<SortLayer>())
                entities.Add(sortLayer, new List<IRenderer>());

            renderers = new List<IRenderer>();
            comparer = Comparer<int>.Default;
        }

        protected abstract void Begin(Camera camera);

        protected abstract void End();

        protected override void Dispose(bool disposing)
        {
            renderers.Clear();

            foreach (var item in entities)
            {
                item.Value.Clear();
            }

            entities.Clear();

            base.Dispose(disposing);
        }

        public void AddToQueue(IRenderer renderer)
        {
            renderers.Add(renderer);

            if (renderer is not IGroupRenderer group)
                return;

            Component original = renderer as Component;

            foreach (var child in renderers.ToList())
            {
                Component childComponent = child as Component;

                if (childComponent.Owner.IsParent(original.Owner))
                {
                    renderers.Remove(child);

                    group.Context.renderers.Add(child);
                }
            }
        }

        public void RemoveFromQueue(IRenderer renderer)
        {
            renderers.Remove(renderer);

            if (renderer is IGroupRenderer group)
                renderers.AddRange(group.Context.renderers);
        }

        private void SortAndRender(List<IRenderer> entitiesToRender)
        {
            entitiesToRender.Sort((x, y) => comparer.Compare(x.Order, y.Order));

            foreach (var entity in entitiesToRender)
                entity.Render(RenderProvider);

            entitiesToRender.Clear();
        }

        public virtual void Render(RenderMode renderMode, Camera camera)
        {
            if (camera == null || !camera.Enabled)
                return;

            foreach (var component in renderers)
                //if (component.Enabled)
                    entities[component.Layer].Add(component);

            if (renderMode == RenderMode.ToOneLayer)
            {
                Begin(camera);

                foreach (var entity in entities)
                {
                    SortAndRender(entity.Value);
                }

                End();
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (!camera.LayerMask.HasFlag(entity.Key))
                        continue;

                    Begin(camera); 

                    SortAndRender(entity.Value);

                    End();
                }
            }
        }
    }
}
