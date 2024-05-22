
//namespace ZZZ.Framework
//{
//    /// <summary>
//    /// Представляет основной интерфейс для контейнеров. 
//    /// </summary>
//    public interface GameObject : IDisposable
//    {
//        /// <summary>
//        /// Имя контейнера.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// Владелец этого контейнера.
//        /// </summary>
//        GameObject Owner { get; internal set; }

//        Scene Scene { get; internal set; }

//        /// <summary>
//        /// Получает значение, включен или выключен контейнер.
//        /// <see href="true"/> - объект включен. Иначе - выключен.
//        /// </summary>
//        bool Enabled { get; set; }

//        /// <summary>
//        /// Вызывает событие, когда значение <see cref="Enabled"/> было изменено.
//        /// </summary>
//        event EventHandler<EventArgs> EnabledChanged;

//        /// <summary>
//        /// Запускает контейнер. Вызывается единожды после добавления в <see cref="Owner"/> (если <see cref="Owner"/> уже запущен) или после запуска <see cref="Owner"/>.
//        /// В случае, когда этот контейнер является корнем, метод необходимо вызывать вручную.
//        /// </summary>
//        /// <remarks>Унаследуйте метод и подготовьте контейнер к работе. 
//        /// Например, получите другой контейнер <see cref="GetGameObject{T}()"/> и подпишитесь на события <see cref="EnabledChanged"/>.</remarks>
//        internal void Startup();

//        /// <summary>
//        /// Выключает объект. Вызывается единожды после удаления из <see cref="Owner"/> (если <see cref="Owner"/> уже запущен) или после отключения <see cref="Owner"/>.
//        /// В случае, когда этот контейнер является корнем, метод необходимо вызывать вручную.
//        /// </summary>
//        /// <remarks>Унаследуйте метод и освободите контейнер от работы.
//        /// Например, отпишитесь от подписанных событий на другие контейнеры.</remarks>
//        internal void Shutdown();

//        /// <summary>
//        /// Выполняет поиск регистратора и регистрирует компонент.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента, по которому выполняется поиск регистратора <see cref="IRegistrar{T}"/>.</typeparam>
//        /// <param name="component">Экземпляр компонента, который необходимо зарегистрировать.</param>
//        /// <remarks>Укажите экземпляр с типом, например <see href="IOtherType"/>, для зарегистрации экземпляра в регистраторe <see href="IRegistrar{IOtherType}"/>.</remarks>
//        internal void RegisterComponent<T>(T component) where T : Component;

//        /// <summary>
//        /// Выполняет поиск регистратора и отменяет регистрацию компонента.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента, по которому выполняется поиск регистратора <see cref="IRegistrar{T}"/>.</typeparam>
//        /// <param name="component">Экземпляр компонента, по которому необходимо отменить регистрацию.</param>
//        /// <remarks>Укажите экземпляр с типом, например <see href="IOtherType"/>, для отмены зарегистрации экземпляра в регистраторe <see href="IRegistrar{IOtherType}"/>.</remarks>
//        internal void UnregisterComponent<T>(T component) where T : Component;

//        /// <summary>
//        /// Вызывает событие, когда в контейнер добавлен владеемый контейнер.
//        /// </summary>
//        event EventHandler<GameObject, GameObject> GameObjectAdded;

//        /// <summary>
//        /// Вызывает событие, когда в контейнере удален владеемый контейнер.
//        /// </summary>
//        event EventHandler<GameObject, GameObject> GameObjectRemoved;

//        /// <summary>
//        /// Вызывает событие, когда в контейнер добавлен компонент.
//        /// </summary>
//        event EventHandler<GameObject, Component> ComponentAdded;

//        /// <summary>
//        /// Вызывает событие, когда в контейнере удален компонент.
//        /// </summary>
//        event EventHandler<GameObject, Component> ComponentRemoved;

//        /// <summary>
//        /// Добавляет контейнер и становится его владельцем.
//        /// </summary>
//        /// <typeparam name="T">Тип контейнера.</typeparam>
//        /// <param name="child">Экземпляр контейнера.</param>
//        /// <returns>Возвращает добавленный контейнер <paramref name="child"/>.</returns>
//        T AddGameObject<T>(T child) where T : GameObject;

//        /// <summary>
//        /// Удаляет локальный контейнер и прекращает быть его владельцем.
//        /// </summary>
//        /// <typeparam name="T">Тип контейнера.</typeparam>
//        /// <param name="child">Экземпляр контейнера.</param>
//        void RemoveGameObject<T>(T child) where T : GameObject;

//        /// <summary>
//        /// Добавляет компонент и становится его владельцем.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <param name="component">Экземпляр компонента.</param>
//        /// <returns>Возвращает добавленный компонент <paramref name="component"/>.</returns>
//        T AddComponent<T>(T component) where T : Component;

//        /// <summary>
//        /// Создает,  а так же добавляет компонент и становится его владельцем.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <returns>Возвращает добавленный компонент.</returns>
//        T AddComponent<T>() where T : Component, new();

