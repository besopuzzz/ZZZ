using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Platform.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Rendering.Assets;
using static System.Net.Mime.MediaTypeNames;

namespace ZZZ.Framework.Core.Rendering
{
    /// <summary>
    /// Представляет вспомогательный менеджер с информацией о времени, слоев и прямоугольник отображаемого экрана.
    /// </summary>
    public class RenderManager : Disposable
    {
        /// <summary>
        /// Игровое время.
        /// </summary>
        public GameTime GameTime => currentGameTime;

        /// <summary>
        /// Отображаемый прямоугольник экрана.
        /// </summary>
        public Rectangle ViewBounds => device.Viewport.Bounds;

        public Sprite Pixel { get; private set; }

        public static RenderManager Instance { get; private set; }

        private GraphicsDevice device;
        private Texture2D pixel;
        private SpriteBatch spriteBatch;
        private GameTime currentGameTime;

        /// <summary>
        /// Инициализирует экземпляр менеджера.
        /// </summary>
        /// <param name="graphicsDevice">Графическое устройство.</param>
        public RenderManager(GraphicsDevice graphicsDevice)
        {
            if (Instance == null)
                Instance = this;
            else return;

            device = graphicsDevice;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            Pixel = new Sprite(pixel, null, Vector2.One / 2);

            spriteBatch = new SpriteBatch(graphicsDevice);
            currentGameTime = new GameTime();
        }

        internal void SetGameTime(GameTime gameTime)
        {
                currentGameTime = gameTime;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
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
        public void DrawText(SpriteFont font, StringBuilder stringBuilder, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, float depth,bool rtl)
        {
            spriteBatch.DrawString(font, stringBuilder, world.Position, color, world.Rotation, origin, world.Scale, effects, depth, rtl);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawText( SpriteFont font, string text, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, bool rtl)
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

                Rectangle destination2 = new Rectangle(zero,ptr.BoundsInTexture.Size);
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
            spriteBatch.Draw(pixel, rect, color);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void FillRectangle(Rectangle rect, Color color, Vector2 origin)
        {
            spriteBatch.Draw(pixel, rect, null, color, 0f, origin, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void FillRectangle(Rectangle rect, Color color, float angle)
        {
            spriteBatch.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
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
            spriteBatch.Draw(pixel, point, null, color, angle, new Vector2(0, 0.5f), new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, 1.0f);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null,
            SamplerState samplerState = null, DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }
    }
}
