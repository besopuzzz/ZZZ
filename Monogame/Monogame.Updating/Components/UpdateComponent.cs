using Microsoft.Xna.Framework;

namespace ZZZ.Framework.Monogame.Updating.Components
{
    public abstract class UpdateComponent : Component, IUpdateComponent
    {
        public int UpdateOrder
        {
            get => updateOrder;
            set
            {
                if (updateOrder == value)
                    return;

                updateOrder = value;
                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private int updateOrder = 0;
        protected override void RegistrationComponents()
        {
            RegistrationComponent<IUpdateComponent>(this);
        }
        protected override void UnregistrationComponents()
        {
            UnregistrationComponent<IUpdateComponent>(this);
        }
        protected virtual void Update(GameTime gameTime)
        {

        }

        void IUpdateComponent.Update(GameTime gameTime)
        {
            Update(gameTime);
        }
    }
}
