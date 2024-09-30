using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.UserInterfacing.Components
{
    public class Canvas : Component, IRender
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

        void IRender.Render(SpriteBatch  spriteBatch)
        {

        }
    }
}
