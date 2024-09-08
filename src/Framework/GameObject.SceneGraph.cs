namespace ZZZ.Framework
{
    public partial class GameObject
    {
        public bool IsOwner(GameObject gameObject)
        {
            if (Owner == null)
                return false;

            if(Owner == gameObject)
                return true;

            return Owner.IsOwner(gameObject);
        }

        public T AddComponent<T>(T component) where T : IComponent
        {
            if (GetComponent(component.GetType()) != null)
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
            var component = GetComponent<T>();

            if (component != null)
                RemoveComponent(component);
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

    }
}
