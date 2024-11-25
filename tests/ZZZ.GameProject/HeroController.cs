using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using ZZZ.Framework;
using ZZZ.Framework.Auding.Components;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Physics.Aether.Components;
using ZZZ.Framework.Rendering.Components;
using ZZZ.Framework.Tiling.Components;
using ZZZ.Framework.Updating;

namespace ZZZ.KNI.GameProject
{
    [RequiredComponent<Rigidbody>]
    [RequiredComponent<FPSCounter>]
    internal class HeroController : Component, IUpdater
    {
        public Vector2 MaxSpeed { get; set; } = new Vector2(10f);

        private Tilemap tilemap;
        private Rigidbody rigidbody;
        private SoundListener soundListener;
        private Transformer camera;
        private Scene scene;
        private KeyboardState oldState;
        private Transformer myTransfrom;
        private FPSCounter fPSCounter;

        protected override void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            camera = ((Camera)Camera.MainCamera).Owner.GetComponent<Transformer>();
            myTransfrom = GetComponent<Transformer>();
            fPSCounter = GetComponent<FPSCounter>();

            base.Awake();
        }

        void IUpdater.Update(TimeSpan time)
        {
            var keyboardState = Keyboard.GetState();
            var speed = new Vector2(0f);
            var scenePos = new Vector2();
            var sceneRotate = 0f;
            var sceneScale = Vector2.One;

            if (keyboardState.IsKeyDown(Keys.A))
                speed.X = -1f;
            else if (keyboardState.IsKeyDown(Keys.D))
                speed.X = 1f;

            if (keyboardState.IsKeyDown(Keys.W))
                speed.Y = -1f;
            else if (keyboardState.IsKeyDown(Keys.S))
                speed.Y = 1f;

            //if (keyboardState.IsKeyDown(Keys.Space) & oldState.IsKeyUp(Keys.Space))
            //    rigidbody.Enabled = !rigidbody.Enabled;

            if (keyboardState.IsKeyDown(Keys.Space) & !oldState.IsKeyDown(Keys.Space))
                fPSCounter.Enabled = !fPSCounter.Enabled;

            if (keyboardState.IsKeyDown(Keys.RightShift))
                if (keyboardState.IsKeyDown(Keys.Space))
                    tilemap.Owner.GetComponent<TilemapCollider>().Enabled = !tilemap.Owner.GetComponent<TilemapCollider>().Enabled;

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                Transform2D local = camera.Local;


                if (keyboardState.IsKeyDown(Keys.Left))
                    scenePos.X -= 1f;
                else if (keyboardState.IsKeyDown(Keys.Right))
                    scenePos.X += 1f;

                if (keyboardState.IsKeyDown(Keys.Up))
                    scenePos.Y -= 1f;
                else if (keyboardState.IsKeyDown(Keys.Down))
                    scenePos.Y += 1f;


                if (keyboardState.IsKeyDown(Keys.Q))
                    sceneRotate -= 0.01f;
                else if (keyboardState.IsKeyDown(Keys.E))
                    sceneRotate += 0.01f;


                if (keyboardState.IsKeyDown(Keys.Z))
                    sceneScale *= 0.9f;
                else if (keyboardState.IsKeyDown(Keys.X))
                    sceneScale *= 1.1f;

                local *= new Transform2D(scenePos, sceneScale, sceneRotate);

                camera.Local = local;
                //scene.GetComponent<Transformer>().Local *= local;

            }

            //if (keyboardState.IsKeyDown(Keys.OemPlus))
            //    updateRegistrar.UpdateOrders[typeof(Camera)].Order += 1;
            //else if (keyboardState.IsKeyDown(Keys.OemMinus))
            //    updateRegistrar.UpdateOrders[typeof(Camera)].Order -= 1;

            myTransfrom.Local = new Transform2D(myTransfrom.Local.Position + MaxSpeed * speed);

            //rigidbody.Velocity += MaxSpeed * speed;

            //myTransfrom.Local = new Transform2D( Mouse.GetState().Position.ToVector2());

            var old = rigidbody.Owner.GetComponent<Transformer>().HasChanges;
            MainGame.SetTitle(fPSCounter.FPS.ToString());

            oldState = keyboardState;
        }

        TilemapCollider tilemapCollider;

    }
}
