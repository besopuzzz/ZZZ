using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Components.Rendering
{
    [RequiredComponent<Transformer>]
    public class TextRenderer : Component, IRenderer
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

        void IRenderer.Render(IRenderProvider renderProvider)
        {
            Transform2D transform = transformer.World;

            renderProvider.RenderText(Font, Text, transform, Color, Origin, SpriteEffect);
        }
    }
}
