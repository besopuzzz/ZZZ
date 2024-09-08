namespace ZZZ.Framework.Core
{
    /// <inheritdoc cref="BaseEntity{TComponent, TEntity}"/>
    public class Entity<TComponent, TEntity> : BaseEntity<TComponent, TEntity>
        where TComponent : IComponent
        where TEntity : BaseEntity<TComponent, TEntity>
    {
        /// <summary>
        /// Ссылка на основной компонент.
        /// </summary>
        public TComponent Component => component;

        /// <summary>
        /// Возвращает <c>true</c>, если <typeparamref name="TComponent"/> и его владелец <see cref="IComponent.Owner"/> включен.
        /// Иначе <c>false</c>.
        /// </summary>
        public override bool Enabled => base.Enabled & Component.Enabled;

        private TComponent component;


        public Entity(TComponent component) : base()
        {
            this.component = component;
        }
    }
}
