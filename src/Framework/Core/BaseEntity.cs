namespace ZZZ.Framework.Core
{
    /// <summary>
    /// Предоставляет базовую сущность для обработки в системе <see cref="System{TComponent, TEntity}"/>.
    /// </summary>
    /// <typeparam name="TComponent">Тип компонента, на основе которых будут создаваться сущности <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Тип сущности, которые будут обрабатывать указанный тип <typeparamref name="TComponent"/>.</typeparam>
    public abstract class BaseEntity<TComponent, TEntity> : Disposable
        where TComponent : IComponent
        where TEntity : BaseEntity<TComponent, TEntity>
    {
        /// <summary>
        /// Возвращает <c>true</c>, если владелец компонента <see cref="IComponent.Owner"/> включен. Иначе <c>false</c>.
        /// </summary>
        public virtual bool Enabled => baseEntity.Enabled;

        private SceneEntity<TComponent, TEntity> baseEntity;

        internal BaseEntity()
        {

        }

        /// <summary>
        /// Возвращает перечислитель дочерних сущностей относительно положения основного компонента в иерархии сцены <see cref="Scene"/>.
        /// </summary>
        /// <remarks>Данный метод использует система <see cref="ZZZ.Framework.Core.System{TComponent, TEntity}.Entities"/>, в которой используется сущность. Таким образом, наследуя метод, вы можете фильтровать
        /// и отсекать ненужные компоненты, например, которые выключены <see cref="Enabled"/>.</remarks>
        /// <returns>Перечислитель дочерних сущностей.</returns>
        protected virtual IEnumerable<TEntity> GetEntities()
        {
            foreach (var child in baseEntity.Childs)
            {
                foreach (var child3 in child.GetEntitiesInternal())
                {
                    yield return child3;
                }
            }
        }

        internal virtual void Initialize(SceneEntity<TComponent, TEntity> entity)
        {
            baseEntity = entity;
        }

        internal virtual IEnumerable<TEntity> GetEntitiesInternal()
        {
            return GetEntities();
        }
    }
}
