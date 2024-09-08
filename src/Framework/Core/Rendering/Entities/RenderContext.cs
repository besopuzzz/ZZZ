using Microsoft.Xna.Platform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public class RenderContext : Disposable, IRenderContext
    {
        /// <summary>
        /// Отображаемый прямоугольник экрана.
        /// </summary>
        public Rectangle Viewport => device.Viewport.Bounds;

        public Sprite Pixel => pixel;

        private GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private static Sprite pixel;

        /// <summary>
        /// Инициализирует экземпляр контекста.
        /// </summary>
        /// <param name="graphicsDevice">Графическое устройство.</param>
        public RenderContext(GraphicsDevice graphicsDevice)
        {
            ArgumentNullException.ThrowIfNull(graphicsDevice);

            device = graphicsDevice;

            if(pixel == null)
            {
                var texture = new Texture2D(graphicsDevice, 1, 1);
                texture.SetData<Color>(new Color[] { Color.White });

                pixel = new Sprite(texture, null, Vector2.One / 2f);
            }

            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((IDisposable)pixel).Dispose();
                spriteBatch.Dispose();
                device.Dispose();
            }

            pixel = null;
            spriteBatch = null;
            device = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="rectangle"></param>
        public void DrawSprite(Sprite sprite, Rectangle rectangle)
        {
            DrawSprite(sprite, rectangle, Color.White);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawSprite(Sprite sprite, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(sprite.Texture, rectangle, sprite.Source, color);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawSprite(Sprite sprite, Transform2D world)
        {
            DrawSprite(sprite, world, Color.White);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawSprite(Sprite sprite, Transform2D world, Color color)
        {
            DrawSprite(sprite, world, color, SpriteEffects.None);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawSprite(Sprite sprite, Transform2D world, Color color, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, color, world.Rotation, sprite.Origin, world.Scale, spriteEffects, 0f);
        }

        /// <summary>
        /// Выполняет рисование текста.
        /// </summary>
        public void DrawText(SpriteFont font, StringBuilder stringBuilder, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, float depth, bool rtl)
        {
            spriteBatch.DrawString(font, stringBuilder, world.Position, color, world.Rotation, origin, world.Scale, effects, depth, rtl);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawText(SpriteFont font, string text, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, bool rtl)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            spriteBatch.DrawString(font, text, world.Position, color, world.Rotation, origin, world.Scale, effects, 0f, rtl);
        }


        /// <summary>
        /// Выполняет рисование текста.
        /// </summary>
        public void DrawText(SpriteFont font, StringBuilder text, Rectangle destination, Color color)
        {
            DrawText(font, text.ToString(), destination, color);
        }

        public void DrawText(SpriteFont font, string text, Rectangle destination, Color color)
        {
            DrawText(font, text, destination, color, 0f);
        }
        /// <summary>
        /// Выполняет рисование текста.
        /// </summary>
        public void DrawText(SpriteFont font, string text, Rectangle destination, Color color, float rotation)
        {
            Point zero = Point.Zero;
            bool flag = true;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                switch (c)
                {
                    case '\n':
                        zero.X = 0;
                        zero.Y += font.LineSpacing;
                        flag = true;
                        continue;
                    case '\r':
                        continue;
                    case '\b':
                        continue;
                }

                if (text.Length != i + 1)
                    if (text[i + 1] == '\b')
                        continue;


                SpriteFont.Glyph ptr;

                if (font.Glyphs.ContainsKey(c))
                    ptr = font.Glyphs[c];
                else ptr = font.Glyphs[font.DefaultCharacter.Value];

                if (flag)
                {
                    zero.X = Math.Max((int)ptr.LeftSideBearing, 0);
                    flag = false;
                }
                else
                {
                    zero.X += (int)font.Spacing + (int)ptr.LeftSideBearing;
                }

                Rectangle destination2 = new Rectangle(zero, ptr.BoundsInTexture.Size);
                destination2.X += ptr.Cropping.X;
                destination2.Y += ptr.Cropping.Y;
                destination2.Location += destination.Location;

                destination2 = Rectangle.Intersect(destination2, destination);


                float sin = (float)Math.Sin(rotation);
                float cos = (float)Math.Cos(rotation);

                var scaled = destination.Location.ToVector2() - destination2.Location.ToVector2();

                Vector2 rotated = new Vector2(cos * scaled.X - sin * scaled.Y, sin * scaled.X + cos * scaled.Y);

                destination2.Location -= rotated.ToPoint() - scaled.ToPoint();

                Rectangle source = ptr.BoundsInTexture;

                source.Size = new Point(Math.Min(source.Width, destination2.Width), Math.Min(source.Height, destination2.Height));

                if (destination2.IsEmpty)
                    continue;

                spriteBatch.Draw(font.Texture, destination2, source, color, rotation, Vector2.Zero, SpriteEffects.None, 0f);

                zero.X += (int)ptr.Width + (int)ptr.RightSideBearing;
            }
        }



        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void FillRectangle(Rectangle rect, Color color)
        {
            DrawSprite(pixel, rect, color);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            DrawLine(point1, point2, 0f, color, thickness);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawLine(Vector2 point1, Vector2 point2, float rotation, Color color, float thickness = 1f)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            DrawLine(point1, distance, angle + rotation, color, thickness);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawLine(Vector2 point, float length, float angle, Color color, float thickness)
        {
            spriteBatch.Draw(pixel.Texture, point, null, color, angle, new Vector2(0, 0.5f), new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, 1.0f);
        }

        public IRenderContext Copy(IRenderContext context)
        {
            return new RenderContext(device);
        }
    }
}
