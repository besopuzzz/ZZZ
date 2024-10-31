using ZZZ.Framework.Attributes;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Transforming;

namespace ZZZ.Framework
{
    public partial class Container
    {
        private static RequiredComponentFactory requiredComponentFactory = new RequiredComponentFactory();
        private static WaitComponentFactory waitComponentFactory = new WaitComponentFactory();
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
            var component = GetComponent(type) as Component;

            if (component != null)
                return component;

            component = Activator.CreateInstance(type, true) as Component;

            OnComponentAdding(component);

            components.Add(component);

            OnComponentAdded(component);

            return component;
        }

        ///<inheritdoc cref="GameObject.RemoveComponent{T}(T)"/>
        public void RemoveComponent<T>(T component) where T : Component
        {
            OnComponentRemoving(component);

            components.Remove(component);

            OnComponentRemoved(component);
        }

        public void RemoveComponent<T>() where T : Component
        {
            var component = GetComponent<T>();

            if (component != null)
                RemoveComponent(component);
        }

        ///<inheritdoc cref="GameObject.AddGameObject{T}(T)"/>
        public GameObject AddGameObject(GameObject gameObject)
        {
            OnGameObjectAdding(gameObject);

            containers.Add(gameObject);

            OnGameObjectAdded(gameObject);

            return gameObject;
        }

        ///<inheritdoc cref="GameObject.AddGameObject{T}(T)"/>
        public GameObject AddGameObject(Transform2D transform2D)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<Transformer>().Local = transform2D;

            return AddGameObject(gameObject);
        }

        ///<inheritdoc cref="GameObject.RemoveGameObject{T}(T)"/>
        public void RemoveGameObject(GameObject gameObject)
        {
            OnGameObjectRemoving(gameObject);

            containers.Remove(gameObject);

            OnGameObjectRemoved(gameObject);
        }

        ///<inheritdoc cref="GameObject.GetComponent{T}"/>
        public T GetComponent<T>() where T : IComponent
        {
            return (T)GetComponent(typeof(T));
        }

        ///<inheritdoc cref="GameObject.GetComponent(Type)"/>
        public IComponent GetComponent(Type type)
        {
            return components.Find(x => type.IsAssignableFrom(x.GetType()));
        }

        ///<inheritdoc cref="GameObject.GetComponents"/>
        public IEnumerable<Component> GetComponents()
        {
            return components.Cast<Component>();
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

        ///<inheritdoc cref="GameObject.GetGameObjects"/>
        public IEnumerable<GameObject> GetGameObjects()
        {
            return containers.ToArray();
        }

        ///<inheritdoc cref="GameObject.FindGameObjects(Predicate{GameObject})"/>
        public IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate)
        {
            if (Owner == null)
                return FindGameObjectsRecursiveDown<GameObject>(this, predicate);

            return Owner.FindGameObjects(predicate);
        }

        ///<inheritdoc cref="GameObject.FindComponent{T}()"/>
        public T FindComponent<T>() where T : IComponent
        {
            if (Owner == null)
                return FindComponentRecursiveDown<T>(this, (x) => true);

            return Owner.FindComponent<T>();
        }

        ///<inheritdoc cref="GameObject.FindGameObject{T}(Predicate{T})"/>
        public T FindComponent<T>(Predicate<T> predicate) where T : IComponent
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
    }
}
