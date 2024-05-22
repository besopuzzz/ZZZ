namespace ZZZ.Framework.Core.Registrars
{
    public interface IAnyRegistrar<T> : IRegistrar
        where T : IComponent
    {
        /// <summary>
        /// Вызывает событие, когда компонент проходит регистрацию.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void Reception(T component);

        /// <summary>
        /// Вызывает событие, когда компонент снимают с регистрации.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void Departure(T component);
    }
}
