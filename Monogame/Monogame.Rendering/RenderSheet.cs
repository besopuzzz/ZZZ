using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering
{
    public sealed class RenderSheet : Disposable
    {
        public RenderLayer Layer { get; }
        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public RasterizerState RasterizerState { get; set; } = null;
        public Effect Effect { get; set; }

        private RenderComponents components = new RenderComponents();
        private RenderTarget2D renderTarget = null!;
        private GraphicsDevice graphicsDevice;

        public RenderSheet(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Reset()
        {
            renderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height);
        }
        public void Add(IRenderComponent component)
        {
            if (components.Contains(component))
                throw new ArgumentException("The component already exist!");

            components.Add(component);

        }
        public void Remove(IRenderComponent component)
        {
            if (!components.Contains(component))
                throw new ArgumentException("The component not exist!");

            components.Remove(component);
        }

        public void DrawToSheet(RenderBatch spriteBatch, Matrix matrix)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);

            components.Begin(SpriteSortMode, BlendState, SamplerState, null, RasterizerState, Effect, matrix);
            components.Draw();
            components.End();

            graphicsDevice.SetRenderTarget(null);
        }
        public void DrawSheet(RenderBatch renderBatch)
        {
            renderBatch.Draw(renderTarget);
        }
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                components.Dispose();
                renderTarget.Dispose();
                graphicsDevice.Dispose();
            }

            components = null;
            renderTarget = null;
            graphicsDevice = null;
            BlendState = null;
            SamplerState = null;
            RasterizerState = null;
        }
    }
}
