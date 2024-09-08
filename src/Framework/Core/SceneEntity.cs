using System.ComponentModel;

namespace ZZZ.Framework.Core
{
    internal sealed class SceneEntity<TComponent, TEntity> : BaseEntity<TComponent, TEntity>
        where TComponent : IComponent
        where TEntity : BaseEntity<TComponent, TEntity>
    {
        public GameObject GameObject => gameObject;

        public override bool Enabled => gameObject.Enabled;

        public IReadOnlyList<SceneEntity<TComponent, TEntity>> Childs => childs;

        public TEntity MainEntity => componentEntity;

        private GameObject gameObject;
        private readonly List<SceneEntity<TComponent, TEntity>> childs = new List<SceneEntity<TComponent, TEntity>>();
        private TEntity componentEntity;
        private ISystem<TComponent, TEntity> system;

        internal void Initialize(GameObject gameObject, ISystem<TComponent, TEntity> system)
        {
            this.gameObject = gameObject;
            this.system = system;

            foreach (var child in gameObject.GetGameObjects())
            {
                GameObject_GameObjectAdded(gameObject, child);
            }

            gameObject.GameObjectAdded += GameObject_GameObjectAdded;
            gameObject.GameObjectRemoved += GameObject_GameObjectRemoved;

            var component = gameObject.GetComponent<TComponent>();

            if (component == null)
            {
                gameObject.ComponentAdded += GameObject_ComponentAdded;
            }
            else
            {
                var entity = system.Process(component);

                if (entity == null)
                    return;

                componentEntity = entity as TEntity;

                componentEntity.Initialize(this);

                gameObject.ComponentRemoved += GameObject_ComponentRemoved;
            }
        }

        private void GameObject_GameObjectRemoved(GameObject sender, GameObject e)
        {
            var child = childs.Find(x => x.GameObject == e);

            if (child != null)
            {
                childs.Remove(child);

                child.Dispose(true);
            }
        }

        private void GameObject_GameObjectAdded(GameObject sender, GameObject e)
        {
            var child = new SceneEntity<TComponent, TEntity>();
            child.Initialize(e, system);

            childs.Add(child);
        }

        private void GameObject_ComponentRemoved(GameObject sender, IComponent e)
        {
            if (e is not TComponent component)
                return;

            ((IDisposable)componentEntity).Dispose();
            componentEntity = null;

            sender.ComponentRemoved -= GameObject_ComponentRemoved;
            sender.ComponentAdded += GameObject_ComponentAdded;
        }

        private void GameObject_ComponentAdded(GameObject sender, IComponent e)
        {
            if (e is not TComponent component)
                return;

            var entity = system.Process(component);

            if (entity == null)
                return;

            componentEntity = entity as TEntity;
            componentEntity.Initialize(this);

            sender.ComponentAdded -= GameObject_ComponentAdded;
            sender.ComponentRemoved += GameObject_ComponentRemoved;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (componentEntity == null)
                {
                    GameObject.ComponentAdded -= GameObject_ComponentAdded;
                }
                else
                {
                    GameObject.ComponentRemoved -= GameObject_ComponentRemoved;
                    ((IDisposable)componentEntity).Dispose();
                }

                foreach (var child in childs)
                {
                    child.Dispose(true);
                }
            }

            componentEntity = null;
            gameObject = null;
            childs.Clear();

            base.Dispose(disposing);
        }

        internal override IEnumerable<TEntity> GetEntitiesInternal()
        {
            if (componentEntity != null)
            {
                yield return componentEntity;

                foreach (var item in componentEntity.GetEntitiesInternal())
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var child in childs)
                {
                    foreach (var child3 in child.GetEntitiesInternal())
                    {
                        yield return child3;
                    }
                }
            }
        }


    }
}