//        /// <summary>
//        /// Удаляет компонент и прекращает быть его владельцем.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <param name="component">Экземпляр компонента.</param>
//        void RemoveComponent<T>(T component) where T : Component;

//        /// <summary>
//        /// Удаляет компонент и прекращает быть его владельцем.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        void RemoveComponent<T>() where T : Component;

//        /// <summary>
//        /// Возвращает найденный владеемый компонент.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <returns>Найденный <typeparamref name="T"/> компонент. Возвращает <see href="null"/>, если компонент с типом <typeparamref name="T"/> отсутствует.</returns>
//        T GetComponent<T>() where T : Component;

//        /// <summary>
//        /// Возвращает найденный владеемый компонент.
//        /// </summary>
//        /// <param name="type">Тип компонента.</param>
//        /// <returns>Найденный компонент. Возвращает <see href="null"/>, если компонент типа type отсутствует.</returns>
//        Component GetComponent(Type type);

//        /// <summary>
//        /// Возвращает первый владеемый контейнер.
//        /// </summary>
//        /// <typeparam name="T">Тип контейнера.</typeparam>
//        /// <returns>Найденный <typeparamref name="T"/> контейнер. Возвращает <see href="null"/>, если контейнер с типом <typeparamref name="T"/> отсутствует.</returns>
//        T GetGameObject<T>() where T : GameObject;

//        /// <summary>
//        /// Возвращает все владеемые компоненты.
//        /// </summary>
//        /// <returns>Коллекция компонентов <see cref="IEnumerable{Component}"/>.</returns>
//        IEnumerable<Component> GetComponents();

//        /// <summary>
//        /// Возвращает все владеемые компоненты с указанным типом.
//        /// </summary>
//        /// <returns>Коллекция компонентов <see cref="IEnumerable{Component}"/>.</returns>
//        IEnumerable<T> GetComponents<T>() where T : Component;

//        /// <summary>
//        /// Возвращает все владеемые контейнеры.
//        /// </summary>
//        /// <returns>Коллекция контейнеров <see cref="IEnumerable{GameObject}"/>.</returns>
//        IEnumerable<GameObject> GetGameObjects();

//        /// <summary>
//        /// Выполняет поиск контейнера начиная с корня.
//        /// </summary>
//        /// <typeparam name="T">Тип контейнера.</typeparam>
//        /// <returns>Первый найденный <typeparamref name="T"/> контейнер. Возвращает <see href="null"/>, если контейнер не найден.</returns>
//        T FindGameObject<T>() where T : GameObject;

//        /// <summary>
//        /// Выполняет поиск контейнера начиная с корня.
//        /// </summary>
//        /// <typeparam name="T">Тип контейнера.</typeparam>
//        /// <param name="predicate">Делегат с условиями поиска.</param>
//        /// <returns>Первый найденный <typeparamref name="T"/> контейнер. Возвращает <see href="null"/>, если контейнер не найден.</returns>
//        T FindGameObject<T>(Predicate<T> predicate) where T : GameObject;

//        /// <summary>
//        /// Выполняет поиск контейнеров начиная с корня.
//        /// </summary>
//        /// <typeparam name="T">Тип объекта.</typeparam>
//        /// <returns>Коллекция найденных <see cref="IEnumerable{ComponentGameObject}"/> контейнеров.</returns>
//        IEnumerable<T> FindGameObjects<T>() where T : GameObject;

//        /// <summary>
//        /// Выполняет поиск контейнеров начиная с корня.
//        /// </summary>
//        /// <param name="predicate">Делегат с условиями поиска.</param>
//        /// <returns>Коллекция найденных <see cref="IEnumerable{ComponentGameObject}"/> контейнеров.</returns>
//        IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate);

//        /// <summary>
//        /// Выполняет поиск компонента начиная с корня.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <returns>Первый найденный <typeparamref name="T"/> компонент. Возвращает <see href="null"/>, если компонент не найден.</returns>
//        T FindComponent<T>() where T : Component;

//        /// <summary>
//        /// Выполняет поиск компонента начиная с корня.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <param name="predicate">Делегат с условиями поиска.</param>
//        /// <returns>Первый найденный компонент. Возвращает <see href="null"/>, если компонент не найден.</returns>
//        T FindComponent<T>(Predicate<T> predicate) where T : Component;

//        /// <summary>
//        /// Выполняет поиск компонентов начиная с корня.
//        /// </summary>
//        /// <typeparam name="T">Тип компонента.</typeparam>
//        /// <returns>Коллекция найденных <see cref="IEnumerable{T}"/> компонентов.</returns>
//        IEnumerable<T> FindComponents<T>() where T : Component;

//        /// <summary>
//        /// Выполняет поиск компонентов начиная с корня.
//        /// </summary>
//        /// <param name="predicate">Делегат с условиями поиска.</param>
//        /// <returns>Коллекция найденных <see cref="IEnumerable{Component}"/> компонентов.</returns>
//        IEnumerable<Component> FindComponents(Predicate<Component> predicate);
//    }
//}
