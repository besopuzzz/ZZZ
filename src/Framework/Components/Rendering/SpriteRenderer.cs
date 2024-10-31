using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Rendering
{
    [RequiredComponent(typeof(Transformer))]
    public class SpriteRenderer : RenderComponent
    {
        public Sprite Sprite { get; set; }

        public Color Color { get; set; } = Color.White;

        public SpriteEffects SpriteEffect { get; set; }

        private Transformer transformer;

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        protected override void Render(RenderContext renderContext)
        {
            Transform2D transform = transformer.World;

            renderContext.RenderSprite(transform, Sprite, Color, SpriteEffect);
        }
    }
}
