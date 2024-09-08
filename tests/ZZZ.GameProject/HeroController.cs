using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ZZZ.Framework;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Assets.Rendering;
using ZZZ.Framework.Auding.Components;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Components.Tiling;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Updating;
using ZZZ.Framework.Core.Updating.Components;
using ZZZ.Framework.Physics.Components;
using ZZZ.Framework.UserInterfacing.Components;

namespace ZZZ.KNI.GameProject
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SoundListener))]
    [RequireComponent(typeof(FPSCounter))]
    internal class HeroController : Framework.Component, IUpdateComponent, IStartupComponent
    {
        public Vector2 MaxSpeed { get; set; } = new Vector2(0.5f);

        private Tilemap tilemap;
        private Rigidbody rigidbody;
        private SoundListener soundListener;
        private Transformer camera;
        private Scene scene;
        private UpdateRegistrar updateRegistrar;
        private KeyboardState oldState;
        private Transformer myTransfrom;
        private FPSCounter fPSCounter;

        protected override void Awake()
        {
            tilemap = FindComponent<Tilemap>();
            scene = FindGameObject<Scene>();
            rigidbody = GetComponent<Rigidbody>();

            soundListener = GetComponent<SoundListener>();
            tilemapCollider = FindComponent<TilemapCollider>();

            camera = FindComponent<Camera>()?.Owner?.GetComponent<Transformer>();
            myTransfrom = GetComponent<Transformer>();
            fPSCounter = GetComponent<FPSCounter>();


            GameObject label = new GameObject();

            var transfoer = label.AddComponent<UITransformer>();

            transfoer.Size = new Vector2(100f);
            var lbl = label.AddComponent(new BindableLabel(myTransfrom, "World"));
            lbl.Font = new Font(AssetManager.Load<SpriteFont>("Fonts/System"));
            lbl.BackgroundColor = Color.Green;

            //AddGameObject(label);

            base.Awake();
        }

        void IUpdateComponent.Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var speed = new Vector2();
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


            if (keyboardState.IsKeyDown(Keys.RightShift))
                if (keyboardState.IsKeyDown(Keys.Space))
                    tilemap.Owner.RemoveComponent<TilemapRenderer>();

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {

                if (keyboardState.IsKeyDown(Keys.Space))
                    tilemap.TileSize += Vector2.One;

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

                var local = new Transform2D(scenePos, sceneScale, sceneRotate);
                //scene.GetComponent<Transformer>().Local *= local;

            }


            //if (keyboardState.IsKeyDown(Keys.OemPlus))
            //    updateRegistrar.UpdateOrders[typeof(Camera)].Order += 1;
            //else if (keyboardState.IsKeyDown(Keys.OemMinus))
            //    updateRegistrar.UpdateOrders[typeof(Camera)].Order -= 1;

            //myTransfrom.Local = new Transform2D(myTransfrom.Local.Position + MaxSpeed * speed);
            rigidbody.Velocity += MaxSpeed * speed;

            var old = rigidbody.Owner.GetComponent<Transformer>().HasChanges;
            MainGame.SetTitle((rigidbody.Velocity).ToString());

            oldState = keyboardState;
        }

        TilemapCollider tilemapCollider;

        void IStartupComponent.Startup()
        {
        }
    }
}
