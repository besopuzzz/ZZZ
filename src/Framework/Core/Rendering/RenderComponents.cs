using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering
{
    internal class GroupComponents
    {

    }

    internal class RenderComponents
    {
        public SortLayer Layer { get; }
        public RenderComponents Next { get; }
        public List<IRender> Components { get; } = new List<IRender>();

        private static RenderComponents root;
        private Comparer<int> comparer = Comparer<int>.Default;


        public RenderComponents()
        {
            root = this;

            Layer = SortLayer.Layer0;

            Next = new RenderComponents((int)SortLayer.Layer1);
        }



        private RenderComponents(int layer)
        {
            Layer = (SortLayer)layer;

            var nextLayer = layer << 1;

            if (nextLayer != (int)SortLayer.Layer31 << 1)
                Next = new RenderComponents(nextLayer);
        }

        public void Add(IRender component)
        {
            if (root == this)
                component.LayerChanged += root.Component_LayerChanged;

            if (component.Layer.HasFlag((SortLayer)Layer))
                Components.Add(component);
            else Next?.Add(component);
        }

        private void Component_LayerChanged(object sender, SortLayerArgs e)
        {
            if (e.OldLayer.HasFlag((SortLayer)Layer))
                Components.Remove((IRender)sender);

            if (e.NewLayer.HasFlag((SortLayer)Layer))
                Components.Add((IRender)sender);

            Next?.Component_LayerChanged(sender, e);
        }

        public void Remove(IRender component)
        {
            if (root == this)
                component.LayerChanged -= root.Component_LayerChanged;

            if (component.Layer.HasFlag((SortLayer)Layer))
                Components.Remove(component);
            else Next?.Remove(component);
        }

        public void Clear()
        {
            if (root == this)
                foreach (var component in Components)
                    component.LayerChanged -= root.Component_LayerChanged;

            Components.Clear();
            Next?.Clear();
        }

        public void Render(ICamera camera, RenderManager renderManager)
        {
            if (camera.Layer.HasFlag((SortLayer)Layer))
            {
                Components.Sort((x, y) => comparer.Compare(x.Order, y.Order));
                camera.Render(renderManager, Components);
            }

            Next?.Render(camera, renderManager);
        }
    }
}
