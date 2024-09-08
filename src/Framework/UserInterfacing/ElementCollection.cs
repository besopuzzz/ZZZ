//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ZZZ.Framework.Core.Rendering;
//using ZZZ.Framework.UserInterfacing.Components;

//namespace ZZZ.Framework.UserInterfacing
//{
//    public class ElementCollection
//    {
//        public Canvas Canvas
//        {
//            get => canvas;
//            set
//            {
//                if (canvas == value)
//                    return;

//                canvas = value;
//            }
//        }

//        public IReadOnlyCollection<UIComponent> Elements => elements;

//        public IReadOnlyCollection<ElementCollection> Childs => childs;

//        private Canvas canvas;
//        private readonly List<UIComponent> elements = new List<UIComponent>();
//        private readonly List<ElementCollection> childs = new List<ElementCollection>();

//        public ElementCollection()
//        {

//        }



//        public void Render(SpriteBatch spriteBatch)
//        {
//            if(Canvas?.RenderTarget != null)
//            {
//                spriteBatch.GraphicsDevice.SetRenderTarget(Canvas.RenderTarget);

//                spriteBatch.Begin();
//            }

//            foreach (var item in elements)
//            {
//                item.Render(spriteBatch);
//            }

//            foreach (var item in childs)
//            {
//                item.Render(spriteBatch);
//            }

//            if (Canvas?.RenderTarget != null)
//            {
//                spriteBatch.End();
//                spriteBatch.GraphicsDevice.SetRenderTarget(null);
//            }
//        }
//    }
//}
