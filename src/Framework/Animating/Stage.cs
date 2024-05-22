using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Animations.Assets;

namespace ZZZ.Framework.Animations
{
    /// <summary>
    /// Предоставляем класс этапа аниматора с коллекцией переходов на другие этапы.
    /// </summary>
    public sealed class Stage
    {
        /// <summary>
        /// Имя этапа.
        /// </summary>
        [ContentSerializer]
        public string Name { get; private set; } = null!;

        /// <summary>
        /// Коллекция переходов на другие этапы с условиями.
        /// </summary>'
        [ContentSerializer(CollectionItemName = "Transition")]
        public List<Transition> Transitions => transitions;

        /// <summary>
        /// Возвращает true, если этап зациклен и повторно воспроизводит анимацию после окончания, иначе false.
        /// </summary>
        public bool IsLoop { get; set; } = true;

        private List<Transition> transitions = null!;

        /// <summary>
        /// Предотавляет экземпляр класса <see cref="Stage"/> без переходов. Только для десериализации.
        /// </summary>
        internal Stage()
        {
            transitions = new List<Transition>();
        }

        /// <summary>
        /// Предотавляет экземпляр класса <see cref="Stage"/> без переходов.
        /// </summary>
        /// <param name="name">Имя этапа.</param>
        public Stage(AnimatorController controller, string name) : this(controller, name, new List<Transition>())
        {
        }

        /// <summary>
        /// Предотавляет экземпляр класса <see cref="Stage"/>.
        /// </summary>
        /// <param name="name">Имя этапа.</param>
        /// <param name="transition">Коллекция переходов.</param>
        public Stage(AnimatorController controller, string name, List<Transition> transition)
        {
            if (controller.Stages.Contains(name))
                throw new ArgumentException($"Stage with name {name} already exist in that controller!");

            Name = name;
            transitions = transition;
        }

        /// <summary>
        /// Выполняет поиск следующего этапа, условия которого первее соблюдены.
        /// </summary>
        /// <param name="next">Следующий этап.</param>
        /// <returns>true, если следующий этап найден, иначе false, а <see cref="next"/> равен null.</returns>
        internal bool TryGetNextStage(out Stage next)
        {
            foreach (var transition in Transitions)
            {
                if(transition.IsConditionsSuccesse())
                {
                    next = transition.Next;
                    return true;
                }
            }

            next = null;
            return false;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Looped: {IsLoop}, Transitions: {Transitions.Count}";
        }
    }
}
