namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый компонент. Только для наследования.
    /// </summary>
    public abstract class Component : Disposable, IComponent
    {
        /// <inheritdoc cref="IComponent.Enabled"/>
        public bool Enabled
        {
            get
            {
                return enabled & (Owner == null || Owner.Enabled);
            }

            set
            {
                if (Enabled == value & enabled == value)
                    return;

                enabled = value;

                LocalEnabledChanged(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc cref="IComponent.Started"/>
        public bool Started => started;

        /// <inheritdoc cref="IComponent.Owner"/>
        public GameObject Owner => owner;
        GameObject IComponent.Owner
        {
            get => owner;
            set
            {
                if (owner == value)
                    return;

                if (owner != null)
                    owner.EnabledChanged -= LocalEnabledChanged;

                if (value != null)
                    value.EnabledChanged += LocalEnabledChanged;

                owner = value;

                OnOwnerChanged();
            }
        }


        /// <inheritdoc cref="IComponent.EnabledChanged"/>
        public event EventHandler<EventArgs> EnabledChanged;

        private GameObject owner;
        private bool enabled = true;
        private bool started = false;

        public Component()
        {
        }

        /// <inheritdoc cref="IComponent.Awake"/>
        protected virtual void Awake()
        {

        }

        /// <inheritdoc cref="IComponent.Shutdown"/>
        protected virtual void Shutdown()
        {

        }

        protected void Destroy()
        {

        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Owner"/> было изменено.
        /// </summary>
        protected virtual void OnOwnerChanged()
        {

        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Enabled"/> было изменено.
        /// </summary>
        protected virtual void OnEnabledChanged()
        {

        }

        /// <inheritdoc cref="GameObject.AddGameObject"/>
        protected T AddGameObject<T>(T child) where T : GameObject => Owner.AddGameObject(child);

        /// <inheritdoc cref="GameObject.RemoveGameObject"/>
        protected void RemoveGameObject<T>(T child) where T : GameObject => Owner.RemoveGameObject(child);

        /// <inheritdoc cref="GameObject.AddComponent"/>
        protected T AddComponent<T>(T component) where T : IComponent => Owner.AddComponent(component);

        /// <inheritdoc cref="GameObject.AddComponent"/>
        protected T AddComponent<T>() where T : IComponent, new() => Owner.AddComponent<T>();

        /// <inheritdoc cref="GameObject.RemoveComponent"/>
        protected void RemoveComponent<T>(T component) where T : IComponent => Owner.RemoveComponent(component);

        /// <inheritdoc cref="GameObject.RemoveComponent"/>
        protected void RemoveComponent<T>() where T : IComponent, new() => Owner.RemoveComponent<T>();

        /// <inheritdoc cref="GameObject.GetComponent{T}()"/>
        protected T GetComponent<T>() where T : IComponent => Owner.GetComponent<T>();

        /// <inheritdoc cref="GameObject.GetComponent"/>
        protected object GetComponent(Type type) => Owner.GetComponent(type);

        /// <inheritdoc cref="GameObject.GetGameObject"/>
        protected T GetGameObject<T>() where T : GameObject => Owner.GetGameObject<T>();

        /// <inheritdoc cref="GameObject.GetComponents"/>
        protected IEnumerable<IComponent> GetComponents() => Owner.GetComponents();

        /// <inheritdoc cref="GameObject.GetComponents"/>
        protected IEnumerable<IComponent> GetComponents(Func<IComponent, bool> func) => Owner.GetComponents(func);

        /// <inheritdoc cref="GameObject.GetComponents{T}()"/>
        protected IEnumerable<T> GetComponents<T>() where T : IComponent => Owner.GetComponents<T>();

        /// <inheritdoc cref="GameObject.GetGameObjects"/>
        protected IEnumerable<GameObject> GetGameObjects() => Owner.GetGameObjects();

        /// <inheritdoc cref="GameObject.FindGameObject{T}()"/>
        protected T FindGameObject<T>() where T : GameObject => Owner.FindGameObject<T>();

        /// <inheritdoc cref="GameObject.FindGameObject{T}(Predicate{T})"/>
        protected GameObject FindGameObject(Predicate<GameObject> predicate) => Owner.FindGameObject(predicate);

        /// <inheritdoc cref="GameObject.FindGameObjects"/>
        protected IEnumerable<T> FindGameObjects<T>() where T : GameObject => Owner.FindGameObjects<T>();

        /// <inheritdoc cref="GameObject.FindComponent{T}()"/>
        protected T FindComponent<T>() where T : IComponent => Owner.FindComponent<T>();

        /// <inheritdoc cref="GameObject.FindIComponent{T}(Predicate{T})"/>
        protected IComponent FindComponent(Predicate<IComponent> predicate) => Owner.FindIComponent(predicate);

        /// <inheritdoc cref="GameObject.FindComponents"/>
        protected IEnumerable<T> FindComponents<T>() where T : IComponent => Owner.FindComponents<T>();

        /// <inheritdoc cref="GameObject.FindComponents"/>
        protected IEnumerable<IComponent> FindComponents(Predicate<IComponent> predicate) => Owner.FindComponents(predicate);

        private void LocalEnabledChanged(object sender, EventArgs eventArgs)
        {
            OnEnabledChanged();
            EnabledChanged?.Invoke(sender, EventArgs.Empty);
        }

        void IComponent.Shutdown()
        {
            Shutdown();

            started = false;
        }
        void IComponent.Awake()
        {
            Awake();

            started = true;
        }
    }
}
