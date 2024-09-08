using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.UserInterfacing
{
    [RequireComponent(typeof(UITransformer))]
    public abstract class UIComponent : Component, IRender
    {
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

        private UITransformer transformer;
        private SortLayer layer;

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
