using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Animations.Assets
{
    /// <summary>
    /// Предоставляет класс контроллера анимаций. Данный класс является ассетом.
    /// </summary>
    public class AnimatorController : Asset
    {
        /// <summary>
        /// Коллекция параметров. 
        /// </summary>
        [ContentSerializer(CollectionItemName = "Parameter", Optional = true)]
        public Dictionary<string, Parameter> Parameters => parameters;

        /// <summary>
        /// Коллекция этапов.
        /// </summary>
        [ContentSerializer(FlattenContent = true, ElementName = "Stage", Optional = true)]
        public Stages Stages => stages;

        /// <summary>
        /// Текущий этап контроллера.
        /// </summary>
        [ContentSerializer(SharedResource = true)]
        public Stage CurrentStage { get; set; }

        /// <summary>
        /// Событие, когда сменился текущий этап контроллера.
        /// </summary>
        public event EventHandler<AnimatorController, Stage> CurrentStageChanged;

        private SharedDicitionary<string, Parameter> parameters = new SharedDicitionary<string, Parameter>();
        private Stages stages = new Stages();

        /// <summary>
        /// Предоставляет экземпляр контроллера анимации.
        /// </summary>
        public AnimatorController()
        {

        }

        /// <summary>
        /// Предоставляет экземпляр контроллера анимации с заданными параметрами.
        /// </summary>
        public AnimatorController(SharedDicitionary<string, Parameter> parametres, Stages stages, Stage startStage)
        {
            this.parameters = parametres;
            this.stages = stages;
            this.CurrentStage = startStage;
        }

        /// <summary>
        /// Проверяет текущий этап контроллера на возможность перехода и переходит на другой этап.
        /// </summary>
        internal void CheckConditions()
        {
            if (CurrentStage == null)
                return;

            if (CurrentStage.TryGetNextStage(out Stage next))
            {
                CurrentStage = next;
                CurrentStageChanged?.Invoke(this, next);
            }
        }
    }
}
