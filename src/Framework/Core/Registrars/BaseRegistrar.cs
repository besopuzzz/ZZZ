using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Registrars;

namespace ZZZ.Framework
{
    public abstract class BaseRegistrar<T> : Disposable, IRegistrar, IGameComponent, IDrawable, IUpdateable
        where T : IComponent
    {
        protected GameManager GameManager => manager;
        protected Scene Scene => scene;
        public bool Initialized => initialized;
        public Type Target => typeof(T);
        public int DrawOrder
        {
            get
            {
                return _drawOrder;
            }
            set
            {
                if (_drawOrder != value)
                {
                    _drawOrder = value;
                    DrawOrderChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    VisibleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    EnabledChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public int UpdateOrder
        {
            get
            {
                return _updateOrder;
            }
            set
            {
                if (_updateOrder != value)
                {
                    _updateOrder = value;
                    UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        Scene IRegistrar.Scene { get => scene; set => scene = value; }
        GameManager IRegistrar.GameManager { get => manager; set => manager = value; }

        private Scene scene;
        private GameManager manager;
        private bool initialized = false;
        private bool _visible = true;
        private bool _enabled = true;
        private int _drawOrder;
        private int _updateOrder;

        /// <summary>
        /// Инициализирует новый экземпляр регистратора
        /// </summary>
        /// <param name="gameManager">Экземпляр игрового менеджера.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseRegistrar()
        {

        }

        protected virtual void Initialize()
        {

        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Draw(GameTime gameTime)
        {

        }

        protected virtual void OnReception(T component)
        {
        }

        protected virtual void OnDeparture(T component)
        {
        }

        private void Component_EnabledChanged(object sender, EventArgs e)
        {
            var component = (T)sender;

            var enabled = component.Enabled;

            if (this is IAcceptingChangesRegistrar<T> acceptingChanges)
                acceptingChanges.AcceptingChanges(component);

            if (!enabled)
            {
                if (this is IOnlyEnabledRegistrar<T> onlyEnabled)
                    onlyEnabled.EnabledDeparture(component);

                if (this is IOnlyDisabledRegistrar<T> onlyDisabled)
                    onlyDisabled.DisabledReception(component);
            }
            else
            {
                if (this is IOnlyEnabledRegistrar<T> onlyEnabled)
                    onlyEnabled.EnabledReception(component);

                if (this is IOnlyDisabledRegistrar<T> onlyDisabled)
                    onlyDisabled.DisabledDeparture(component);
            }
        }
        void IRegistrar.RegistrationObject(IComponent comp)
        {
            if (!Target.IsAssignableFrom(comp.GetType()))
                return;

            T component = (T)comp;

            component.EnabledChanged += Component_EnabledChanged;

            var enabled = component.Enabled;

            if (this is IAnyRegistrar<T> any)
                any.Reception(component);

            if (enabled)
            {
                if (this is IOnlyEnabledRegistrar<T> onlyEnabled)
                    onlyEnabled.EnabledReception(component);
            }
            else if (this is IOnlyDisabledRegistrar<T> onlyDisabled)
                onlyDisabled.DisabledReception(component);

            OnReception(component);
        }

        void IRegistrar.DeregistrationObject(IComponent comp)
        {
            if (!Target.IsAssignableFrom(comp.GetType()))
                return;

            T component = (T)comp;

            component.EnabledChanged -= Component_EnabledChanged;

            var enabled = component.Enabled;

            if (this is IAnyRegistrar<T> any)
                any.Departure(component);

            if (enabled)
            {
                if (this is IOnlyEnabledRegistrar<T> onlyEnabled)
                    onlyEnabled.EnabledDeparture(component);
            }
            else if (this is IOnlyDisabledRegistrar<T> onlyDisabled)
                onlyDisabled.DisabledDeparture(component);

            OnDeparture(component);
        }

        void IGameComponent.Initialize()
        {
            Initialize();
            initialized = true;
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
