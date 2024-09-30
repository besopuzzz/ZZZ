using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Rendering
{
    [RequireComponent(typeof(Transformer))]
    public class SpriteRenderer : Component, IRender
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

        private SortLayer layer = SortLayer.Layer1;
        public Transformer transformer;

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRender.Render(SpriteBatch spriteBatch)
        {
            Transform2D transform = transformer.World;

            spriteBatch.DrawSprite(Sprite, transform, Color, SpriteEffect);
        }
    }
}
