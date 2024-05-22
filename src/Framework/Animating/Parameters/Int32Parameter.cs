namespace ZZZ.Framework.Animations.Parameters
{
    /// <summary>
    /// Предоставляет класс целочисленного параметра типа <see cref="Int32"/>.
    /// </summary>
    public class Int32Parameter : Parameter<int>
    {
        /// <summary>
        /// Предоставляет экземпляр целочисленного параметра со значением <see cref="0"/>.
        /// </summary>
        public Int32Parameter() : this(0)
        {

        }

        /// <summary>
        /// Предоставляет экземпляр целочисленного параметра с указанным значением.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        public Int32Parameter(int value) : base(value)
        {

        }

        protected override bool Check(int value, ParameterCondition condition)
        {
            switch (condition)
            {
                case ParameterCondition.Equals:
                    return value == Value;
                case ParameterCondition.NotEquals:
                    return value != Value;
                case ParameterCondition.More:
                    return value > Value;
                case ParameterCondition.Less:
                    return value < Value;
                case ParameterCondition.MoreAndEquals:
                    return value >= Value;
                case ParameterCondition.LessAndEquals:
                    return value <= Value;
                default: return false;
            }
        }
    }
}
