//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ZZZ.Framework.Core.Rendering;
//using ZZZ.Framework.Core.Rendering.Components;

//namespace ZZZ.Framework.Components.Rendering
//{
//    public class GroupRender : Component, IRender
//    {
//        public int Order { get; set; }

//        public SortLayer Layer
//        {
//            get => layer;
//            set
//            {
//                if (layer == value)
//                    return;

//                SortLayer oldValue = value;

//                layer = value;

//                LayerChanged?.Invoke(this, new SortLayerArgs(oldValue, value));
//            }
//        }

//        public event EventHandler<SortLayerArgs> LayerChanged;

//        private UITransformer transformer;
//        private SortLayer layer;

//        protected override void Awake()
//        {
//            transformer = GetComponent<UITransformer>();

//            base.Awake();
//        }
//    }
//}
