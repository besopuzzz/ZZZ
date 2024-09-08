using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering;

namespace ZZZ.Framework.UserInterfacing.Components
{
    public class Canvas : Component, IRender
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

        void IRender.Render(SpriteBatch  spriteBatch)
        {

        }
    }
}
