namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет базовый класс регистратора компонентов <see href="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Тип компонента, который регистратор будет обрабатывать.</typeparam>
    public abstract class Registrar<TComponent> : Registrar, IRegistrar<TComponent>
        where TComponent : IComponent
    {
        /// <inheritdoc cref="IRegistrar{TComponent}.Reception(TComponent)"/>
        protected abstract void Reception(TComponent component);

        /// <inheritdoc cref="IRegistrar{TComponent}.Departure(TComponent)"/>
        protected abstract void Departure(TComponent component);

        void IRegistrar<TComponent>.Reception(TComponent component)
        {
            Reception(component);
        }

        void IRegistrar<TComponent>.Departure(TComponent component)
        {
            Departure(component);
        }
    }

    /// <summary>
    /// Представляет базовый класс регистратора без регистрации компонентов.
    /// </summary>
    /// <remarks>Используйте этот класс для инициализации, например, статичных классов.</remarks>
    public abstract partial class Registrar : Component, IRegistrar
    {

    }
}
