using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core
{
    public abstract class EntityComponent<TEntity, TEntityComponent, TComponent> : Disposable
        where TEntity : Entity<TEntity, TEntityComponent, TComponent>
        where TEntityComponent : EntityComponent<TEntity, TEntityComponent, TComponent>
        where TComponent : IComponent
    {
        public TComponent Component => component;
        public TEntity Owner => owner;
        public bool Enabled => owner.Enabled && component.Enabled;

        private TComponent component;
        private TEntity owner;

        public EntityComponent(TComponent component)
        {
            this.component = component;
        }

        internal void Initialize(TEntity entity)
        {
            owner = entity;
            Initialize();
        }

        protected virtual void Initialize()
        {

        }

        protected override void Dispose(bool disposing)
        {
            component = default;
            owner = default;

            base.Dispose(disposing);
        }

        public virtual void Foreach(Func<TEntityComponent> func)
        {
        }
    }
}
