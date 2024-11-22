using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.UserInterfacing.Components
{
    public class Canvas : Component, IRenderer
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

        void IRenderer.Render(IRenderProvider provider)
        {

        }
    }
}
