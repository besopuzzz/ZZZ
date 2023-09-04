using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ZZZ.Framework;
using ZZZ.Framework.Monogame;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace WinDX
{
    [RequireComponent(Type = typeof(Transformer))]
    public class SceneTransformerController : UpdateComponent
    {
        private KeyboardState keyboardState;
        private Transformer sceneTransformer;

        protected override void Startup()
        {
            sceneTransformer = GetComponent<Transformer>();

            base.Startup();
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            Transform2D local = sceneTransformer.Local;

            if (keyboardState.IsKeyDown(Keys.Left))
                local *= new Transform2D(-1f, 0);
            else if (keyboardState.IsKeyDown(Keys.Right))
                local *= new Transform2D(1f, 0);


            if (keyboardState.IsKeyDown(Keys.Up))
                local *= new Transform2D(0f, -1f);
            else if (keyboardState.IsKeyDown(Keys.Down))
                local *= new Transform2D(0, 1f);

            if (keyboardState.IsKeyDown(Keys.Z))
                local *= Transform2D.CreateRotation(0.01f);
            else if (keyboardState.IsKeyDown(Keys.X))
                local *= Transform2D.CreateRotation(-0.01f);

            if (keyboardState.IsKeyDown(Keys.OemPlus))
                local *= Transform2D.CreateScale(new Vector2(1.01f));
            else if (keyboardState.IsKeyDown(Keys.OemMinus))
                local *= Transform2D.CreateScale(new Vector2(0.99f));

            sceneTransformer.Local = local;

            base.Update(gameTime);
        }
    }
}
