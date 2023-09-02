using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering
{
    public class RenderLayerEventArgs : EventArgs
    {
        public RenderLayer Value { get; }
        public RenderLayer OldValue { get; }
        public IRenderComponent Component { get; }

        public RenderLayerEventArgs(RenderLayer value, RenderLayer oldValue, IRenderComponent component)
        {
            Value = value;
            OldValue = oldValue;
            Component = component;
        }
    }
}
