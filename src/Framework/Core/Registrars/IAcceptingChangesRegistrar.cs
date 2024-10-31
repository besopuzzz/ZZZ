using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core.Registrars
{
    /// <summary>
    /// Представляет интерфейс регистратора, который отслеживает состояние компонентов и вызывает метод <see cref="IAcceptingChangesRegistrar{T}"/> для каждого из них.
    /// </summary>
    /// <typeparam name="T">Тип компонентов.</typeparam>
    public interface IAcceptingChangesRegistrar<T> : IAnyRegistrar<T>
        where T : IComponent
    {
        /// <summary>
        /// Вызывает метод, если у компонента поменялось состояние.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void AcceptingChanges(IComponent component);
    }
}
