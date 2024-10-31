using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core
{
    public class Entity<TEntity, TEntityComponent, TComponent> : Disposable
        where TEntity : Entity<TEntity, TEntityComponent, TComponent>
        where TEntityComponent : EntityComponent<TEntity, TEntityComponent, TComponent>
        where TComponent : IComponent
    {
        public bool Enabled => gameObject.Enabled;
        public EventedList<TEntityComponent> EntityComponents => components;
        public EventedList<TEntity> Childs => childs;
        public GameObject Owner => gameObject;
        public TEntity OwnerEntity => ownerEntity;

        private ISystem<TEntity, TEntityComponent, TComponent> system;
        private GameObject gameObject;
        private TEntity ownerEntity;
        private readonly EventedList<TEntityComponent> components = new EventedList<TEntityComponent>();
        private readonly EventedList<TEntity> childs = new EventedList<TEntity>();

        internal void Initialize(GameObject gameObject, ISystem<TEntity, TEntityComponent, TComponent> system)
        {
            this.gameObject = gameObject;
            this.system = system;

            foreach (var child in gameObject.GetGameObjects())
            {
                GameObject_GameObjectAdded(gameObject, child);
            }

            gameObject.GameObjectAdded += GameObject_GameObjectAdded;
            gameObject.GameObjectRemoved += GameObject_GameObjectRemoved;

            foreach (var component in gameObject.GetComponents<TComponent>())
            {
                GameObject_ComponentAdded(gameObject, component);
            }

            gameObject.ComponentAdded += GameObject_ComponentAdded;
            gameObject.ComponentRemoved += GameObject_ComponentRemoved;

            Initialize();
        }


        private void GameObject_GameObjectRemoved(GameObject sender, GameObject e)
        {
            var child = childs.Find(x => x.Owner == e);

            if (child != null)
            {
                childs.Remove(child);

                EntityRemoved(child);

                child.ownerEntity = null;
            }
        }

        private void GameObject_GameObjectAdded(GameObject sender, GameObject e)
        {
            var child = system.CreateEntity(this);

            child.ownerEntity = (TEntity)this;
            child.Initialize(e, system);

            childs.Add(child);

            EntityAdded(child);
        }

        private void GameObject_ComponentRemoved(GameObject sender, IComponent e)
        {
            if (e is not TComponent component)
                return;

            var entity = components.Find(x => x.Component.Equals(component));

            components.Remove(entity);

            ComponentEntityRemoved(entity);
        }

        private void GameObject_ComponentAdded(GameObject sender, IComponent e)
        {
            if (e is not TComponent component)
                return;

            var entity = system.CreateEntityComponent(this, component);

            if (entity == null)
                return;

            entity.Initialize((TEntity)this);

            components.Add(entity);

            ComponentEntityAdded(entity);
        }

        protected virtual void Initialize()
        {

        }

        protected virtual void EntityAdded(TEntity entity) { }
        protected virtual void EntityRemoved(TEntity entity) { }

        protected virtual void ComponentEntityAdded(TEntityComponent entityComponent)
        {

        }
        protected virtual void ComponentEntityRemoved(TEntityComponent entityComponent)
        {

        }

        public virtual void ForEveryComponent(Action<TEntityComponent> action)
        {
            components.ForEach(action);

            childs.ForEach((e)=> e.ForEveryComponent(action));
        }

        public virtual void ForEveryChild(Action<TEntity> action)
        {
            action.Invoke(this as TEntity);

            childs.ForEach((e) => e.ForEveryChild(action));
        }
    }
}
