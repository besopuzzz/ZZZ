namespace ZZZ.Framework.Core.Rendering.Components
{
    public interface IRender : IGraphics
    {
        int Order { get; }
        event EventHandler<SortLayerArgs> LayerChanged;
        void Render(RenderManager renderManager);
    }
}
