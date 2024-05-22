using ZZZ.Framework.Rendering.Assets;

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

        public Matrix CameraProjection { get; internal set; }

        public Sprite Pixel { get; private set; }


        private GraphicsDevice device;
        private Texture2D pixel;
        private SpriteBatch spriteBatch;
        private GameTime currentGameTime;

        /// <summary>
        /// Инициализирует экземпляр менеджера.
        /// </summary>
        /// <param name="graphicsDevice">Графическое устройство.</param>
        public RenderManager()
        {

        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {

            device = graphicsDevice;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            Pixel = new Sprite(pixel, null, Vector2.One / 2);

            spriteBatch = new SpriteBatch(graphicsDevice);

        }

        internal void SetGameTime(GameTime gameTime)
        {
            if (currentGameTime == null)
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
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawText(SpriteFont font, StringBuilder stringBuilder, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, bool rtl)
        {
            spriteBatch.DrawString(font, stringBuilder, world.Position, color, world.Rotation, origin, world.Scale, effects, 0f, rtl);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawText(SpriteFont font, string text, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, bool rtl)
        {
            spriteBatch.DrawString(font, text, world.Position, color, world.Rotation, origin, world.Scale, effects, 0f, rtl);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawText(SpriteFont font, string text, Transform2D world, Rectangle region, Color color, Vector2 origin)
        {
            Transform2D offset = new Transform2D(); // Indent from each character

            for (int i = 0; i < text.Length; i++) // Iterate over each character
            {
                char character = text[i];

                switch (character)
                {
                    case '\n':
                        // New line indent
                        offset = Transform2D.CreateTranslation(0f, font.LineSpacing);
                        continue;
                    case '\r':
                        continue;
                }

                var glyf = font.Glyphs[character];
                var glyfSource = glyf.BoundsInTexture; // Rectangle source of character 

                Rectangle glyfRect = new Rectangle(region.Location + offset.Position.ToPoint(), glyfSource.Size); // Destination rectangle for character

                var overlapRect = Rectangle.Intersect(glyfRect, region); // Create overlapp

                if (overlapRect.Width == 0 && overlapRect.Height == 0) // Exit if current character rectangle not included in destination rectangle
                    continue;

                // Set new size destination and source for current character 
                glyfSource.Size = overlapRect.Size;
                glyfRect.Size = overlapRect.Size;

                var newWorld = offset * world;

                // Draw
                spriteBatch.Draw(font.Texture, newWorld.Position, glyfSource, color, newWorld.Rotation, origin, newWorld.Scale, SpriteEffects.None, 0f);

                offset *= Transform2D.CreateTranslation(glyf.WidthIncludingBearings, 0f);
            }
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public void DrawText(SpriteFont font, string text, Rectangle destination, Color color)
        {
            Point offset = new Point();

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];

                switch (character)
                {
                    case '\n':
                        offset = new Point(0, font.LineSpacing);
                        continue;
                    case '\r':
                        continue;
                }

                var glyf = font.Glyphs[character];
                var glyfSource = glyf.BoundsInTexture;

                Rectangle glyfRect = new Rectangle(destination.Location + offset, glyfSource.Size);

                var overlapRect = Rectangle.Intersect(glyfRect, destination);

                if (overlapRect.Width == 0 && overlapRect.Height == 0)
                    continue;

                glyfSource.Size = overlapRect.Size;
                glyfRect.Size = overlapRect.Size;

                spriteBatch.Draw(font.Texture, glyfRect, glyfSource, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                offset += new Point((int)glyf.WidthIncludingBearings, 0);
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
