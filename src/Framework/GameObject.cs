using System.Linq;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый контейнер.
    /// </summary>
    public class GameObject : Disposable
    {
        public string Name
        {
            get => name;
            set => name = value;
        }

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

        public event EventHandler<EventArgs> EnabledChanged;

        public GameObject Owner
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

        public Scene Scene { get => scene; set => scene = value; }

        ///<inheritdoc cref="GameObject.GameObjectAdded"/>
        public event EventHandler<GameObject, GameObject> GameObjectAdded;

        ///<inheritdoc cref="GameObject.GameObjectRemoved"/>
        public event EventHandler<GameObject, GameObject> GameObjectRemoved;

        ///<inheritdoc cref="GameObject.ComponentAdded"/>
        public event EventHandler<GameObject, IComponent> ComponentAdded;

        ///<inheritdoc cref="GameObject.ComponentRemoved"/>
        public event EventHandler<GameObject, IComponent> ComponentRemoved;

        private EventedList<IComponent> components = new();
        private EventedList<GameObject> containers = new();
        private GameObject owner;
        private Scene scene;
        private bool enabled = true;
        private bool disposed = false;
        private string name = "";

        public GameObject()
        {

        }

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

        protected virtual void OnComponentAdded<T>(T component) where T : IComponent
        {

        }

        protected virtual void OnComponentRemoved<T>(T component) where T : IComponent
        {

        }

        protected virtual void OnGameObjectAdded<T>(T container) where T : GameObject
        {

        }

        protected virtual void OnGameObjectRemoved<T>(T container) where T : GameObject
        {

        }

        public T AddComponent<T>(T component) where T : IComponent
        {
            if(GetComponent(component.GetType()) != null)
                throw new ArgumentException(nameof(component) + $" type already exist!");

            component.Owner?.RemoveComponent(component);
            component.Owner = this;

            var requireIComponents = ProcessAddingComponent(component);
            // Add required components before main component

            components.Add(component);

            //foreach (var item in requireIComponents)
            //{
            //    AddIComponent(item); // Add required components AFTER main component
            //}

            OnComponentAdded(component);
            ComponentAdded?.Invoke(this, component);

            return component;
        }

        public T AddComponent<T>() where T : IComponent, new()
        {
            var component = GetComponent<T>();

            return component != null ? component : AddComponent<T>(Activator.CreateInstance<T>());
        }

        ///<inheritdoc cref="GameObject.RemoveComponent{T}(T)"/>
        public void RemoveComponent<T>(T component) where T : IComponent
        {
            if (!components.Contains(component))
                return;

            var requireIComponents = ProcessRemovingComponent(component);

            components.Remove(component);

            //foreach (var item in requireIComponents)
            //{
            //    RemoveIComponent(item);
            //}

            OnComponentRemoved(component);
            ComponentRemoved?.Invoke(this, component);

            component.Owner = null;
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            RemoveComponent<T>(Activator.CreateInstance<T>());
        }

        ///<inheritdoc cref="GameObject.AddGameObject{T}(T)"/>
        public T AddGameObject<T>(T container) where T : GameObject
        {
            container.Owner?.RemoveGameObject(container);
            container.Owner = this;

            containers.Add(container);

            OnGameObjectAdded(container);
            GameObjectAdded?.Invoke(this, container);

            return container;
        }

        ///<inheritdoc cref="GameObject.RemoveGameObject{T}(T)"/>
        public void RemoveGameObject<T>(T container) where T : GameObject
        {
            containers.Remove(container);

            OnGameObjectRemoved(container);
            GameObjectRemoved?.Invoke(this, container);

            container.Owner = null;
        }

        ///<inheritdoc cref="GameObject.GetComponent{T}"/>
        public T GetComponent<T>() where T : IComponent
        {
            return (T)components.Find(x => x is T);
        }

        ///<inheritdoc cref="GameObject.GetComponent(Type)"/>
        public IComponent GetComponent(Type type)
        {
            return components.Find(x => type.IsAssignableFrom(x.GetType()));
        }

        ///<inheritdoc cref="GameObject.GetComponents"/>
        public IEnumerable<IComponent> GetComponents()
        {
            return components.ToArray();
        }

        ///<inheritdoc cref="GameObject.GetComponents{T}()"/>
        public IEnumerable<T> GetComponents<T>() where T : IComponent
        {
            return GetComponents().Where(x => x is T).Cast<T>();
        }
        ///<inheritdoc cref="GameObject.GetComponents{T}()"/>
        public IEnumerable<IComponent> GetComponents(Func<IComponent, bool> predicate)
        {
            return components.Where(predicate);
        }

        ///<inheritdoc cref="GameObject.GetGameObject{T}"/>
        public T GetGameObject<T>() where T : GameObject
        {
            return (T)containers.Find(x => x is T);
        }

        ///<inheritdoc cref="GameObject.GetGameObjects"/>
        public IEnumerable<GameObject> GetGameObjects()
        {
            return containers.ToArray();
        }

        ///<inheritdoc cref="GameObject.FindGameObject{T}()"/>
        public T FindGameObject<T>() where T : GameObject
        {
            if (Owner == null)
            {
                if (typeof(T) == this.GetType())
                {
                    return (T)((GameObject)this);
                }

                return FindGameObjectRecursiveDown<T>(this, (x) => true);
            }
            return Owner.FindGameObject<T>();
        }

        ///<inheritdoc cref="GameObject.FindGameObject{T}(Predicate{T})"/>
        public T FindGameObject<T>(Predicate<T> predicate) where T : GameObject
        {
            if (Owner == null)
                return FindGameObjectRecursiveDown<T>(this, predicate);

            return Owner.FindGameObject<T>(predicate);
        }

        ///<inheritdoc cref="GameObject.FindGameObjects{T}()"/>
        public IEnumerable<T> FindGameObjects<T>() where T : GameObject
        {
            if (Owner == null)
                return FindGameObjectsRecursiveDown<T>(this, (x) => true);

            return Owner.FindGameObjects<T>();
        }

        ///<inheritdoc cref="GameObject.FindGameObjects(Predicate{GameObject})"/>
        public IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate)
        {
            if (Owner == null)
                return FindGameObjectsRecursiveDown<GameObject>(this, predicate);

            return Owner.FindGameObjects<GameObject>();
        }

        ///<inheritdoc cref="GameObject.FindComponent{T}()"/>
        public T FindComponent<T>() where T : IComponent
        {
            if (Owner == null)
                return FindComponentRecursiveDown<T>(this, (x) => true);

            return Owner.FindComponent<T>();
        }

        ///<inheritdoc cref="GameObject.FindGameObject{T}(Predicate{T})"/>
        public T FindIComponent<T>(Predicate<T> predicate) where T : IComponent
        {
            if (Owner == null)
                return FindComponentRecursiveDown<T>(this, predicate);

            return Owner.FindComponent<T>();
        }

        ///<inheritdoc cref="GameObject.FindComponents{T}()"/>
        public IEnumerable<T> FindComponents<T>() where T : IComponent
        {
            if (Owner == null)
                return FindComponentsRecursiveDown<T>(this, (x) => true);

            return Owner.FindComponents<T>();
        }

        ///<inheritdoc cref="GameObject.FindComponents{T}()"/>
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

            return $"GameObject: {name}";
        }

        /// <inheritdoc cref="GameObject.RegisterIComponent{T}(T)"/>
        protected internal virtual void RegistrationComponent<T>(T component) where T : IComponent
        {
            Owner.RegistrationComponent(component);
        }

        /// <inheritdoc cref="GameObject.UnregisterIComponent{T}(T)"/>
        protected internal virtual void DeregistrationComponent<T>(T component) where T : IComponent
        {
            Owner.DeregistrationComponent(component);
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
        private void ShutdownGameObject(EventedList<GameObject> sender, GameObject  container)
        {
            container.Shutdown();
        }
        private void AwakeGameObject(EventedList<GameObject> sender, GameObject container)
        {
            container.Awake();
        }
        private void ShutdownComponent(EventedList<IComponent> sender, IComponent component)
        {
            DeregistrationComponent<IComponent>(component);

            component.Shutdown();

        }

        private void StartupComponent(EventedList<IComponent> sender, IComponent component)
        {
            component.Awake();

            RegistrationComponent<IComponent>(component);
        }

        private T FindGameObjectRecursiveDown<T>(GameObject container, Predicate<T> predicate) where T : GameObject
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
        private IEnumerable<T> FindGameObjectsRecursiveDown<T>(GameObject container, Predicate<T> predicate) where T : GameObject
        {
            var containers = container.GetGameObjects();
            var findeds = containers.Where(x => x is T t && predicate.Invoke(t)).Cast<T>().ToList();

            foreach (var item in containers)
            {
                findeds.AddRange(FindGameObjectsRecursiveDown<T>(item, predicate));
            }

            return findeds;
        }
        private T FindComponentRecursiveDown<T>(GameObject container, Predicate<T> predicate) where T : IComponent
        {
            var finded = (T)container.GetComponents().FirstOrDefault(x => x is T t && predicate.Invoke(t));

            if (finded != null)
                return finded;

            var containers = container.GetGameObjects();

            foreach (var item in containers)
            {
                finded = FindComponentRecursiveDown<T>(item, predicate);

                if (finded != null)
                    return finded;
            }

            return finded;
        }
        private IEnumerable<T> FindComponentsRecursiveDown<T>(GameObject container, Predicate<T> predicate) where T : IComponent
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

        private IEnumerable<IComponent> ProcessAddingComponent(IComponent component)
        {
            var attr = component.GetType().GetCustomAttributes(typeof(RequireComponentAttribute), true);

            List<IComponent> addAfterList = new List<IComponent>();

            foreach (RequireComponentAttribute item in attr)
            {
                if (item?.Type == null)
                    continue; // Empty attribute, do nothing

                if (GetComponent(item.Type) != null)
                    continue;

                if (item.Type == component.GetType())
                    continue; // An attempt to add a required component of the same type

                IComponent required = Activator.CreateInstance(item.Type) as IComponent
                    ?? throw new Exception($"The require component {item.Type} is not inherit from a IComponent!");

                if (item.AddingOrder == AddingOrderType.Before)
                    component.Owner.AddComponent(required); // Add now
                else addAfterList.Add(required); // This adding later
            }

            return addAfterList;
        }

        private IEnumerable<IComponent> ProcessRemovingComponent(IComponent component)
        {
            var attr = component.GetType().GetCustomAttributes(typeof(RequireComponentAttribute), true);
            List<IComponent> removeAfterList = new List<IComponent>();

            foreach (RequireComponentAttribute item in attr)
            {
                if (item?.Type == null)
                    continue; // Empty attribute, do nothing

                if (!item.Remove)
                    continue;

                IComponent requier = component.Owner.GetComponent(item.Type);

                if (requier == null)
                    continue;

                if (item.AddingOrder == AddingOrderType.After)
                    component.Owner.RemoveComponent(requier);
                else removeAfterList.Add(requier);
            }

            return removeAfterList;
        }

    }
}
