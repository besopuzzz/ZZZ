namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет интерфейс регистратора компонентов <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Тип компонента, который регистратор будет обрабатывать.</typeparam>
    public interface IRegistrar<TComponent> : IRegistrar
        where TComponent : IComponent
    {
        /// <summary>
        /// Проводит регистрацию компонента.
        /// </summary>
        /// <param name="component">Экземпляр компонента, который необходимо зарегистрировать.</param>
        internal void Reception(TComponent component);

        /// <summary>
        /// Отменяет регистрацию компонента.
        /// </summary>
        /// <param name="component">Экземпляр компонента, который необходимо снять с регистрации.</param>
        internal void Departure(TComponent component);
    }

    /// <summary>
    /// Представляет основной интерфейс регистратора компонентов.
    /// </summary>
    /// <remarks>Используйте для создания других регистраторов.</remarks>
    public interface IRegistrar : IComponent
    {
    }

}
