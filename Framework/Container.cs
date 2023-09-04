using System.Reflection;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый контейнер.
    /// </summary>
    public class Container : IContainer
    {
        ///<inheritdoc cref="IContainer.Name"/>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <inheritdoc cref="IContainer.Enabled"/>
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

                LocalEnabledChanged(this, value);
            }
        }

        /// <inheritdoc cref="IContainer.EnabledChanged"/>
        public event EventHandler<IContainer, bool> EnabledChanged;

        /// <inheritdoc cref="IContainer.Owner"/>
        public IContainer Owner => owner;
        IContainer IContainer.Owner
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

        ///<inheritdoc cref="IContainer.ContainerAdded"/>
        public event EventHandler<IContainer, IContainer> ContainerAdded;

        ///<inheritdoc cref="IContainer.ContainerRemoved"/>
        public event EventHandler<IContainer, IContainer> ContainerRemoved;

        ///<inheritdoc cref="IContainer.ComponentAdded"/>
        public event EventHandler<IContainer, IComponent> ComponentAdded;

        ///<inheritdoc cref="IContainer.ComponentRemoved"/>
        public event EventHandler<IContainer, IComponent> ComponentRemoved;

        private EventedList<IComponent> components = new();
        private EventedList<IContainer> containers = new();
        private IContainer owner;
        private bool enabled = true;
        private bool disposed = false;
        private string name = "";
        private bool started = false;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Container"/> без владеемых компонентов и контейнеров.
        /// </summary>
        public Container()
        {

        }

        ///<inheritdoc cref="IContainer.Startup"/>
        protected virtual void Startup()
        {
            var comps = components.ToList();
            var conts = containers.ToList();

            containers.ItemAdded += StartupContainer;
            containers.ItemRemoved += ShoutdownContainer;

            components.ItemAdded += StartupComponent;
            components.ItemRemoved += ShoutdownComponent;

            foreach (var item in comps)
                StartupComponent(components, item);

            foreach (var item in conts.ToList())
                StartupContainer(containers, item);
        }

        ///<inheritdoc cref="IContainer.Shutdown"/>
        protected virtual void Shoutdown()
        {
            var comps = components.ToList();
            var conts = containers.ToList();

            foreach (var item in conts)
                ShoutdownContainer(containers, item);

            foreach (var item in comps)
                ShoutdownComponent(components, item);

            containers.ItemAdded -= StartupContainer;
            containers.ItemRemoved -= ShoutdownContainer;

            components.ItemAdded -= StartupComponent;
            components.ItemRemoved -= ShoutdownComponent;
        }

        /// <summary>
        /// Освобождает управляемые и неуправляемые ресурсы контейнера.
        /// </summary>
        /// <param name="disposing">Если <see href="false"/>, вызов выполнен деконструктором, иначе <see cref="IComponent.Owner"/>'ом.</param>
        /// <remarks>Унаследуйте метод и освободите контейнер от управляемых ресурсов (noncontrolresource.Dispose()),
        /// если disposing истина, и от неуправляемых, назначив null.</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    (components[i] as IDisposable)?.Dispose();
                }
                for (int i = 0; i < containers.Count; i++)
                {
                    (containers[i] as IDisposable)?.Dispose();
                }
            }

            components = null;
            containers = null;
        }

        ///<inheritdoc cref = "IContainer.ComponentAdded" />
        protected virtual void OnComponentAdded<T>(T component) where T : IComponent
        {

        }

        ///<inheritdoc cref="IContainer.ComponentRemoved"/>
        protected virtual void OnComponentRemoved<T>(T component) where T : IComponent
        {

        }

        ///<inheritdoc cref="IContainer.ContainerAdded"/>
        protected virtual void OnContainerAdded<T>(T container) where T : IContainer
        {

        }

        ///<inheritdoc cref="IContainer.ContainerRemoved"/>
        protected virtual void OnContainerRemoved<T>(T container) where T : IContainer
        {

        }

        ///<inheritdoc cref="IContainer.AddComponent{T}(T)"/>
        public T AddComponent<T>(T component) where T : IComponent
        {
            component.Owner?.RemoveComponent(component);
            component.Owner = this;

            Container.FindAndCreateRequireComponents(component);
            components.Add(component);

            OnComponentAdded(component);
            ComponentAdded?.Invoke(this, component);

            return component;
        }

        ///<inheritdoc cref="IContainer.RemoveComponent{T}(T)"/>
        public void RemoveComponent<T>(T component) where T : IComponent
        {
            if (!components.Contains(component))
                return;

            Container.FindAndRemoveRequireComponents(component);
            components.Remove(component);

            OnComponentRemoved(component);
            ComponentRemoved?.Invoke(this, component);

            component.Owner = null;
        }

        ///<inheritdoc cref="IContainer.AddContainer{T}(T)"/>
        public T AddContainer<T>(T container) where T : IContainer
        {
            container.Owner?.RemoveContainer(container);
            container.Owner = this;

            containers.Add(container);

            OnContainerAdded(container);
            ContainerAdded?.Invoke(this, container);

            return container;
        }

        ///<inheritdoc cref="IContainer.RemoveContainer{T}(T)"/>
        public void RemoveContainer<T>(T container) where T : IContainer
        {
            containers.Remove(container);

            OnContainerRemoved(container);
            ContainerRemoved?.Invoke(this, container);

            container.Owner = null;
        }

        ///<inheritdoc cref="IContainer.GetComponent{T}"/>
        public T GetComponent<T>() where T : IComponent
        {
            return (T)components.Find(x => x is T);
        }

        ///<inheritdoc cref="IContainer.GetComponent(Type)"/>
        public IComponent GetComponent(Type type)
        {
            return components.Find(x => x.GetType() == type);
        }

        ///<inheritdoc cref="IContainer.GetComponents"/>
        public IEnumerable<IComponent> GetComponents()
        {
            return components.ToArray();
        }

        ///<inheritdoc cref="IContainer.GetContainer{T}"/>
        public T GetContainer<T>() where T : IContainer
        {
            return (T)containers.Find(x => x is T);
        }

        ///<inheritdoc cref="IContainer.GetContainers"/>
        public IEnumerable<IContainer> GetContainers()
        {
            return containers.ToArray();
        }

        ///<inheritdoc cref="IContainer.FindContainer{T}()"/>
        public T FindContainer<T>() where T : IContainer
        {
            if (Owner == null)
                return FindContainerRecursiveDown<T>(this, (x) => true);

            return Owner.FindContainer<T>();
        }

        ///<inheritdoc cref="IContainer.FindContainer{T}(Predicate{T})"/>
        public T FindContainer<T>(Predicate<T> predicate) where T : IContainer
        {
            if (Owner == null)
                return FindContainerRecursiveDown<T>(this, predicate);

            return Owner.FindContainer<T>();
        }

        ///<inheritdoc cref="IContainer.FindContainers{T}()"/>
        public IEnumerable<T> FindContainers<T>() where T : IContainer
        {
            if (Owner == null)
                return FindContainersRecursiveDown<T>(this, (x) => true);

            return Owner.FindContainers<T>();
        }

        ///<inheritdoc cref="IContainer.FindContainers(Predicate{IContainer})"/>
        public IEnumerable<IContainer> FindContainers(Predicate<IContainer> predicate)
        {
            if (Owner == null)
                return FindContainersRecursiveDown<IContainer>(this, predicate);

            return Owner.FindContainers<IContainer>();
        }

        ///<inheritdoc cref="IContainer.FindComponent{T}()"/>
        public T FindComponent<T>() where T : IComponent
        {
            if (Owner == null)
                return FindComponentRecursiveDown<T>(this, (x) => true);

            return Owner.FindComponent<T>();
        }

        ///<inheritdoc cref="IContainer.FindContainer{T}(Predicate{T})"/>
        public T FindComponent<T>(Predicate<T> predicate) where T : IComponent
        {
            if (Owner == null)
                return FindComponentRecursiveDown<T>(this, predicate);

            return Owner.FindComponent<T>();
        }

        ///<inheritdoc cref="IContainer.FindComponents{T}()"/>
        public IEnumerable<T> FindComponents<T>() where T : IComponent
        {
            if (Owner == null)
                return FindComponentsRecursiveDown<T>(this, (x) => true);

            return Owner.FindComponents<T>();
        }

        ///<inheritdoc cref="IContainer.FindComponents{T}()"/>
        public IEnumerable<IComponent> FindComponents(Predicate<IComponent> predicate)
        {
            if (Owner == null)
                return FindComponentsRecursiveDown<IComponent>(this, predicate);

            return Owner.FindComponents<IComponent>();
        }

        /// <summary>
        /// Возвращает имя контейнера.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = string.IsNullOrWhiteSpace(Name) ? "Noname" : Name;

            return $"Container: {name}";
        }

        /// <inheritdoc cref="IContainer.RegistrationComponent{T}(T)"/>
        protected internal virtual void RegistrationComponent<T>(T component) where T : IComponent
        {
            Owner.RegistrationComponent(component);
        }

        /// <inheritdoc cref="IContainer.UnregistrationComponent{T}(T)"/>
        protected internal virtual void UnregistrationComponent<T>(T component) where T : IComponent
        {
            Owner.UnregistrationComponent(component);
        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Owner"/> было изменено.
        /// </summary>
        protected virtual void OnOwnerChanged()
        {

        }

        /// <inheritdoc cref="EnabledChanged"/>
        protected virtual void OnEnabledChanged()
        {

        }
        private void ShoutdownContainer(EventedList<IContainer> sender, IContainer  container)
        {
            container.Shutdown();
        }
        private void StartupContainer(EventedList<IContainer> sender, IContainer container)
        {
            container.Startup();
        }
        private void ShoutdownComponent(EventedList<IComponent> sender, IComponent component)
        {
            component.Shutdown();
            component.UnregistrationComponents();
        }
        private void StartupComponent(EventedList<IComponent> sender, IComponent component)
        {
            component.RegistrationComponents();
            component.Startup();
        }
        private static void FindAndCreateRequireComponents(IComponent component)
        {
            var attr = component.GetType().GetCustomAttributes<RequireComponentAttribute>();

            foreach (RequireComponentAttribute item in attr)
            {
                Container.CreateRequire(item, component);
            }
        }
        private static void FindAndRemoveRequireComponents(IComponent component)
        {
            var attr = component.GetType().GetCustomAttributes<RequireComponentAttribute>();

            foreach (RequireComponentAttribute item in attr)
            {
                if (item.Remove)
                    Container.RemoveRequire(item, component);
            }
        }
        private static void CreateRequire(RequireComponentAttribute attr, IComponent component)
        {
            if (attr?.Type == null)
                return;

            if (attr.Type == component.GetType())
                throw new Exception($"The require component cannot be of the same type as {component.GetType().Name}");

            IContainer owner = component.Owner;

            if (attr.ToOwner)
                owner = owner.Owner;

            if (owner == null)
                return;

            if (!attr.Duplicate & owner.GetComponent(attr.Type) != null)
                return;

            IComponent required = Activator.CreateInstance(attr.Type) as IComponent ?? throw new Exception("The require component is not inherit from a IComponent!");
            owner.AddComponent(required);
        }
        private static void RemoveRequire(RequireComponentAttribute attr, IComponent component)
        {
            if (!attr.Remove)
                return;

            IContainer owner = component.Owner;

            if (attr.ToOwner)
                owner = owner.Owner;

            if (owner == null)
                return;

            IComponent finded = owner.GetComponent(attr.Type);

            if (finded != null)
                component.Owner.RemoveComponent(finded);
        }

        private T FindContainerRecursiveDown<T>(IContainer container, Predicate<T> predicate) where T : IContainer
        {
            var containers = container.GetContainers();
            var finded = (T)containers.FirstOrDefault(x => x is T t && predicate.Invoke(t));

            if (finded != null)
                return finded;

            foreach (var item in containers)
            {
                finded = FindContainerRecursiveDown<T>(item, predicate);

                if (finded != null)
                    return finded;
            }

            return finded;
        }
        private IEnumerable<T> FindContainersRecursiveDown<T>(IContainer container, Predicate<T> predicate) where T : IContainer
        {
            var containers = container.GetContainers();
            var findeds = containers.Where(x => x is T t && predicate.Invoke(t)).Cast<T>().ToList();

            foreach (var item in containers)
            {
                findeds.AddRange(FindContainersRecursiveDown<T>(item, predicate));
            }

            return findeds;
        }
        private T FindComponentRecursiveDown<T>(IContainer container, Predicate<T> predicate) where T : IComponent
        {
            var containers = container.GetContainers();
            var finded = (T)components.FirstOrDefault(x => x is T t && predicate.Invoke(t));

            if (finded != null)
                return finded;

            foreach (var item in containers)
            {
                finded = FindComponentRecursiveDown<T>(item, predicate);

                if (finded != null)
                    return finded;
            }

            return finded;
        }
        private IEnumerable<T> FindComponentsRecursiveDown<T>(IContainer container, Predicate<T> predicate) where T : IComponent
        {
            var containers = container.GetContainers();
            var findeds = components.Where(x => x is T t && predicate.Invoke(t)).Cast<T>().ToList();

            foreach (var item in containers)
            {
                findeds.AddRange(FindComponentsRecursiveDown<T>(item, predicate));
            }

            return findeds;
        }

        private void LocalEnabledChanged(IContainer sender, bool result)
        {
            OnEnabledChanged();
            EnabledChanged?.Invoke(this, result);
        }

        /// <summary>
        /// Деконструктор контейнера.
        /// </summary>
        ~Container()
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
        void IContainer.Shutdown()
        {
            Shoutdown();
        }
        void IContainer.Startup()
        {
            Startup();
        }
        void IContainer.RegistrationComponent<T>(T component)
        {
            RegistrationComponent(component);
        }
        void IContainer.UnregistrationComponent<T>(T component)
        {
            UnregistrationComponent(component);
        }
    }
}
