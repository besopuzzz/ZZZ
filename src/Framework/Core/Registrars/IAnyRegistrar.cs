namespace ZZZ.Framework.Core.Registrars
{
    /// <summary>
    /// Представляет интерфейс регистратора, который обрабатывает компоненты в любом состоянии. 
    /// Вызывает метод регистрации и депортации только в том случае, если компонент был добавлен или удален со сцены.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAnyRegistrar<T> : IRegistrar
        where T : IComponent
    {
        /// <summary>
        /// Вызывает метод, когда компонент проходит регистрацию.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void Reception(T component);

        /// <summary>
        /// Вызывает метод, когда компонент снимают с регистрации.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void Departure(T component);
    }
}
