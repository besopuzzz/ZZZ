using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Monogame.Animations.Assets;
using ZZZ.Framework.Monogame.Rendering.Components;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Animations.Components
{
    /// <summary>
    /// Предоставляет класс покадровой анимации. Является игровым компонентом.
    /// </summary>
    [RequireComponent(Duplicate = false, Type = typeof(SpriteRenderer))]
    public class Animator : UpdateComponent
    {
        /// <summary>
        /// Контроллер анимаций.
        /// </summary>
        [ContentSerializer(SharedResource = true)]
        public AnimatorController Controller { get; set; } = null!;

        /// <summary>
        /// Коллекция анимаций, соотнесенная с именами этапов контроллера <see cref="Controller"/>, где ключ является именем этапа.
        /// </summary>
        [ContentSerializer(CollectionItemName = "Animation")]
        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

        private AnimationPlayer player = null!;
        private SpriteRenderer spriteRenderer = null!;
        private Animation currentAnimation = null!;
        private Sprite reserve = null!;

        protected override void Startup()
        {
            Controller.CurrentStageChanged += Controller_CurrentStageChanged;

            spriteRenderer = GetComponent<SpriteRenderer>()!;
            reserve = spriteRenderer.Sprite;

            string stageName = Controller.StartStage?.Name;

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

            base.Startup();
        }

        protected override void Shutdown()
        {
            if (player != null)
            {
                player.Stop();
                player.CurrentSpriteChanged -= Player_SpriteChanged;
            }

            Controller.CurrentStageChanged -= Controller_CurrentStageChanged;
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

        protected override void Update(GameTime gameTime)
        {

            player?.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
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
