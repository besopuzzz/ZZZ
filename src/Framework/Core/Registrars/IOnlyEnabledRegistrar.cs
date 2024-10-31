using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core.Registrars
{
    /// <summary>
    /// Представляет интерфейс регистратора, который обрабатывает только включенные компоненты.
    /// </summary>
    /// <typeparam name="T">Тип компонентов.</typeparam>
    /// <remarks>Например, если компонент был принят, а потом был выключен (<see cref="IComponent.Enabled"/> равен false), то компонент будет депортирован.</remarks>
    public interface IOnlyEnabledRegistrar<T> : IRegistrar
        where T : IComponent
    {
        /// <summary>
        /// Вызывает прием компонента, который включили.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void EnabledReception(T component);

        /// <summary>
        /// Вызывает депортацию компонента, который выключили.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void EnabledDeparture(T component);
    }
}
