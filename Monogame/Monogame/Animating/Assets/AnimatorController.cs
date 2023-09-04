using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Monogame;
using ZZZ.Framework.Monogame.Assets;

namespace ZZZ.Framework.Monogame.Animations.Assets
{
    /// <summary>
    /// Предоставляет класс контроллера анимаций. Данный класс является ассетом.
    /// </summary>
    public class AnimatorController : Asset
    {
        /// <summary>
        /// Коллекция параметров. 
        /// </summary>
        [ContentSerializer(CollectionItemName = "Parameter")]
        public Dictionary<string, Parameter> Parameters => parameters;

        /// <summary>
        /// Коллекция этапов.
        /// </summary>
        [ContentSerializer(FlattenContent = true, ElementName = "Stage")]
        public Stages Stages => stages;

        /// <summary>
        /// Начальный этап. Создайте переход до вашего этапа.
        /// </summary>
        public Stage StartStage => startStage;

        /// <summary>
        /// Текущий этап контроллера.
        /// </summary>
        [ContentSerializerIgnore]
        public Stage CurrentStage => currentStage;

        /// <summary>
        /// Событие, когда сменился текущий этап контроллера.
        /// </summary>
        public event EventHandler<AnimatorController, Stage> CurrentStageChanged;

        private Stage currentStage  = null!;
        private SharedDicitionary<string, Parameter> parameters = new SharedDicitionary<string, Parameter>();
        private Stages stages = new Stages();
        private Stage startStage = new Stage();

        /// <summary>
        /// Предоставляет экземпляр контроллера анимации.
        /// </summary>
        public AnimatorController()
        {

        }

        /// <summary>
        /// Проверяет текущий этап контроллера на возможность перехода и переходит на другой этап.
        /// </summary>
        internal void CheckConditions()
        {
            if (currentStage == null)
                currentStage = startStage;

            if (currentStage.TryGetNextStage(out Stage next))
            {
                currentStage = next;
                CurrentStageChanged?.Invoke(this, next);
            }
        }
    }
}
