using Microsoft.Xna.Framework.Content;

namespace ZZZ.Framework.Animations
{
    /// <summary>
    /// Предоставляет класс для хранения и сравнивания любых значений. Только для наследования.
    /// </summary>
    public abstract class Parameter
    {
        /// <summary>
        /// Проверяет и сравнивает текущее значение <see cref="Value"/> с переданным параметром и указанными условиями.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <param name="condition">Условия для сравнивания.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal abstract bool Check(object value, ParameterCondition condition);

        /// <summary>
        /// Проверяет и устанавливает значение в экземпляр.
        /// </summary>
        /// <param name="value">Новое значение параметра.</param>
        /// <exception cref="ArgumentException"></exception>
        internal abstract void SetValue(object value);
    }

    /// <summary>
    /// Предоставляет класс для хранения и сравнивания любых значений.
    /// </summary>
    public abstract class Parameter<T> : Parameter
    {
        /// <summary>
        /// Значение параметра.
        /// </summary>
        [ContentSerializer]
        public T Value { get; private set; }


        /// <summary>
        /// Предоставляет экземпляр параметра с нулевым значением <see cref="Value"/>. Только для десериализации.
        /// </summary>
        internal Parameter()
        {

        }

        /// <summary>
        /// Предоставляет экземпляр параметра с указанным значением <see cref="Value"/>
        /// </summary>
        public Parameter(T value)
        {
            Value = value;
        }

        /// <inheritdoc cref="Parameter.SetValue(object)"/>
        internal override void SetValue(object value)
        {
            if(value is T)
                Value = (T)value;
            else throw new ArgumentException("The value does not belong to the same type!");
        }

        /// <inheritdoc cref="Parameter.Check(object, ParameterCondition)"/>
        internal override bool Check(object value, ParameterCondition condition)
        {
            if (value is Parameter<T> parameter)
                return Check(parameter.Value, condition);
            else throw new ArgumentException("The parameters being compared do not belong to the same type!");
        }

        /// <inheritdoc cref="Check(object, ParameterCondition)"/>
        protected abstract bool Check(T value, ParameterCondition condition);

        public override string ToString()
        {
            return $"Value: {Value}, Type: {typeof(T).Name}";
        }
    }
}
