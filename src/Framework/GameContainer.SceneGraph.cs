using System.Reflection;
using ZZZ.Framework.Components;
using ZZZ.Framework.Exceptions;

namespace ZZZ.Framework
{
    public partial class GameContainer
    {
        public bool IsParent(GameObject gameObject)
        {
            if (Owner == null)
                return false;

            if(Owner == gameObject)
                return true;

            return Owner.IsParent(gameObject);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            return AddComponent(typeof(T)) as T;
        }

        public Component AddComponent(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            InvalidComponentOperationException.ThrowIfIsNotComponentOrAbstract(type);

            var component = GetComponent(type);

            if (component != null)
                return component;

            component = Activator.CreateInstance(type, true) as Component;

            OnComponentAdding(component);

            var attributes = type.GetCustomAttributes<RequiredComponentAttribute>();

            foreach (var attribute in attributes)
            {
                var other = AddComponent(attribute.Type);

                attribute.AutoReference?.Connect(component, other);
            }

            component.FactOwner = this;

            components.Add(component);

            OnComponentAdded(component);

            return component;
        }

        ///<inheritdoc cref="GameObject.RemoveComponent{T}(T)"/>
        public void RemoveComponent<T>(T component) where T : Component
        {
            RemoveComponent<T>();
        }

        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            var component = GetComponent(type) as Component;

            if (component == null)
                return;

            OnComponentRemoving(component);

            foreach (var attribute in type.GetCustomAttributes<RequiredComponentAttribute>())
            {
                var other = GetComponent(attribute.Type);

                attribute?.AutoReference.Disconnect(component, other);

                if (attribute.Remove)
                    RemoveComponent(attribute.Type);
            }

            components.Remove(component);

            OnComponentRemoved(component);

            component.FactOwner = null;
        }

        internal void AddGameObject(GameContainer container)
        {
            container.owner?.RemoveGameObject(container);

            container.owner = this;

            containers.Add(container);
        }

        ///<inheritdoc cref="GameObject.AddGameObject{T}(T)"/>
        public GameObject AddGameObject(GameObject gameObject)
        {
            gameObject.owner?.RemoveGameObject(gameObject);

            gameObject.owner = this;

            OnGameObjectAdding(gameObject);

            containers.Add(gameObject);

            OnGameObjectAdded(gameObject);

            return gameObject;
        }

        ///<inheritdoc cref="GameObject.RemoveGameObject{T}(T)"/>
        public void RemoveGameObject(GameObject gameObject)
        {
            OnGameObjectRemoving(gameObject);

            containers.Remove(gameObject);

            OnGameObjectRemoved(gameObject);

            gameObject.owner = null;
        }

        internal void RemoveGameObject(GameContainer container)
        {
            containers.Remove(container);

            container.owner = null;
        }

        ///<inheritdoc cref="GameObject.GetComponent{T}"/>
        public T GetComponent<T>()
        {
            return (T)(object)GetComponent(typeof(T));
        }

        ///<inheritdoc cref="GameObject.GetComponent(Type)"/>
        public Component GetComponent(Type type)
        {
            if (type == typeof(Component))
                throw new ArgumentException($"You cannot get a component with the {typeof(Component)} type. Try to specify the type.");

            return components.Find(x => type.IsAssignableFrom(x.GetType()));
        }

        ///<inheritdoc cref="GameObject.GetComponents{T}()"/>
        public IEnumerable<T> GetComponents<T>()
        {
            var type = typeof(T);

            if (type == typeof(Component))
                throw new ArgumentException($"You cannot get a component with the {typeof(Component)} type. Try to specify the type.");

            return components.Where(x => x is T).Cast<T>();
        }

        ///<inheritdoc cref="GameObject.GetGameObjects"/>
        public virtual IEnumerable<GameObject> GetGameObjects()
        {
            return containers.Where(x => x is GameObject).Cast<GameObject>();
        }

        ///<inheritdoc cref="GameObject.FindGameObjects(Predicate{GameObject})"/>
        public virtual IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate)
        {
            if (Owner == null)
                return FindGameObjectsRecursiveDown<GameObject>(this as GameObject, predicate);

            return Owner.FindGameObjects(predicate);
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
    }
}
