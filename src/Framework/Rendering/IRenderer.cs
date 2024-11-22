using ZZZ.Framework.Core.Rendering;

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
