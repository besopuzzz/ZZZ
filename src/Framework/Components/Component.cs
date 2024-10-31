using System.ComponentModel;
using ZZZ.Framework.Components.Transforming;

namespace ZZZ.Framework.Components
{
    /// <summary>
    /// Представляет базовый компонент. Только для наследования.
    /// </summary>
    [RequiredComponent(typeof(Transformer))]
    public abstract class Component : Disposable, IComponent
    {
        /// <inheritdoc cref="IComponent.Enabled"/>
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

        [ContentSerializerIgnore]
        /// <inheritdoc cref="IComponent.Owner"/>
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

        /// <inheritdoc cref="IComponent.EnabledChanged"/>
        public event EventHandler<EventArgs> EnabledChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private GameObject owner;
        private bool enabled = true;

        /// <inheritdoc cref="IComponent.Awake"/>
        protected virtual void Awake()
        {

        }

        /// <inheritdoc cref="IComponent.Shutdown"/>
        protected virtual void Shutdown()
        {

        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Owner"/> было изменено.
        /// </summary>
        protected virtual void OnOwnerChanged()
        {

        }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Enabled"/> было изменено.
        /// </summary>
        protected virtual void OnEnabledChanged()
        {

        }

        /// <inheritdoc cref="GameObject.AddGameObject"/>
        protected GameObject AddGameObject(GameObject child)  => Owner.AddGameObject(child);

        /// <inheritdoc cref="GameObject.RemoveGameObject"/>
        protected void RemoveGameObject<T>(T child) where T : GameObject => Owner.RemoveGameObject(child);

        /// <inheritdoc cref="GameObject.AddComponent"/>
        protected Component AddComponent(Type type) => Owner.AddComponent(type);

        /// <inheritdoc cref="GameObject.AddComponent"/>
        protected T AddComponent<T>() where T : Component, new() => Owner.AddComponent<T>();

        /// <inheritdoc cref="GameObject.RemoveComponent"/>
        protected void RemoveComponent<T>(T component) where T : Component => Owner.RemoveComponent(component);

        /// <inheritdoc cref="GameObject.RemoveComponent"/>
        protected void RemoveComponent<T>() where T : Component => Owner.RemoveComponent<T>();

        /// <inheritdoc cref="GameObject.GetComponent{T}()"/>
        protected T GetComponent<T>() where T : IComponent => Owner.GetComponent<T>();

        /// <inheritdoc cref="GameObject.GetComponent"/>
        protected object GetComponent(Type type) => Owner.GetComponent(type);

        /// <inheritdoc cref="GameObject.GetComponents"/>
        protected IEnumerable<IComponent> GetComponents() => Owner.GetComponents();

        /// <inheritdoc cref="GameObject.GetComponents"/>
        protected IEnumerable<IComponent> GetComponents(Func<IComponent, bool> func) => Owner.GetComponents(func);

        /// <inheritdoc cref="GameObject.GetComponents{T}()"/>
        protected IEnumerable<T> GetComponents<T>() where T : IComponent => Owner.GetComponents<T>();

        /// <inheritdoc cref="GameObject.GetGameObjects"/>
        protected IEnumerable<GameObject> GetGameObjects() => Owner.GetGameObjects();

        /// <inheritdoc cref="GameObject.FindGameObjects"/>
        protected IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate) => Owner.FindGameObjects(predicate);

        /// <inheritdoc cref="GameObject.FindComponent{T}()"/>
        protected T FindComponent<T>() where T : IComponent => Owner.FindComponent<T>();

        /// <inheritdoc cref="GameObject.FindComponents"/>
        protected IEnumerable<T> FindComponents<T>() where T : IComponent => Owner.FindComponents<T>();

        private void LocalEnabledChanged(object sender, EventArgs eventArgs)
        {
            OnEnabledChanged();
            EnabledChanged?.Invoke(sender, EventArgs.Empty);
        }

        internal void InternalAwake()
        {
            Awake();
        }

        internal void InternalShutdown()
        {
            Shutdown();
        }
    }
}
