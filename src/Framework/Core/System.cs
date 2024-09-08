using ZZZ.Framework.Core.Rendering;

namespace ZZZ.Framework.Core
{
    public abstract class System<TComponent, TEntity> : Disposable, IGameComponent, IDrawable, IUpdateable, ISystem<TComponent, TEntity>
        where TEntity : BaseEntity<TComponent, TEntity>
        where TComponent : IComponent
    {
        /// <summary>
        /// Получает значение, прошел ли инициализацию регистратор.
        /// </summary>
        public bool Initialized => initialized;

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

        public IGameInstance Game { get;  set; }

        private SceneEntity<TComponent, TEntity> root;
        private bool initialized = false;
        private bool _visible = true;
        private bool _enabled = true;
        private int _drawOrder;
        private int _updateOrder;

        /// <summary>
        /// Инициализирует новый экземпляр регистратора.
        /// </summary>
        protected System() { }

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

        void IGameComponent.Initialize()
        {
            Initialize();

            root = new SceneEntity<TComponent, TEntity>();
            root.Initialize(SceneLoader.CurrentScene, this);

            initialized = true;
        }

        protected abstract TEntity OnProcess(TComponent component);

        protected IEnumerable<TEntity> Entities => root.GetEntitiesInternal() as IEnumerable<TEntity>;

        protected BaseEntity<TComponent, TEntity> BaseEntity => root;

        BaseEntity<TComponent, TEntity> ISystem<TComponent, TEntity>.Process(TComponent component)
        {
            if (component is not TComponent component1)
                return null;

            return OnProcess(component1);
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