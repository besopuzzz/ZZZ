using ZZZ.Framework.Components;
using static ZZZ.Framework.SignalMessenger;

namespace ZZZ.Framework
{
    public abstract partial class GameContainer : Disposable
    {
        public string Name
        {
            get => name;
            set => name = value;
        }

        [ContentSerializerIgnore]
        public bool Enabled
        {
            get
            {
                return enabled & (owner == null || owner.Enabled);
            }

            set
            {
                if (Enabled == value & enabled == value)
                    return;

                enabled = value;
            }
        }

        [ContentSerializerIgnore]
        public GameObject Owner
        {
            get => owner as GameObject;
        }

        [ContentSerializer(ElementName = "Components")]
        private EventedList<Component> components = new();

        [ContentSerializer(ElementName = "Childs")]
        private EventedList<GameContainer> containers = new();

        [ContentSerializer(ElementName = "Enabled")]
        private bool enabled = true;

        private GameContainer owner;

        private string name = "";

        protected internal void Awake()
        {
            var comps = components.ToList();
            var conts = containers.ToList();

            containers.ItemAdded += AwakeGameObject;
            containers.ItemRemoved += ShutdownGameObject;

            components.ItemAdded += SendToParentsAdded;
            components.ItemRemoved += SendToParentsRemoved;

            foreach (var item in comps)
                SendToParentsAdded(components, item);

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
                SendToParentsRemoved(components, item);

            containers.ItemAdded -= AwakeGameObject;
            containers.ItemRemoved -= ShutdownGameObject;

            components.ItemAdded -= SendToParentsAdded;
            components.ItemRemoved -= SendToParentsRemoved;
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

        protected virtual void OnSceneComponentAdded<T>(T component)
            where T : Component
        {
            owner.OnSceneComponentAdded(component);
        }

        protected virtual void OnSceneComponentShutdowned<T>(T component)
            where T : Component
        {
            owner.OnSceneComponentShutdowned(component);
        }

        protected virtual void OnComponentAdding<T>(T component)
            where T : Component
        {
        }

        protected virtual void OnComponentAdded<T>(T component)
            where T : Component
        {

        }

        protected virtual void OnComponentRemoving<T>(T component)
            where T : Component
        {
           
        }

        protected virtual void OnComponentRemoved<T>(T component)
            where T : Component
        {
        }

        protected virtual void OnGameObjectAdding<T>(T container)
            where T : GameObject
        {
        }

        protected virtual void OnGameObjectAdded<T>(T container)
            where T : GameObject
        {

        }

        protected virtual void OnGameObjectRemoving<T>(T container)
            where T : GameObject
        {

        }

        protected virtual void OnGameObjectRemoved<T>(T container)
            where T : GameObject
        {
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

        internal void SendSignalToParents<T>(object data, SignalFunction<T> action)
        {
            if (owner != null)
            {
                if (owner.SendSignal(data, action))
                    return;

                owner.SendSignalToParents(data, action);
            }
        }

        internal void SendSignalToChilds<T>(object data, SignalFunction<T> action)
        {
            foreach (var child in containers)
            {
                if (!child.SendSignal(data, action))
                    child.SendSignalToChilds(data, action);
            }
        }

        internal bool SendSignal<T>(object data, SignalFunction<T> action)
        {
            foreach (var component in GetComponents<T>())
            {
                if (action.Invoke(component, data))
                    return true;
            }

            return false;
        }

        private void ShutdownGameObject(EventedList<GameContainer> sender, GameContainer container)
        {
            container.Shutdown();
        }

        private void AwakeGameObject(EventedList<GameContainer> sender, GameContainer container)
        {
            container.Awake();
        }

        private void SendToParentsRemoved(EventedList<Component> sender, Component component)
        {
            OnSceneComponentShutdowned(component);
        }

        private void SendToParentsAdded(EventedList<Component> sender, Component component)
        {
            OnSceneComponentAdded(component);
        }
    }
}
