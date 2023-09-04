using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ZZZ.Framework.Monogame.Rendering
{
    public class RenderBatch : IDisposable
    {
        private Texture2D pixel;
        private SpriteBatch spriteBatch;
        private bool disposedValue;

        public RenderBatch(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public virtual void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            spriteBatch?.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        public virtual void End()
        {
            spriteBatch?.End();
        }

        public virtual void DrawString(SpriteFont font, string text,Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void DrawString(SpriteFont font, StringBuilder stringBuilder, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl)
        {
            spriteBatch.DrawString(font, stringBuilder,position, color, rotation, origin, scale, effects, layerDepth, rtl);
        }

        public virtual void Draw(Texture2D texture)
        {
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void FillRectangle(Rectangle rect, Color color)
        {
            spriteBatch.Draw(pixel, rect, color);
        }

        public void FillRectangle(Rectangle rect, Color color, float angle)
        {
            spriteBatch.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void FillRectangle(Vector2 location, Vector2 size, Color color)
        {
            FillRectangle(location, size, color, 0.0f);
        }

        public void FillRectangle(Vector2 location, Vector2 size, Color color, float angle)
        {
            spriteBatch.Draw(pixel, location, null, color, angle, Vector2.Zero, size, SpriteEffects.None, 0);
        }

        public void FillRectangle(float x, float y, float w, float h, Color color)
        {
            FillRectangle(new Vector2(x, y), new Vector2(w, h), color, 0.0f);
        }

        public void FillRectangle(float x, float y, float w, float h, Color color, float angle)
        {
            FillRectangle(new Vector2(x, y), new Vector2(w, h), color, angle);
        }

        public void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            DrawLine(point1, distance, angle, color, thickness);
        }

        public void DrawLine(Vector2 point, float length, float angle, Color color, float thickness)
        {
            spriteBatch.Draw(pixel, point, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        public void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, 1.0f);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                pixel.Dispose();
                spriteBatch.Dispose();
            }

            pixel = null;
            spriteBatch = null;

            disposedValue = true;
        }

        ~RenderBatch()
        {
            if (disposedValue)
                return;

            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            if (disposedValue)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
