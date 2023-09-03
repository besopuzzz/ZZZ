using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Transforming.Components;

namespace ZZZ.Framework.Monogame.Rendering.Components
{
    [RequireComponent(Type = typeof(Transformer))]
    public class TextRenderer : RenderComponent
    {
        public Font Font { get; set; }
        public StringBuilder Text { get; set; } = new StringBuilder();
        public Color Color { get; set; } = Color.White;
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        [ContentSerializerIgnore]
        public Vector2 Origin { get; set; }

        private Transformer transformer;

        public TextRenderer()
        {
            Layer = RenderLayer.Eighth;
        }
        public TextRenderer(Font font) : this()
        {
            Font = font;
        }

        public TextRenderer(Font font, Color color, SpriteEffects spriteEffect, float depth) : this()
        {
            Font = font;
            Color = color;
            SpriteEffect = spriteEffect;
            Depth = depth;
        }

        public void SetFromString(string text)
        {
            if(Text == null)
                Text = new StringBuilder();

            Text.Clear();
            Text.Append(text);
        }

        protected override void Startup()
        {
            transformer = Owner.GetComponent<Transformer>();

            base.Startup();
        }

        protected override void Draw()
        {
            Renderer.DrawText(Font, Text, transformer.World, Color, Origin, SpriteEffect, Depth, false);
        }
    }
}
