using ZZZ.Framework.Rendering.Components;

namespace ZZZ.Framework.Rendering
{
    public interface IRenderer
    {
        bool Enabled { get; }
        int Order { get; }
        SortLayer Layer { get; }
        void Render(IRenderProvider provider);
    }

    public interface IGroupRenderer : IRenderer
    {
        RenderContext Context { get; }
    }
}
