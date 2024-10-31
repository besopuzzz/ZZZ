using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core.Registrars
{
    /// <summary>
    /// Представляет интерфейс регистратора, который обрабатывает только выключенные компоненты.
    /// </summary>
    /// <typeparam name="T">Тип компонентов.</typeparam>
    /// <remarks>Например, если компонент был зарегистрирован, а потом был включен (<see cref="IComponent.Enabled"/> равен true), то компонент будет депортирован.</remarks>
    public interface IOnlyDisabledRegistrar<T> : IRegistrar
        where T : IComponent
    {
        /// <summary>
        /// Вызывает метод регистрации компонента, который выключили.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void DisabledReception(T component);

        /// <summary>
        /// Вызывает метод регистрации компонента, который включили.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void DisabledDeparture(T component);
    }
}
