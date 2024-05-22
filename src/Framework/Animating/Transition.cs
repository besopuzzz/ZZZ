using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Animations.Assets;

namespace ZZZ.Framework.Animations
{
    /// <summary>
    /// Предоставляет класс для переходов из одного этапа аниматора в другой с определенными условиями.
    /// </summary>
    public sealed class Transition
    {
        /// <summary>
        /// Следующий этап перехода. 
        /// </summary>
        [ContentSerializer(SharedResource = true)]
        public Stage Next { get; private set; } = null!;

        /// <summary>
        /// Условия, благодаря которым произойдет переход на следующий этап.
        /// </summary>
        [ContentSerializer(CollectionItemName = "Condition")]
        public List<Condition> Conditions { get; }

        /// <summary>
        /// Предоставляет экземпляр класса <see cref="Transition"/> без условий перехода. Только для десериализации.
        /// </summary>
        internal Transition()
        {
            Conditions = new List<Condition>();
        }

        /// <summary>
        /// Предоставляет экземпляр класса <see cref="Transition"/>.
        /// </summary>
        /// <param name="next">Следующий этап.</param>
        /// <param name="conditions">Условия, необходимые для перехода на следующий <see cref="Next"/>  этап.</param>
        public Transition(AnimatorController controller, string nextStage, List<Condition> conditions)
        {
            Next = controller.Stages[nextStage];
            Conditions = conditions;
        }

        /// <inheritdoc cref="Transition.Transition(AnimatorController, string, List{Condition})"/>
        public Transition(AnimatorController controller, string nextStage, params Condition[] conditions)
        {
            Next = controller.Stages[nextStage];
            Conditions = conditions.ToList();
        }

        /// <summary>
        /// Возвращает <see cref="true"/>, если все условия <see cref="Conditions"/> были выполнены.
        /// </summary>
        /// <returns>true - если все выполнены, иначе false.</returns>
        internal bool IsConditionsSuccesse()
        {
            return !Conditions.Any(x => !x.IsSuccessed());
        }

        public override string ToString()
        {
            return $"Next: {Next}, Conditions: {Conditions.Count}";
        }
    }
}
