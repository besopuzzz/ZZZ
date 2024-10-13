using ZZZ.Framework.Components.Transforming;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый контейнер.
    /// </summary>
    public partial class GameObject : Disposable
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

        /// <summary>
        /// Distance from the Scene.
        /// </summary>
        public int DistanceOfScene => distance;


        /// <summary>
        /// Distance from the Owner.
        /// </summary>
        public int DistanceOfOwner => ownerDistance;



        [ContentSerializerIgnore]
        public GameObject Owner
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

        [ContentSerializerIgnore]
        public Scene Scene => GetCurrentScene();

        ///<inheritdoc cref="GameObject.GameObjectAdded"/>
        public event EventHandler<GameObject, GameObject> GameObjectAdded;

        ///<inheritdoc cref="GameObject.GameObjectRemoved"/>
        public event EventHandler<GameObject, GameObject> GameObjectRemoved;

        ///<inheritdoc cref="GameObject.ComponentAdded"/>
        public event EventHandler<GameObject, IComponent> ComponentAdded;

        ///<inheritdoc cref="GameObject.ComponentRemoved"/>
        public event EventHandler<GameObject, IComponent> ComponentRemoved;

        [ContentSerializer(ElementName = "Components")]
        private EventedList<IComponent> components = new();

        [ContentSerializer(ElementName = "Childs")]
        private EventedList<GameObject> containers = new();

        private GameObject owner;
        private bool enabled = true;
        private bool disposed = false;
        private string name = "";
        private int distance = -1;
        private int ownerDistance = -1;

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

        protected internal virtual Scene GetCurrentScene()
        {
            return Owner?.GetCurrentScene();
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

            container.distance = -1;
            container.ownerDistance = -1;
        }

        private void AwakeGameObject(EventedList<GameObject> sender, GameObject container)
        {
            if (this == Scene)
                container.distance = 0;
            else container.distance = distance + 1;

            container.ownerDistance = sender.IndexOf(container);

            container.Awake();
        }

        private void ShutdownComponent(EventedList<IComponent> sender, IComponent component)
        {
            component.Shutdown();

            DeregistrationComponent<IComponent>(component);
        }

        private void StartupComponent(EventedList<IComponent> sender, IComponent component)
        {
            RegistrationComponent<IComponent>(component);

            component.Awake();
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
