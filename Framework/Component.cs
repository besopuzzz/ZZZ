namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый абстрактный компонент. Только для наследования.
    /// </summary>
    public abstract class Component : IComponent
    {
        /// <inheritdoc cref="IComponent.Enabled"/>
        public bool Enabled
        {
            get
            {
                return enabled & (Owner != null ? Owner.Enabled : true);
            }

            set
            {
                if (Enabled == value & enabled == value)
                    return;

                enabled = value;

                LocalEnabledChanged(this, value);
            }
        }

        /// <inheritdoc cref="IComponent.Owner"/>
        public IContainer Owner => container;
        IContainer IComponent.Owner {
            get => container;
            set
            {
                if (container == value)
                    return;

                if (container != null)
                    container.EnabledChanged -= (sender, result) => LocalEnabledChanged(this, result);

                if (value != null)
                    value.EnabledChanged += (sender, result) => LocalEnabledChanged(this, result);

                container = value;
                OnContainerChanged();
            }
        }

        /// <inheritdoc cref="IComponent.EnabledChanged"/>
        public event EventHandler<IComponent, bool> EnabledChanged;

        private IContainer container;
        private bool enabled = true;
        private bool disposed = false;

        /// <inheritdoc cref="IComponent.Startup"/>
        protected virtual void Startup()
        {

        }

        /// <inheritdoc cref="IComponent.Shutdown"/>
        protected virtual void Shutdown()
        {

        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Owner"/> было изменено.
        /// </summary>
        protected virtual void OnContainerChanged()
        {

        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Enabled"/> было изменено.
        /// </summary>
        protected virtual void OnEnabledChanged()
        {

        }

        /// <inheritdoc cref="IContainer.AddContainer"/>
        protected T AddContainer<T>(T child) where T : IContainer => Owner.AddContainer(child);

        /// <inheritdoc cref="IContainer.RemoveContainer"/>
        protected void RemoveContainer<T>(T child) where T : IContainer => Owner.RemoveContainer(child);

        /// <inheritdoc cref="IContainer.AddComponent"/>
        protected T AddComponent<T>(T component) where T : IComponent => Owner.AddComponent(component);

        /// <inheritdoc cref="IContainer.RemoveComponent"/>
        protected void RemoveComponent<T>(T component) where T : IComponent => Owner.RemoveComponent(component);

        /// <inheritdoc cref="IContainer.GetComponent"/>
        protected T GetComponent<T>() where T : IComponent => Owner.GetComponent<T>();

        /// <inheritdoc cref="IContainer.GetComponent"/>
        protected object GetComponent(Type type) => Owner.GetComponent(type);

        /// <inheritdoc cref="IContainer.GetContainer"/>
        protected T GetContainer<T>() where T : IContainer => Owner.GetContainer<T>();

        /// <inheritdoc cref="IContainer.GetComponents"/>
        protected IEnumerable<IComponent> GetComponents() => Owner.GetComponents();

        /// <inheritdoc cref="IContainer.GetContainers"/>
        protected IEnumerable<IContainer> GetContainers() => Owner.GetContainers();

        /// <inheritdoc cref="IContainer.FindContainer"/>
        protected T FindContainer<T>() where T : IContainer => Owner.FindContainer<T>();

        /// <inheritdoc cref="IContainer.FindContainer"/>
        protected IContainer FindContainer(Predicate<IContainer> predicate) => Owner.FindContainer(predicate);

        /// <inheritdoc cref="IContainer.FindContainers"/>
        protected IEnumerable<T> FindContainers<T>() where T : IContainer => Owner.FindContainers<T>();

        /// <inheritdoc cref="IContainer.FindComponent"/>
        protected T FindComponent<T>() where T : IComponent => Owner.FindComponent<T>();

        /// <inheritdoc cref="IContainer.FindComponent"/>
        protected IComponent FindComponent(Predicate<IComponent> predicate) => Owner.FindComponent(predicate);

        /// <inheritdoc cref="IContainer.FindComponents"/>
        protected IEnumerable<T> FindComponents<T>() where T : IComponent => Owner.FindComponents<T>();

        /// <inheritdoc cref="IContainer.FindComponents"/>
        protected IEnumerable<IComponent> FindComponents(Predicate<IComponent> predicate) => Owner.FindComponents(predicate);

        /// <inheritdoc cref="IComponent.RegistrationComponent{T}(T)"/>
        protected void RegistrationComponent<T>(T component) where T : IComponent
        {
            Owner.RegistrationComponent(component);
        }

        /// <inheritdoc cref="IComponent.UnregistrationComponent{T}(T)"/>
        protected void UnregistrationComponent<T>(T component) where T : IComponent
        {
            Owner.UnregistrationComponent(component);
        }

        /// <inheritdoc cref="IComponent.RegistrationComponents"/>
        protected virtual void RegistrationComponents()
        {
        }

        /// <inheritdoc cref="IComponent.UnregistrationComponents"/>
        protected virtual void UnregistrationComponents()
        {
        }

        /// <summary>
        /// Освобождает управляемые и неуправляемые ресурсы компонента.
        /// </summary>
        /// <param name="disposing">Если <see cref="False"/>, вызов выполнен деконструктором, иначе <see cref="IComponent.Owner"/>'ом.</param>
        /// <remarks>Унаследуйте метод и освободите компонент от управляемых ресурсов (noncontrolresource.Dispose()),
        /// если disposing истина, и от неуправляемых, назначив null.</remarks>
        protected virtual void Dispose(bool disposing)
        {

        }

        private void LocalEnabledChanged(IComponent sender, bool eventArgs)
        {
            OnEnabledChanged();
            EnabledChanged?.Invoke(this, eventArgs);
        }

        ~Component()
        {
            if (disposed)
                return;

            Dispose(disposing: false);
            disposed = true;
        }
        void IDisposable.Dispose()
        {
            if (disposed)
                return;

            Dispose(disposing: true);
            disposed = true;
            GC.SuppressFinalize(this);
        }
        void IComponent.Shutdown()
        {
            Shutdown();
        }
        void IComponent.Startup()
        {
            Startup();
        }
        void IComponent.RegistrationComponent<T>(T component)
        {
            RegistrationComponent(component);
        }
        void IComponent.UnregistrationComponent<T>(T component)
        {
            UnregistrationComponent(component);
        }
        void IComponent.RegistrationComponents()
        {
            RegistrationComponents();
        }
        void IComponent.UnregistrationComponents()
        {
            UnregistrationComponents();
        }
    }
}
