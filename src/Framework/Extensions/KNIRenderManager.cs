using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Extensions
{
    internal sealed class KNIRenderManager : IRenderManager
    {
        private GraphicsDevice graphicsDevice;

        public KNIRenderManager(GraphicsDevice graphicsDevice) 
        {
            this.graphicsDevice = graphicsDevice;
        }

        public bool UseHalfPixelOffset => graphicsDevice.UseHalfPixelOffset;

        public Point ScreenSize => graphicsDevice.Viewport.Bounds.Size;

        public RenderContext CreateInstance()
        {
            return new KNIRenderContext(graphicsDevice);
        }
    }
}
