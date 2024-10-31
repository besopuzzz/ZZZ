using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core
{
    public abstract class System<TEntity, TEntityComponent, TComponent> : Disposable, IGameComponent, IDrawable, IUpdateable, ISystem<TEntity, TEntityComponent, TComponent>
        where TEntity : Entity<TEntity, TEntityComponent, TComponent>
        where TEntityComponent : EntityComponent<TEntity, TEntityComponent, TComponent>
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



        public IGameInstance Game { get; set; }

        private Entity<TEntity, TEntityComponent, TComponent> root;
        private bool initialized = false;
        private bool _visible = true;
        private bool _enabled = true;
        private int _drawOrder;
        private int _updateOrder;

        /// <summary>
        /// Инициализирует новый экземпляр регистратора.
        /// </summary>
        protected System()
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

        void IGameComponent.Initialize()
        {
            Initialize();

            root = CreateEntity(default);/* new Entity<TEntity, TEntityComponent, TComponent>();*/
            root.Initialize(SceneLoader.CurrentScene, this);

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

        protected abstract TEntity CreateEntity(TEntity owner);
        protected abstract TEntityComponent CreateEntityComponent(TEntity owner, TComponent component);
        protected void ForEveryComponent(Action<TEntityComponent> action) => root.ForEveryComponent(action);
        protected void ForEveryChild(Action<TEntity> action) => root.ForEveryChild(action);

        TEntity ISystem<TEntity, TEntityComponent, TComponent>.CreateEntity(Entity<TEntity, TEntityComponent, TComponent> owner)
        {
            return CreateEntity(owner as TEntity);
        }

        TEntityComponent ISystem<TEntity, TEntityComponent, TComponent>.CreateEntityComponent(Entity<TEntity, TEntityComponent, TComponent> owner, TComponent component)
        {
            return CreateEntityComponent(owner as TEntity, component);
        }
    }
}
