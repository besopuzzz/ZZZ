using System.ComponentModel;
using ZZZ.Framework.Exceptions;

namespace ZZZ.Framework.Components
{
    /// <summary>
    /// Представляет базовый компонент. Только для наследования.
    /// </summary>
    public abstract class Component : Disposable
    {
        /// <inheritdoc cref="IComponent.Enabled"/>
        [ContentSerializerIgnore]
        public virtual bool Enabled
        {
            get
            {
                return enabled & (FactOwner == null || FactOwner.Enabled);
            }

            set
            {
                if (/*Enabled == value & */enabled == value)
                    return;

                enabled = value;
            }
        }

        [ContentSerializerIgnore]
        /// <inheritdoc cref="IComponent.Owner"/>
        public GameObject Owner => FactOwner as GameObject;

        public bool Awaked => awaked;

        [ContentSerializerIgnore]
        internal GameContainer FactOwner { get; set; }

        [ContentSerializer(ElementName = "Enabled")]
        private bool enabled = true;

        private bool awaked;

        /// <inheritdoc cref="IComponent.Awake"/>
        protected virtual void Awake()
        {
            awaked = true;
        }

        /// <inheritdoc cref="IComponent.Shutdown"/>
        protected virtual void Shutdown()
        {
            awaked = false;
        }

        /// <inheritdoc cref="GameObject.AddGameObject"/>
        protected GameObject AddGameObject(GameObject child)
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner?.AddGameObject(child);
        }

        /// <inheritdoc cref="GameObject.RemoveGameObject"/>
        protected void RemoveGameObject<T>(T child) where T : GameObject
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            FactOwner?.RemoveGameObject(child);
        }

        /// <inheritdoc cref="GameObject.AddComponent"/>
        protected Component AddComponent(Type type)
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner?.AddComponent(type);
        }

        /// <inheritdoc cref="GameObject.AddComponent"/>
        protected T AddComponent<T>() where T : Component, new()
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner.AddComponent<T>();
        }

        /// <inheritdoc cref="GameObject.RemoveComponent"/>
        protected void RemoveComponent<T>(T component) where T : Component
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            FactOwner.RemoveComponent(component);
        }

        /// <inheritdoc cref="GameObject.RemoveComponent"/>
        protected void RemoveComponent<T>() where T : Component
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            FactOwner?.RemoveComponent<T>();
        }

        /// <inheritdoc cref="GameObject.GetComponent{T}()"/>
        protected T GetComponent<T>()
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner.GetComponent<T>();
        }

        /// <inheritdoc cref="GameObject.GetComponent"/>
        protected object GetComponent(Type type)
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner?.GetComponent(type);
        }

        /// <inheritdoc cref="GameObject.GetComponents"/>
        protected IEnumerable<T> GetComponents<T>()
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner?.GetComponents<T>();
        }
        
        /// <inheritdoc cref="GameObject.GetGameObjects"/>
        protected IEnumerable<GameObject> GetGameObjects()
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner?.GetGameObjects();
        }
        
        /// <inheritdoc cref="GameObject.FindGameObjects"/>
        protected IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate)
        {
            InvalidComponentOperationException.ThrowIfOwnerNull(this);

            return FactOwner?.FindGameObjects(predicate);
        }
        
        public void InternalAwake()
        {
            Awake();
        }

        internal void InternalShutdown()
        {
            Shutdown();
        }
    }
}
