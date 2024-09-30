namespace ZZZ.Framework.Core
{
    public interface ISystem<TEntity, TEntityComponent, TComponent>
        where TEntity : Entity<TEntity, TEntityComponent, TComponent>
        where TEntityComponent : EntityComponent<TEntity, TEntityComponent, TComponent>
        where TComponent : IComponent
    {
        TEntity CreateEntity(Entity<TEntity, TEntityComponent, TComponent> owner);
        TEntityComponent CreateEntityComponent(Entity<TEntity, TEntityComponent, TComponent> owner, TComponent component);
    }
}
