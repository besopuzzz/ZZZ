using ZZZ.Framework.Designing.UnityStyle.Systems;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.Extensions.SystemComponents
{
    internal sealed class KNiSystemUpdater : UpdaterSystem, IUpdateable, IGameComponent
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

        protected override UpdateOrderList UpdateOrderTypes => types;

        private UpdateOrderList types;

        private int order = 0;
        protected override void Awake()
        {
            types = new UpdateOrderList();

            base.Awake();
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime.ElapsedGameTime);
        }

        void IGameComponent.Initialize()
        {

        }
    }
}
