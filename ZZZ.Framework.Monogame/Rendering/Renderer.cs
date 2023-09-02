using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Transforming;

namespace ZZZ.Framework.Monogame.Rendering
{
    public sealed class Renderer : IDisposable
    {
        public static GameTime GameTime => currentGameTime;

        private static GraphicsDevice device = null!;
        private static Texture2D pixel = null!;
        private static SpriteBatch spriteBatch = null!;
        private static GameTime currentGameTime = null!;
        private bool disposedValue = false;

        internal Renderer(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        internal void SetGameTime(GameTime gameTime)
        {
            if(currentGameTime == null)
                currentGameTime = gameTime;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                pixel.Dispose();
                spriteBatch.Dispose();
                device.Dispose();
            }

            pixel = null;
            spriteBatch = null;
            device = null;

            disposedValue = true;
        }
        ~Renderer()
        {
            if (disposedValue)
                return;

            Dispose(disposing: false);
        }
        public void Dispose()
        {
            if (disposedValue)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public static Sprite CreateSprite(Point size, Color color)
        {
            Texture2D texture = new Texture2D(device, size.X, size.Y);
            Color[] colors = new Color[size.X * size.Y];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }

            texture.SetData<Color>(colors);

            return new Sprite(texture, null, Vector2.Zero);
        }

        public static void DrawSprite(Sprite sprite, Rectangle rectangle)
        {
            spriteBatch.Draw(sprite.Texture, rectangle, sprite.Source, Color.White);
        }
        public static void DrawSprite(Sprite sprite, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(sprite.Texture, rectangle, sprite.Source, color);
        }
        public static void DrawSprite(Sprite sprite, Rectangle rectangle, Color color, float depth)
        {
            spriteBatch.Draw(sprite.Texture, rectangle, sprite.Source, color, 0f,Vector2.Zero, SpriteEffects.None, depth);
        }
        public static void DrawSprite(Sprite sprite, Transform2D world)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, Color.White, world.Rotation, sprite.Origin, world.Scale, SpriteEffects.None, 0f);
        }
        public static void DrawSprite(Sprite sprite, Transform2D world, Color color)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, color, world.Rotation, sprite.Origin, world.Scale, SpriteEffects.None, 0f);
        }
        public static void DrawSprite(Sprite sprite, Transform2D world, Color color, float depth)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, color, world.Rotation, sprite.Origin, world.Scale, SpriteEffects.None, depth);
        }
        public static void DrawSprite(Sprite sprite, Transform2D world, Vector2 scale)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, Color.White, world.Rotation, sprite.Origin, world.Scale * scale, SpriteEffects.None, 0f);
        }
        public static void DrawSprite(Sprite sprite, Transform2D world, Color color, SpriteEffects effects, float depth)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, color, world.Rotation, sprite.Origin, world.Scale, effects, depth);
        }

        public static void DrawText(Font font, StringBuilder stringBuilder, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, float depth, bool rtl)
        {
            spriteBatch.DrawString(font.SpriteFont, stringBuilder, world.Position, color, world.Rotation, origin, world.Scale, effects, depth, rtl);
        }
        public static void DrawText(Font font, string text, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, float depth, bool rtl)
        {
            spriteBatch.DrawString(font.SpriteFont, text, world.Position, color, world.Rotation, origin, world.Scale, effects, depth, rtl);
        }

        public static void DrawText(Font font, string text, Transform2D world, Rectangle region, Color color, Vector2 origin, float depth)
        {
            Transform2D offset = new Transform2D(); // Indent from each character

            for (int i = 0; i < text.Length; i++) // Iterate over each character
            {
                char character = text[i];

                switch (character)
                {
                    case '\n':
                        // New line indent
                        offset = Transform2D.CreateTranslation(0f, font.SpriteFont.LineSpacing);
                        continue;
                    case '\r':
                        continue;
                }

                var glyf = font.SpriteFont.Glyphs[character];
                var glyfSource = glyf.BoundsInTexture; // Rectangle source of character 

                Rectangle glyfRect = new Rectangle(region.Location + offset.Position.ToPoint(), glyfSource.Size); // Destination rectangle for character

                var overlapRect = Rectangle.Intersect(glyfRect, region); // Create overlapp

                if (overlapRect.Width == 0 && overlapRect.Height == 0) // Exit if current character rectangle not included in destination rectangle
                    continue;

                // Set new size destination and source for current character 
                glyfSource.Size  = overlapRect.Size;
                glyfRect.Size = overlapRect.Size;

                var newWorld = offset * world;

                // Draw
                spriteBatch.Draw(font.SpriteFont.Texture, newWorld.Position, glyfSource, color, newWorld.Rotation, origin, newWorld.Scale, SpriteEffects.None, depth);

                offset *= Transform2D.CreateTranslation(glyf.WidthIncludingBearings, 0f) ;
            }
        }

        public static void DrawText(Font font, string text, Rectangle destination, Color color, Vector2 origin,float depth)
        {
            Point offset = new Point();

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];

                switch (character)
                {
                    case '\n':
                        offset = new Point(0, font.SpriteFont.LineSpacing);
                        continue;
                    case '\r':
                        continue;
                }

                var glyf = font.SpriteFont.Glyphs[character];
                var glyfSource = glyf.BoundsInTexture;

                Rectangle glyfRect = new Rectangle(destination.Location + offset, glyfSource.Size);

                var overlapRect = Rectangle.Intersect(glyfRect, destination);

                if (overlapRect.Width == 0 && overlapRect.Height == 0)
                    continue;

                glyfSource.Size = overlapRect.Size;
                glyfRect.Size = overlapRect.Size;

                spriteBatch.Draw(font.SpriteFont.Texture, glyfRect, glyfSource, color,0f, origin, SpriteEffects.None, depth);

                offset += new Point((int)glyf.WidthIncludingBearings, 0);
            }
        }

        public static void FillRectangle(Rectangle rect, Color color)
        {
            spriteBatch.Draw(pixel, rect, color);
        }
        public static void FillRectangle(Rectangle rect, Color color, float angle)
        {
            spriteBatch.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            DrawLine(point1, distance, angle, color, thickness);
        }
        public static void DrawLine(Vector2 point, float length, float angle, Color color, float thickness)
        {
            spriteBatch.Draw(pixel, point, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }
        public static void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, 1.0f);
        }

        public static void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }
        public static void End()
        {
            spriteBatch.End();
        }

    }
}
