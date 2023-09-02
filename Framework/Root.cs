namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет основной корневой контейнер.
    /// </summary>
    /// <typeparam name="TRegistrar">Тип регистраторов, которые будут использоваться корневым контейром.</typeparam>
    /// <remarks>Унаследуйте класс и используйте методы <see cref="PrepairController(TRegistrar)"/> для подготовки и <see cref="DepairController(TRegistrar)"/> 
    /// для опустошения регистратора.</remarks>
    public class Root<TRegistrar> : Container
        where TRegistrar : IRegistrar
    {
        /// <summary>
        /// Список регистраторов, добавленные в <see cref="Root{TRegistrar}"/>.
        /// </summary>
        protected IEnumerable<TRegistrar> Registrars => registrars;

        private List<TRegistrar> registrars = new();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Root{TRegistrar}"/> без владеемых компонентов и контейнеров.
        /// </summary>
        public Root()
        {
        }

        /// <summary>
        /// Подготавливает регистратор.
        /// </summary>
        /// <param name="registrar">Регистратор, который необходимо подготовить.</param>
        protected virtual void PrepairController(TRegistrar registrar)
        {

        }

        /// <summary>
        /// Опустошает регистратор.
        /// </summary>
        /// <param name="registrar">Регистратор, который необходимо опустошить.</param>
        protected virtual void DepairController(TRegistrar registrar)
        {

        }

        /// <inheritdoc cref="Container.RegistrationComponent{T}(T)"/>
        protected internal override void RegistrationComponent<T>(T component)
        {
            foreach (IRegistrar<T> item in registrars.Where(x => x is IRegistrar<T>).Select(v => (IRegistrar<T>)v))
            {
                item.Reception(component);
            }
        }

        /// <inheritdoc cref="Container.UnregistrationComponent{T}(T)"/>
        protected internal override void UnregistrationComponent<T>(T component)
        {
            foreach (IRegistrar<T> item in registrars.Where(x => x is IRegistrar<T>).Select(v => (IRegistrar<T>)v))
            {
                item.Departure(component);
            }
        }

        /// <inheritdoc cref="Container.OnComponentAdded{T}(T)"/>
        protected override void OnComponentAdded<T>(T component)
        {
            if (component is TRegistrar registrar)
            {
                PrepairController(registrar);
                registrars.Add(registrar);
            }

            base.OnComponentAdded(component);
        }

        /// <inheritdoc cref="Container.OnComponentRemoved{T}(T)"/>
        protected override void OnComponentRemoved<T>(T component)
        {
            if (component is TRegistrar registrar)
            {
                DepairController(registrar);
                registrars.Remove(registrar);
            }

            base.OnComponentRemoved(component);
        }

        /// <inheritdoc cref="Container.Dispose(bool)"/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                registrars.Clear();
            }

            registrars = null;

            base.Dispose(disposing);
        }
    }
}
