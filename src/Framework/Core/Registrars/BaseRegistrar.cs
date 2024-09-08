using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Registrars;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый класс регистратора компонентов.
    /// </summary>
    /// <typeparam name="T">Тип обрабатываемых компонентов.</typeparam>
    public abstract class BaseRegistrar<T> : Disposable, IRegistrar, IGameComponent, IDrawable, IUpdateable
        where T : IComponent
    {
        /// <summary>
        /// Экземпляр игрового менеджера, который использует этот регистратор.
        /// </summary>
        protected GameManager GameManager => manager;

        /// <summary>
        /// Получает значение, прошел ли инициализацию регистратор.
        /// </summary>
        public bool Initialized => initialized;

        /// <summary>
        /// Тип обрабатываемых компонентов.
        /// </summary>
        public Type Target => typeof(T);

        /// <summary>
        /// Порядок вызова метода <see cref="BaseRegistrar{T}.Draw(GameTime)"/> в отношении к другим регистраторам.
        /// </summary>
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

        /// <summary>
        /// Получает или устанавливает значение, будет ли вызываться метод рисования <see cref="BaseRegistrar{T}.Draw(GameTime)"/>.
        /// </summary>
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

        /// <summary>
        /// Получает или устанавливает значение, будет ли вызываться метод обновления <see cref="BaseRegistrar{T}.Update(GameTime)"/>.
        /// </summary>
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

        /// <summary>
        /// Порядок вызова метода <see cref="BaseRegistrar{T}.Update(GameTime)"/> в отношении к другим регистраторам.
        /// </summary>
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

        /// <summary>
        /// Вызывает событие, когда у свойства <see cref="DrawOrder"/> изменили значение.
        /// </summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// Вызывает событие, когда у свойства <see cref="UpdateOrder"/> изменили значение.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Вызывает событие, когда у свойства <see cref="Enabled"/> изменили значение.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Вызывает событие, когда у свойства <see cref="Visible"/> изменили значение.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        GameManager IRegistrar.GameManager { get => manager; set => manager = value; }

        private GameManager manager;
        private bool initialized = false;
        private bool _visible = true;
        private bool _enabled = true;
        private int _drawOrder;
        private int _updateOrder;

        /// <summary>
        /// Инициализирует новый экземпляр регистратора.
        /// </summary>
        protected BaseRegistrar()
        {

        }

        /// <summary>
        /// Инициализирует регистратор. Инициализация вызывается во время или после загрузки основного класса <see cref="Game"/>.
        /// </summary>
        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// Вызывает метод обновления регистратора. Метод не будет обрабатываться, если значение <see cref="Enabled"/> будет false.
        /// Частота вызова зависит от логики основного класса <see cref="Game"/>.
        /// </summary>
        /// <param name="gameTime">Экземпляр класса содержащий информацию о времени.</param>
        protected virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Вызывает метод рисования регистратора. Метод не будет обрабатываться, если значение <see cref="Visible"/> будет false.
        /// Частота вызова зависит от логики основного класса <see cref="Game"/>.
        /// </summary>
        /// <param name="gameTime">Экземпляр класса содержащий информацию о времени.</param>
        protected virtual void Draw(GameTime gameTime)
        {

        }

        /// <summary>
        /// Вызывает внутреннее событие регистрации нового компонента. 
        /// </summary>
        /// <param name="component">Экземпляр нового компонента.</param>
        protected virtual void OnReception(T component)
        {

        }

        /// <summary>
        /// Вызывает внутреннее событие снятия с регистрации компонента. 
        /// </summary>
        /// <param name="component">Экземпляр нового компонента.</param>
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
