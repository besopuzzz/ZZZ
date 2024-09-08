using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Core.Rendering
{
    public static class Extentions
    {

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="rectangle"></param>
        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Rectangle rectangle)
        {
            DrawSprite(spriteBatch, sprite, rectangle, Color.White);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(sprite.Texture, rectangle, sprite.Source, color);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Transform2D world)
        {
            DrawSprite(spriteBatch, sprite, world, Color.White);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Transform2D world, Color color)
        {
            DrawSprite(spriteBatch, sprite, world, color, SpriteEffects.None);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Transform2D world, Color color, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(sprite.Texture, world.Position, sprite.Source, color, world.Rotation, sprite.Origin, world.Scale, spriteEffects, 0f);
        }

        /// <summary>
        /// Выполняет рисование спрайта.
        /// </summary>
        public static void DrawText(this SpriteBatch spriteBatch, SpriteFont font, string text, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, bool rtl)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            spriteBatch.DrawString(font, text, world.Position, color, world.Rotation, origin, world.Scale, effects, 0f, rtl);
        }


        /// <summary>
        /// Выполняет рисование текста.
        /// </summary>
        public static void DrawText(this SpriteBatch spriteBatch, SpriteFont font, StringBuilder text, Rectangle destination, Color color)
        {
            DrawText(spriteBatch, font, text.ToString(), destination, color);
        }

        public static void DrawText(this SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle destination, Color color)
        {
            DrawText(spriteBatch, font, text, destination, color, 0f);
        }
        /// <summary>
        /// Выполняет рисование текста.
        /// </summary>
        public static void DrawText(this SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle destination, Color color, float rotation)
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


    }
}
