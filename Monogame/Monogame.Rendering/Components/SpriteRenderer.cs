using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Transforming.Components;

namespace ZZZ.Framework.Monogame.Rendering.Components
{
    [RequireComponent(Type = typeof(Transformer))]
    public class SpriteRenderer : RenderComponent
    {
        public Sprite Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        private Transformer transformer;

        public SpriteRenderer()
        {

        }
        public SpriteRenderer(Sprite sprite, Color color, SpriteEffects spriteEffect, float depth)
        {
            Sprite = sprite;
            Color = color;
            SpriteEffect = spriteEffect;
            Depth = depth;
        }

        protected override void Startup()
        {
            transformer = GetComponent<Transformer>();

            base.Startup();
        }

        protected override void Draw()
        {
            Renderer.DrawSprite(Sprite, transformer.World, Color, SpriteEffect, Depth);
        }
    }
}
