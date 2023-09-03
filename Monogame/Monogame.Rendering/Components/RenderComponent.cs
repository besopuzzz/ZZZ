namespace ZZZ.Framework.Monogame.Rendering.Components
{
    public abstract class RenderComponent : Component, IRenderComponent
    {
        public RenderLayer Layer
        {
            get => layer;
            set
            {
                if (layer == value) return;

                RenderLayerEventArgs args = new RenderLayerEventArgs(value, layer, this);

                layer = value;
                LayerChanged?.Invoke(this, args);
            }
        }
        public event EventHandler<RenderLayerEventArgs> LayerChanged;
        public float Depth { get; set; }

        private RenderLayer layer = RenderLayer.First;

        protected override void RegistrationComponents()
        {
            RegistrationComponent<IRenderComponent>(this);

            base.RegistrationComponents();
        }
        protected override void UnregistrationComponents()
        {
            UnregistrationComponent<IRenderComponent>(this);

            base.UnregistrationComponents();
        }
        protected virtual void Draw()
        {

        }
        void IRenderComponent.Draw()
        {
            Draw();
        }
    }
}
