using System.Reflection;
using ZZZ.Framework.Assets.Rendering;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.UserInterfacing.Components
{
    public class Label : UIComponent
    {
        public string Text
        {
            get
            {
                return stringBuilder.ToString();
            }
            set
            {
                stringBuilder.Clear();
                stringBuilder.Append(value);
            }
        }
        public Font Font { get; set; }
        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        private StringBuilder stringBuilder;

        public Label()
        {
            stringBuilder = new StringBuilder();
            BackgroundColor = Color.Transparent;
            Color = Color.White;
            SpriteEffect = SpriteEffects.None;
        }

        protected override void Render(IRenderProvider renderProvider, UITransformer transformer)
        {

        }
    }
}
