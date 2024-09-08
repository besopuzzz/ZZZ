namespace ZZZ.Framework.Assets.Rendering
{
    public class Font : Asset
    {
        public Vector2 Origin { get; set; }
        internal SpriteFont SpriteFont => spriteFont;

        private SpriteFont spriteFont;

        internal Font()
        {

        }

        public Font(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public static explicit operator SpriteFont(Font font)
        {
            return font.spriteFont;
        }
    }
}
