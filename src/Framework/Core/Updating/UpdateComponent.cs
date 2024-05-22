using ZZZ.Framework.Core.Updating.Components;

namespace ZZZ.Framework.Core.Updating
{
    internal sealed class UpdateComponent
    {
        public IComponent Component { get; }
        public int UpdateOrder
        {
            get => updateOrderType.Order;
        }
        public bool Enabled
        {
            get
            {
                return Component.Enabled;
            }

            set
            {
                Component.Enabled = value;
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private UpdateOrderType updateOrderType;
        private IUpdateComponent updateComponent;

        public UpdateComponent(UpdateOrderType updateOrderType, IComponent gameComponent)
        {
            Component = gameComponent;
            updateComponent = gameComponent as IUpdateComponent;
            this.updateOrderType = updateOrderType;
        }

        public void Bind()
        {
            Component.EnabledChanged += MonogameComponent_EnabledChanged;
            updateOrderType.OrderChanged += UpdateOrderType_OrderChanged;
        }

        public void Unbind()
        {
            Component.EnabledChanged -= MonogameComponent_EnabledChanged;
            updateOrderType.OrderChanged -= UpdateOrderType_OrderChanged;
        }

        private void MonogameComponent_EnabledChanged(object sender, EventArgs e)
        {
            EnabledChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateOrderType_OrderChanged(object sender, EventArgs e)
        {
            UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Update(GameTime gameTime)
        {
            updateComponent?.Update(gameTime);
        }

        public override string ToString()
        {
            return $"Target: {Component}, enabled: {Enabled}, order: {UpdateOrder}";
        }
    }
}
