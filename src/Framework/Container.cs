using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    public abstract partial class Container : Disposable
    {
        public virtual string Name
        {
            get => name;
            set => name = value;
        }

        public virtual bool Enabled
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

        public event EventHandler<EventArgs> EnabledChanged;

        [ContentSerializerIgnore]
        public virtual GameObject Owner
        {
            get => owner;
            internal set
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

        ///<inheritdoc cref="GameObject.GameObjectAdded"/>
        public event EventHandler<GameObject, GameObject> GameObjectAdded;

        ///<inheritdoc cref="GameObject.GameObjectRemoved"/>
        public event EventHandler<GameObject, GameObject> GameObjectRemoved;

        ///<inheritdoc cref="GameObject.ComponentAdded"/>
        public event EventHandler<GameObject, Component> ComponentAdded;

        ///<inheritdoc cref="GameObject.ComponentRemoved"/>
        public event EventHandler<GameObject, Component> ComponentRemoved;

        [ContentSerializer(ElementName = "Components")]
        private EventedList<Component> components = new();

        [ContentSerializer(ElementName = "Childs")]
        private EventedList<GameObject> containers = new();

        private GameObject owner;
        private bool enabled = true;
        private bool disposed = false;
        private string name = "";

        protected internal void Awake()
        {
            var comps = components.ToList();
            var conts = containers.ToList();

            containers.ItemAdded += AwakeGameObject;
            containers.ItemRemoved += ShutdownGameObject;

            components.ItemAdded += StartupComponent;
            components.ItemRemoved += ShutdownComponent;

            foreach (var item in comps)
                StartupComponent(components, item);

            foreach (var item in conts.ToList())
                AwakeGameObject(containers, item);
        }

        ///<inheritdoc cref="GameObject.Shutdown"/>
        protected internal void Shutdown()
        {
            var comps = components.ToList();
            var conts = containers.ToList();

            foreach (var item in conts)
                ShutdownGameObject(containers, item);

            foreach (var item in comps)
                ShutdownComponent(components, item);

            containers.ItemAdded -= AwakeGameObject;
            containers.ItemRemoved -= ShutdownGameObject;

            components.ItemAdded -= StartupComponent;
            components.ItemRemoved -= ShutdownComponent;
        }

        protected override void Dispose(bool disposing)
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

        protected virtual void OnComponentAdding<T>(T component)
            where T : Component
        {
            if (GetComponent(component.GetType()) != null)
                throw new ArgumentException(nameof(component) + $" type already exist!");

            component.Owner?.RemoveComponent(component);

            component.Owner = this as GameObject;

            var attr = component.GetType().GetCustomAttributes(typeof(RequiredComponentAttribute), true);

            foreach (RequiredComponentAttribute item in attr)
            {
                if (item?.Type == null)
                    continue; // Empty attribute, do nothing

                AddComponent(item.Type);
            }
        }

        protected virtual void OnComponentAdded<T>(T component) where T : IComponent
        {

        }

        protected virtual void OnComponentRemoving<T>(T component)
            where T : Component
        {
            if (!components.Contains(component))
                throw new ArgumentException("Component not found in GameObject!");

            var attr = component.GetType().GetCustomAttributes(typeof(RequiredComponentAttribute), true);

            foreach (RequiredComponentAttribute item in attr)
            {
                if (item?.Type == null)
                    continue; // Empty attribute, do nothing

                IComponent requier = GetComponent(item.Type);

                if (requier == null)
                    continue;

                RemoveComponent(requier as Component);
            }
        }

        protected virtual void OnComponentRemoved<T>(T component) where T : Component
        {
            component.Owner = null;
        }

        protected virtual void OnGameObjectAdding<T>(T container) where T : GameObject
        {
            container.Owner?.RemoveGameObject(container);
            container.Owner = this as GameObject;
        }

        protected virtual void OnGameObjectAdded<T>(T container) where T : GameObject
        {

        }

        protected virtual void OnGameObjectRemoving<T>(T container) where T : GameObject
        {

        }

        protected virtual void OnGameObjectRemoved<T>(T container) where T : GameObject
        {
            container.Owner = null;
        }

        /// <summary>
        /// Возвращает имя контейнера.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = string.IsNullOrWhiteSpace(Name) ? "No name" : Name;

            return $"GameObject: {name}";
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

        private void ShutdownGameObject(EventedList<GameObject> sender, GameObject container)
        {
            container.Shutdown();
        }

        private void AwakeGameObject(EventedList<GameObject> sender, GameObject container)
        {
            container.Awake();
        }

        private void ShutdownComponent(EventedList<Component> sender, Component component)
        {
            component.InternalShutdown();
        }

        private void StartupComponent(EventedList<Component> sender, Component component)
        {
            component.InternalAwake();
        }

        private T FindGameObjectRecursiveDown<T>(Container container, Predicate<T> predicate) where T : GameObject
        {
            var containers = container.GetGameObjects();
            var finded = (T)containers.FirstOrDefault(x => x is T t && predicate.Invoke(t));

            if (finded != null)
                return finded;

            foreach (var item in containers)
            {
                finded = FindGameObjectRecursiveDown<T>(item, predicate);

                if (finded != null)
                    return finded;
            }

            return finded;
        }

        private IEnumerable<T> FindGameObjectsRecursiveDown<T>(Container container, Predicate<T> predicate) where T : GameObject
        {
            var containers = container.GetGameObjects();
            var findeds = containers.Where(x => x is T t && predicate.Invoke(t)).Cast<T>().ToList();

            foreach (var item in containers)
            {
                findeds.AddRange(FindGameObjectsRecursiveDown<T>(item, predicate));
            }

            return findeds;
        }

        private T FindComponentRecursiveDown<T>(Container container, Predicate<T> predicate) where T : IComponent
        {
            var containers = container.GetGameObjects();

            foreach (var item in containers)
            {
                var founded = FindComponentRecursiveDown<T>(item, predicate);

                if (founded != null)
                    return founded;
            }

            return default;
        }

        private IEnumerable<T> FindComponentsRecursiveDown<T>(Container container, Predicate<T> predicate) where T : IComponent
        {
            var containers = container.GetGameObjects();
            var findeds = container.GetComponents().Where(x => x is T t && predicate.Invoke(t)).Cast<T>().ToList();

            foreach (var item in containers)
            {
                findeds.AddRange(FindComponentsRecursiveDown<T>(item, predicate));
            }

            return findeds;
        }

        private void LocalEnabledChanged(object sender, EventArgs e)
        {
            OnEnabledChanged();
            EnabledChanged?.Invoke(this, e);
        }

    }
}
