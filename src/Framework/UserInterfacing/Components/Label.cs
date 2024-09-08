using System.Reflection;
using ZZZ.Framework.Assets.Rendering;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;

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

        protected override void Render(SpriteBatch spriteBatch, UITransformer transformer)
        {
            //renderManager.FillRectangle(transformer.Bounds, BackgroundColor, transformer.World.Rotation);

            if (Font != null)
                spriteBatch.DrawText((SpriteFont)Font, stringBuilder.ToString(), transformer.Bounds, Color, transformer.World.Rotation);
        }
    }
    public class BindableLabel : UIComponent
    {
        public string Text
        {
            get
            {
                return getValue.Invoke(source).ToString();
            }
            set
            {
                if (Text == value)
                    return;

                setValue?.Invoke(source, value);
            }
        }
        public Font Font { get; set; }
        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        private object source;
        private Action<object, object> setValue;
        private Func<object, object> getValue;
        protected readonly BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public BindableLabel(object dataSource, string propertySource)
        {
            ArgumentNullException.ThrowIfNull(nameof(dataSource));
            ArgumentNullException.ThrowIfNull(nameof(propertySource));

            source = dataSource;

            var property = dataSource.GetType().GetProperty(propertySource, flags);

            if(property == null)
            {
                var field = dataSource.GetType().GetField(propertySource, flags);

                if(field == null) 
                    throw new InvalidOperationException($"Property {propertySource} not found!");

                setValue = field.SetValue;
                getValue = field.GetValue;
            }
            else
            {
                setValue = property.SetValue;
                getValue = property.GetValue;
            }

            BackgroundColor = Color.Transparent;
            Color = Color.White;
            SpriteEffect = SpriteEffects.None;
        }

        protected override void Render(SpriteBatch spriteBatch, UITransformer transformer)
        {
            //renderManager.FillRectangle(transformer.Bounds, BackgroundColor, transformer.World.Rotation);

            if (Font != null)
                spriteBatch.DrawText((SpriteFont)Font, Text, transformer.Bounds, Color, transformer.World.Rotation);
        }
    }
}
