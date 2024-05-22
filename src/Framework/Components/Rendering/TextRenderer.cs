using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Components.Rendering
{
    [RequireComponent(typeof(Transformer))]
    public class TextRenderer : Component, IRender
    {
        public SpriteFont Font { get; set; }
        public StringBuilder Text { get; set; } = new StringBuilder();
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public int Order { get; set; }
        public SortLayer Layer
        {
            get => layer;
            set
            {
                if (layer == value)
                    return;

                SortLayer oldValue = value;

                layer = value;

                LayerChanged?.Invoke(this, new SortLayerArgs(oldValue, value));
            }
        }

        public event EventHandler<SortLayerArgs> LayerChanged;

        private SortLayer layer;

        private Transformer transformer;

        public TextRenderer()
        {

        }

        public void SetFromString(string text)
        {
            if(Text == null)
                Text = new StringBuilder();

            Text.Clear();
            Text.Append(text);
        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRender.Render(RenderManager renderManager)
        {
            Transform2D transform = transformer.World;

            renderManager.DrawText(Font, Text, transform, Color, Origin, SpriteEffect, false);
        }
    }
}
