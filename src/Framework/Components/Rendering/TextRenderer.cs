using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Rendering
{
    [RequireComponent(typeof(Transformer))]
    public class TextRenderer : Component, IRender
    {
        [ContentSerializerIgnore]
        public SpriteFont Font { get; set; }
        public string Text { get; set; } = " ";
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public int Order { get; set; }
        public SortLayer Layer
        {
            get => layer;
            set => layer = value;
        }

        private SortLayer layer = SortLayer.Layer1;

        private Transformer transformer;

        public TextRenderer()
        {

        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRender.Render(SpriteBatch spriteBatch)
        {
            Transform2D transform = transformer.World;

            spriteBatch.DrawText(Font, Text, transform, Color, Origin, SpriteEffect, false);
        }
    }
}
