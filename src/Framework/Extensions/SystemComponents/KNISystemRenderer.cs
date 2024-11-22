using ZZZ.Framework.Designing.UnityStyle.Systems;

namespace ZZZ.Framework.Extensions.SystemComponents
{
    internal sealed class KNISystemRenderer : RenderSystem, IDrawable, IGameComponent
    {
        public int DrawOrder
        {
            get
            {
                return order;
            }

            set
            {
                if (order == value)
                    return;

                order = value;

                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Visible
        {
            get
            {
                return base.Enabled;
            }

            set
            {
                if (Enabled == value)
                    return;

                Enabled = value;

                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        private int order = 0;

        public void Draw(GameTime gameTime)
        {
            Render();
        }

        void IGameComponent.Initialize()
        {

        }
    }
}
