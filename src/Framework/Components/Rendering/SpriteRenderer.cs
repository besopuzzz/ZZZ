using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Rendering
{
    [RequiredComponent<Transformer>]
    public class SpriteRenderer : Component, IRenderer
    {
        public Sprite Sprite { get; set; }

        public Color Color { get; set; } = Color.White;

        public SpriteEffects SpriteEffect { get; set; }

        public int Order { get; set; }

        public SortLayer Layer { get; set; }

        private Transformer transformer;

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRenderer.Render(IRenderProvider provider)
        {
            Transform2D transform = transformer.World;

            provider.RenderSprite(transform, Sprite, Color, SpriteEffect);
        }
    }
}
