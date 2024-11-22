namespace ZZZ.Framework.Rendering
{
    public interface IRenderManager
    {
        bool UseHalfPixelOffset { get; }
        Point ScreenSize { get; }
        RenderContext CreateInstance();
    }
}
