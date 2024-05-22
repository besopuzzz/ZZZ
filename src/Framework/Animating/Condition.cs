using ZZZ.Framework.Animations.Assets;

namespace ZZZ.Framework.Animations
{
    /// <summary>
    /// Предоставляет класс для сравнения параметров <see cref="Parameter{T}"/>.
    /// </summary>
    public sealed class Condition
    {
        /// <summary>
        /// Первостепенный параметр, необходимый для сравнение.
        /// </summary>
        [ContentSerializer(SharedResource = true)]
        public Parameter AnimatorParameter { get; private set; } = null!;

        /// <summary>
        /// Второстепенный параметр, необходимый для сравнения.
        /// </summary>
        public Parameter Parameter { get; set; } = null!;

        /// <summary>
        /// Условия сравнения.
        /// </summary>
        public ParameterCondition ParameterCondition { get; set; }

        /// <summary>
        /// Предоставляет экземпляр сравнения. Только для десериализации.
        /// </summary>
        internal Condition()
        {

        }

        /// <summary>
        /// Предоставляет экземпляр сравнения.
        /// </summary>
        /// <param name="controller">Контроллер с первостепенным параметром.</param>
        /// <param name="parameterName">Имя первостепенного параметра.</param>
        /// <param name="parameterCondition">Условия сравнения.</param>
        /// <param name="parameter">Второстепенный параметр.</param>
        public Condition(AnimatorController controller, string parameterName, ParameterCondition parameterCondition, Parameter parameter)
        {
            AnimatorParameter = controller.Parameters[parameterName];
            Parameter = parameter;
            ParameterCondition = parameterCondition;
        }

        /// <summary>
        /// Сравнивает первостепенный и второстепенный параметр с условиями <see cref="ParameterCondition"/>.
        /// </summary>
        /// <returns>Возвращает <see cref="true"/>, если первостепенный параметр прошел проверку, иначе <see cref="false"/>.</returns>
        public bool IsSuccessed()
        {
            return AnimatorParameter != null ? AnimatorParameter.Check(Parameter, ParameterCondition) : false;
        }

        public override string ToString()
        {
            return $"AnimatorParameter: {AnimatorParameter}, Parameter: {Parameter}, Condition: {ParameterCondition}";
        }
    }
}
