using ZZZ.Framework.Animations.Assets;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Updating.Components;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Animations.Components
{
    /// <summary>
    /// Предоставляет класс покадровой анимации. Является игровым компонентом.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Animator : Component, IUpdateComponent
    {
        /// <summary>
        /// Контроллер анимаций.
        /// </summary>
        [ContentSerializer(SharedResource = true)]
        public AnimatorController Controller 
        { 
            get => controller;
            set
            {
                if(controller == value) return;

                if(controller != null)
                    controller.CurrentStageChanged -= Controller_CurrentStageChanged;

                controller = value;

                if(controller !=null)
                    controller.CurrentStageChanged += Controller_CurrentStageChanged;

            }
        }
        /// <summary>
        /// Коллекция анимаций, соотнесенная с именами этапов контроллера <see cref="Controller"/>, где ключ является именем этапа.
        /// </summary>
        [ContentSerializer(CollectionItemName = "Animation")]
        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

        private AnimationPlayer player = null!;
        private SpriteRenderer spriteRenderer = null!;
        private Animation currentAnimation = null!;
        private Sprite reserve = null!;
        private AnimatorController controller = null!;

        protected override void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>()!;
            reserve = spriteRenderer.Sprite;

            string stageName = Controller?.CurrentStage?.Name;

            if (stageName == null)
                return;

            if (Animations.ContainsKey(stageName))
            {
                currentAnimation = Animations[stageName];

                player = new AnimationPlayer(currentAnimation);
                player.CurrentSpriteChanged += Player_SpriteChanged;
                player.IsLoop = Controller.CurrentStage.IsLoop;
                player.Start();
            }

            base.Awake();
        }

        protected override void Shutdown()
        {
            if (player != null)
            {
                player.Stop();
                player.CurrentSpriteChanged -= Player_SpriteChanged;
            }

            spriteRenderer.Sprite = reserve;
            reserve = null!;

            base.Shutdown();
        }

        private void Player_SpriteChanged(object sender, EventArgs e)
        {
            spriteRenderer.Sprite = player.CurrentSprite;
        }

        private void Controller_CurrentStageChanged(AnimatorController controller, Stage e)
        {
            if (!Animations.ContainsKey(e.Name))
                return;

            currentAnimation = Animations[e.Name];

            if (player == null)
            {
                player = new AnimationPlayer(currentAnimation);
                player.CurrentSpriteChanged += Player_SpriteChanged;
                player.Start();
            }

            player.Animation = currentAnimation;
            player.IsLoop = e.IsLoop;

            if (player.CurrentSprite != null)
                spriteRenderer.Sprite = player.CurrentSprite;
        }

        void IUpdateComponent.Update(GameTime gameTime)
        {
            player?.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        /// <summary>
        /// Устанавливает новое значение в параметр контроллера <see cref="Controller"/>.
        /// </summary>
        /// <param name="name">Имя параметра.</param>
        /// <param name="value">Новое значение параметра.</param>
        public void SetValue(string name, object value)
        {
            Controller.Parameters[name].SetValue(value);
            Controller.CheckConditions();
        }
    }

}
