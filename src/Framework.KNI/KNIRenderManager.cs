using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.KNI
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
