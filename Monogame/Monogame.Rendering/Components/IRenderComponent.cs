namespace ZZZ.Framework.Monogame.Rendering.Components
{
    public interface IRenderComponent : IComponent
    {
        RenderLayer Layer { get; }
        event EventHandler<RenderLayerEventArgs> LayerChanged;
        void Draw();
    }
}
