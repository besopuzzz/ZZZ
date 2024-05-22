namespace ZZZ.Framework.Animations.Parameters
{
    /// <summary>
    /// Предоставляет класс числового параметра с плавающей запятой типа <see cref="float"/>.
    /// </summary>
    public class FloatParameter : Parameter<float>
    {
        /// <summary>
        /// Предоставляет экземпляр числового параметра со значением <see cref="0f"/>.
        /// </summary>
        public FloatParameter() : this(0f)
        {

        }


        /// <summary>
        /// Предоставляет экземпляр числового параметра с указанным значением.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        public FloatParameter(float value) : base(value)
        {

        }

        protected override bool Check(float value, ParameterCondition condition)
        {
            switch (condition)
            {
                case ParameterCondition.Equals:
                    return value == Value;
                case ParameterCondition.NotEquals:
                    return value != Value;
                case ParameterCondition.More:
                    return Value > value;
                case ParameterCondition.Less:
                    return Value < value;
                case ParameterCondition.MoreAndEquals:
                    return Value >= value;
                case ParameterCondition.LessAndEquals:
                    return Value <= value;
                default: return false;
            }
        }
    }
}
