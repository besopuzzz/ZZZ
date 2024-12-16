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
        internal GameContainer FactOwner
        {
            get => owner;

            set
            {
                if (owner == value)
                    return;

                owner = value;

                if (owner != null)
                    OnCreated();
                else
                {
                    OnDestroy();

                    ((IDisposable)this).Dispose();
                }
            }
        }

        [ContentSerializer(ElementName = "Enabled")]
        private bool enabled = true;

        private GameContainer owner;
        private bool awaked;

        protected virtual void Awake()
        {
            awaked = true;
        }

        protected virtual void Shutdown()
        {
            awaked = false;
        }

        /// <summary>
        /// Вызывает событие, когда компонент создан и добавлен в родительский контейнер.
        /// Метод гарантирует, что обязательные компоненты уже добавлены в родительский контейнер.
        /// </summary>
        protected virtual void OnCreated()
        {

        }

        protected virtual void OnDestroy()
        {

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
