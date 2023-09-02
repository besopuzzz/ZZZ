namespace ZZZ.Framework
{
    /// <summary>
    /// Предоставляет аттрибут для автоматического добавления компонента <see cref="IComponent"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireComponentAttribute : Attribute
    {
        /// <summary>
        /// Тип компонента, который необходимо добавить.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Если <see href="true"/>, то добавляемый компомент будет игнорировать наличие такого же типа у <see cref="IContainer"/>.
        /// </summary>
        public bool Duplicate { get; set; }

        /// <summary>
        /// Если <see href="true"/>, то компонент будет удален вместе с основным.
        /// </summary>
        public bool Remove { get; set; }

        /// <summary>
        /// Если <see href="true"/>, то компонент будет добавлен во владельца контейнера <see cref="IComponent.Owner"/>.
        /// </summary>
        public bool ToOwner { get; set; }
    }
}
