namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет основной интерфейс для компонентов.
    /// </summary>
    public interface IComponent : IDisposable
    {
        /// <summary>
        /// Владелец этого компонента.
        /// </summary>
        IContainer Owner { get; internal set; }

        /// <summary>
        /// Получает значение, включен или выключен компонент.
        /// <see cref="True"/> - объект включен. Иначе - выключен.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Enabled"/> было изменено.
        /// </summary>
        event EventHandler<IComponent, bool> EnabledChanged;

        /// <summary>
        /// Запускает компонент. Вызывается единожды после добавления в <see cref="Owner"/> (если <see cref="Owner"/> уже запущен) или после запуска <see cref="Owner"/>.
        /// </summary>
        /// <remarks>Унаследуйте метод и подготовьте компонент к работе. 
        /// Например, получите другой компонент <see cref="GetComponent{T}()"/> и подпишитесь на события <see cref="EnabledChanged"/>.</remarks>
        internal void Startup();

        /// <summary>
        /// Выключает объект. Вызывается единожды после удаления из <see cref="Owner"/> (если <see cref="Owner"/> уже запущен) или после отключения <see cref="Owner"/>.
        /// </summary>
        /// <remarks>Унаследуйте метод и освободите компонент от работы.
        /// Например, отпишитесь от подписанных событий на другие компоненты.</remarks>
        internal void Shutdown();

        /// <inheritdoc cref="IContainer.RegistrationComponent{T}(T)"/>
        internal void RegistrationComponent<T>(T component) where T : IComponent;

        /// <inheritdoc cref="IContainer.UnregistrationComponent{T}(T)"/>
        internal void UnregistrationComponent<T>(T component) where T : IComponent;

        /// <summary>
        /// Выполняет регистрацию компонентов в регистраторах.
        /// </summary>
        /// <remarks>Унаследуйте метод и вызовете <see cref="RegistrationComponent{T}(T)"/> для регистрации вашего компонента.</remarks>
        internal void RegistrationComponents();

        /// <summary>
        /// Выполняет снятие регистраций компонентов в регистраторах.
        /// </summary>
        /// <remarks>Унаследуйте метод и вызовете <see cref="UnregistrationComponent{T}(T)"/> для снятия регистрации вашего компонента.</remarks>
        internal void UnregistrationComponents();
    }
}
