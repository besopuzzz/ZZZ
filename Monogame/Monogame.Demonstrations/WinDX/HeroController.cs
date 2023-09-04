using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ZZZ.Framework.Monogame.Animations;
using ZZZ.Framework.Monogame.Animations.Components;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace WinDX
{
    public class HeroController : UpdateComponent
    {
        public Point MovingDirection => moveDirection;
        public Vector2 Speed { get; set; } = Vector2.One;

        private Rigidbody rigidbody = null;
        private Animator animator = null;
        private Point moveDirection = Point.Zero;
        protected override void Startup()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            base.Startup();
        }

        protected override void Update(GameTime gameTime)
        {
            if (rigidbody == null)
                return;

            moveDirection = Point.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                moveDirection.X = -1;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                moveDirection.X = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                moveDirection.Y = -1;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                moveDirection.Y = 1;

            rigidbody.Velocity += moveDirection.ToVector2() * Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            animator.SetValue("velocityX", moveDirection.X);
            animator.SetValue("velocityY", moveDirection.Y);

            base.Update(gameTime);
        }


    }
}
