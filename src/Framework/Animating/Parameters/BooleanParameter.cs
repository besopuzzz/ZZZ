namespace ZZZ.Framework.Animations.Parameters
{
    /// <summary>
    /// Предоставляет класс логического параметра типа <see cref="Boolean"/>.
    /// </summary>
    public class BooleanParameter : Parameter<bool>
    {
        /// <summary>
        /// Предоставляет экземпляр логиеского параметра со значением <see cref="false"/>.
        /// </summary>
        public BooleanParameter() : this(false)
        {

        }

        /// <summary>
        /// Предоставляет экземпляр логиеского параметра с указанным значением.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        public BooleanParameter(bool value) : base(value)
        {

        }

        protected override bool Check(bool value, ParameterCondition condition)
        {
            switch (condition)
            {
                case ParameterCondition.Equals:
                    return value == Value;
                case ParameterCondition.NotEquals:
                    return value != Value;
                case ParameterCondition.More:
                    return value & !Value;
                case ParameterCondition.Less:
                    return !value & Value;
                case ParameterCondition.MoreAndEquals:
                    return value == Value;
                case ParameterCondition.LessAndEquals:
                    return value == Value;
                default: return false;
            }
        }
    }
}
