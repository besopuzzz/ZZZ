namespace ZZZ.Framework.Monogame
{
    public interface IMonogameRegistrar : IRegistrar, IGameComponent, IDrawable, IUpdateable
    {
        Game Game { get; internal set; }
    }

    public abstract class MonogameRegistrar<TComponent> : Registrar<TComponent>, IMonogameRegistrar
        where TComponent : class, IComponent
    {
        /// <summary>
        /// QWEQWEQWE
        /// </summary>
        /// <summary xml:lang="ru">
        /// sdsdsd
        /// </summary>
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
        public int DrawOrder
        {
            get => drawOrder;
            set
            {
                if (drawOrder == value)
                    return;

                drawOrder = value;
                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        [ContentSerializerIgnore]
        protected Game Game { get; private set; }
        Game IMonogameRegistrar.Game { get => Game; set => Game = value; }

        private int updateOrder;
        private int drawOrder;

        event EventHandler<EventArgs> IDrawable.VisibleChanged
        {
            add
            {
                EnabledChanged += (sender, result) => value.Invoke(sender, EventArgs.Empty);
            }

            remove
            {
                EnabledChanged += (sender, result) => value.Invoke(sender, EventArgs.Empty);
            }
        }

        event EventHandler<EventArgs> IUpdateable.EnabledChanged
        {
            add
            {
                EnabledChanged += (sender, result) => value.Invoke(sender, EventArgs.Empty);
            }

            remove
            {
                EnabledChanged += (sender, result) => value.Invoke(sender, EventArgs.Empty);
            }
        }
        bool IDrawable.Visible => Enabled;

        protected override void Startup()
        {
        }
        protected override void Reception(TComponent component)
        {

        }
        protected override void Departure(TComponent component)
        {

        }

        protected virtual void Draw(GameTime gameTime)
        {

        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        void IGameComponent.Initialize()
        {

        }

        void IDrawable.Draw(GameTime gameTime)
        {
            Draw(gameTime);
        }

        void IUpdateable.Update(GameTime gameTime)
        {
            Update(gameTime);
        }
    }
}
