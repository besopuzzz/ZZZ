using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using ZZZ.Framework.Monogame.Content;

namespace ZZZ.Framework.Monogame.Rendering.Content
{
    public sealed class Font : Asset
    {
        internal SpriteFont SpriteFont => spriteFont;
        public float Spacing
        {
            get => spriteFont.Spacing;
            set => spriteFont.Spacing = value;
        }
        public int LineSpacing
        {
            get => spriteFont.LineSpacing;
            set=> spriteFont.LineSpacing = value;
        }
        public char? DefaultCharacter
        {
            get => spriteFont.DefaultCharacter;
            set=> spriteFont.DefaultCharacter = value;
        }

        private SpriteFont spriteFont;

        internal Font()
        {

        }
        public Font(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                spriteFont.Texture.Dispose();
            }

            spriteFont = null;
        }

        public Vector2 MeasureString(string text)
        {
            return spriteFont.MeasureString(text);
        }
        public Vector2 MeasureString(StringBuilder stringBuilder)
        {
            return spriteFont.MeasureString(stringBuilder);
        }
    }
}
