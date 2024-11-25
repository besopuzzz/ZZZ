using ZZZ.Framework.Components;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Rendering.Components;

namespace ZZZ.Framework.Tiling.Components
{
    [RequiredComponent<Transformer>]
    internal sealed class TileRenderer : Component, IRenderer
    {
        public Sprite Sprite { get; set; }

        public Color Color { get; set; } = Color.White;

        public SpriteEffects SpriteEffect { get; set; }

        public int Order { get; set; }
        public SortLayer Layer
        {
            get => layer;
            set => layer = value;
        }
        public Transform2D Transform { get; set; }

        private SortLayer layer = SortLayer.Layer1;
        public Transformer transformer;

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRenderer.Render(IRenderProvider provider)
        {
            Transform2D transform = Transform * transformer.World;

            provider.RenderSprite(transform, Sprite, Color, SpriteEffect);
        }
    }
}
