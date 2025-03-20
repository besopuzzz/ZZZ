using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Rendering.Components;

namespace ZZZ.Framework.KNI
{
    internal sealed class KNIRenderContext : RenderContext
    {
        protected override IRenderProvider RenderProvider => renderBatch;

        private RenderBatch renderBatch;
        private BasicEffect basicEffect;

        public KNIRenderContext(GraphicsDevice device)
        {
            renderBatch = new RenderBatch(device);
            basicEffect = new BasicEffect(device);
        }

        protected override void Begin(Camera camera)
        {
            //basicEffect.Projection = camera.Projection;
            //basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Up);
            //basicEffect.World = camera.View;
            //basicEffect.TextureEnabled = true;

            renderBatch.Begin(transformMatrix: camera.View.GetMatrix(), sortMode: SpriteSortMode.Texture, /*effect: basicEffect,*/ samplerState: SamplerState.PointClamp);
        }

        protected override void End()
        {
            renderBatch.End();
        }

        private sealed class RenderBatch : SpriteBatch, IRenderProvider
        {
            public RenderBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
            {
            }

            public void RenderText(SpriteFont font, string text, Transform2D transform, Color color, Vector2 origin, SpriteEffects spriteEffect)
            {
                DrawString(font, text, transform.Position, color, transform.Rotation, origin, transform.Scale, spriteEffect, 0f);
            }

            public void RenderSprite(Transform2D transform, Sprite sprite, Color color, SpriteEffects spriteEffect)
            {
                Draw(sprite.Texture, transform.Position, sprite.Source, color, transform.Rotation, sprite.Origin, transform.Scale, spriteEffect, 0f);
            }
        }
    }
}
