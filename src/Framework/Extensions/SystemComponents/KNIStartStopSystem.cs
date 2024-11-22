using ZZZ.Framework.Designing.UnityStyle.Systems;

namespace ZZZ.Framework.Extensions.SystemComponents
{
    internal sealed class KNIStartStopSystem : StartStopSystem, IUpdateable, IGameComponent
    {
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                if (base.Enabled == value)
                    return;

                base.Enabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int UpdateOrder
        {
            get => order;
            set
            {
                if (order == value)
                    return;

                order = value;

                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> EnabledChanged;

        private int order = 0;

        public void Update(GameTime gameTime)
        {
            Validate();
        }

        public void Initialize()
        {

        }
    }
}
