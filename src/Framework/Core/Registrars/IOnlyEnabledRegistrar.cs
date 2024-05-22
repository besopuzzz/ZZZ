namespace ZZZ.Framework.Core.Registrars
{
    /// <summary>
    /// Представляет интерфейс регистратора для регистрации только включенных компонентов <see cref="Component.Enabled"/>=true.
    /// </summary>
    /// <typeparam name="T">Тип компонентов.</typeparam>
    public interface IOnlyEnabledRegistrar<T> : IRegistrar
        where T : IComponent
    {
        /// <summary>
        /// Регистрация компонента, которого включили.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void EnabledReception(T component);

        /// <summary>
        /// Снятие с регистрации компонента, которого выключили.
        /// </summary>
        /// <param name="component">Экземпляр компонента.</param>
        void EnabledDeparture(T component);
    }
}
