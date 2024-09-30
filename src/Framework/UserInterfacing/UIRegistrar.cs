//using ZZZ.Framework.Core.Registrars;
//using ZZZ.Framework.Core.Rendering;

//namespace ZZZ.Framework.UserInterfacing
//{
//    public class UIRegistrar : BaseRegistrar<UIComponent>, IOnlyEnabledRegistrar<UIComponent>
//    {
//        private List<UIComponent> components = new List<UIComponent>();

//        void IOnlyEnabledRegistrar<UIComponent>.EnabledReception(UIComponent component)
//        {
//            components.Add(component);
//        }

//        void IOnlyEnabledRegistrar<UIComponent>.EnabledDeparture(UIComponent component)
//        {
//            components.Remove(component);
//        }

//        protected override void Draw(GameTime gameTime)
//        {
//            RenderManager.Instance.Begin(samplerState: SamplerState.PointClamp);

//            foreach (var item in components)
//            {
//                //item.RenderInternal();
//            }

//            RenderManager.Instance.End();

//            base.Draw(gameTime);
//        }
//    }
//}
