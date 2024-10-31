using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Tiling
{
    [RequiredComponent(typeof(Transformer))]
    internal sealed class TileRenderer : Component, IRender
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

        void IRender.Render(SpriteBatch spriteBatch)
        {
            Transform2D transform = Transform * transformer.World;

            spriteBatch.DrawSprite(Sprite, transform, Color, SpriteEffect);
        }
    }
}
