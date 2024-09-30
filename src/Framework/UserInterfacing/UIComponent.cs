using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.UserInterfacing
{
    [RequireComponent(typeof(UITransformer))]
    public abstract class UIComponent : Component, IRender
    {
        public int Order { get; set; }
        public SortLayer Layer
        {
            get => layer;
            set => layer = value;
        }

        private SortLayer layer = SortLayer.Layer1;

        private UITransformer transformer;

        protected override void Awake()
        {
            transformer = GetComponent<UITransformer>();

            base.Awake();
        }

        protected abstract void Render(SpriteBatch spriteBatch, UITransformer transformer);

        void IRender.Render(SpriteBatch spriteBatch)
        {
            Render(spriteBatch, transformer);
        }
    }
}
