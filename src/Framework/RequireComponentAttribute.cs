namespace ZZZ.Framework
{
    /// <summary>
    /// Предоставляет аттрибут для автоматического добавления компонента <see cref="Component"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequireComponentAttribute : Attribute
    {
        /// <summary>
        /// Тип компонента, который необходимо добавить.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Если <see href="true"/>, то компонент будет удален вместе с основным.
        /// </summary>
        public bool Remove { get; }

        /// <summary>
        /// Порядок добавления/удаления обязательного компонента, по сравнению с основным.
        /// </summary>
        public AddingOrderType AddingOrder { get; }


        public RequireComponentAttribute(Type type, AddingOrderType order = AddingOrderType.Before, bool remove = false)
        {
            Type = type;
            AddingOrder = order;
            Remove = remove;
        }
    }

    /// <summary>
    /// Предоставляет список порядка добавления/удаления обязательного компонента, используемый в атрибуте <see cref="RequireComponentAttribute.AddingOrder"/>.
    /// </summary>
    public enum AddingOrderType
    {
        /// <summary>
        /// Обязательный компонент должен быть добавлен после основного компонента.
        /// </summary>
        After,

        /// <summary>
        /// Обязательный компонент должен быть добавлен до основного компонента.
        /// </summary>
        Before
    }

}
