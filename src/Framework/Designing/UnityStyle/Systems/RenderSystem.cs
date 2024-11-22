using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Designing.UnityStyle.Systems
{
    public abstract class RenderSystem : System, IGroupRenderer
    {
        public SortLayer LayerMask => SortLayer.All;

        public RenderContext Context => context;

        public int Order { get; set; }

        public SortLayer Layer { get; set; }

        private RenderContext context;

        protected override void Awake()
        {
            context = Services.Get<IRenderManager>().CreateInstance();

            base.Awake();
        }

        void IRenderer.Render(IRenderProvider provider)
        {
            context.Render(RenderContext.RenderMode.ToOneLayer, Camera.MainCamera);
        }
    
        protected void Render()
        {
            Camera.MainCamera?.UpdateMatrix();

            Context.Render(RenderContext.RenderMode.ToEveryLayer, Camera.MainCamera);
        }

        protected override void Input(IEnumerable<Component> components)
        {
            foreach (var component in components.Where(x => x is IRenderer).Cast<IRenderer>())
            {
                SignalMessenger.SendToParents<IGroupRenderer>(((Component)component).Owner, this, (x, y) =>
                {
                    x.Context.AddToQueue(component);

                    return true;
                });
            }

            base.Input(components);
        }

        protected override void Output(Component component)
        {
            if (component is IRenderer updater)
            {
                SignalMessenger.SendToParents<IGroupRenderer>(((Component)component).Owner, this, (x, y) =>
                {
                    x.Context.RemoveFromQueue(updater);

                    return true;
                });
            }

            base.Output(component);
        }
    }
}
