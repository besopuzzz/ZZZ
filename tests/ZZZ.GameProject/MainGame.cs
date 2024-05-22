using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ZZZ.Framework;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Components.Tiling;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Registrars;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Transforming;
using ZZZ.Framework.Core.Updating;
using ZZZ.Framework.Diagnostics.Components;
using ZZZ.Framework.Physics.Components;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.KNI.Content.Pipeline;

namespace ZZZ.KNI.GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static MainGame game;

        public static void SetTitle(string titile)
        {
            game.Window.Title = titile;
        }

        public MainGame()
        {
            game = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _ = GameManager.StartBuild(this)
                .UseStaticAssetManager()
                .UseRegistrar(new InitializeRegistrar())
                .UseRegistrar(new UpdateRegistrar())
                .UseRegistrar(new WorldRegistrar())
                .UseRegistrar(new RenderRegistrar())
                .UseRegistrar(new WorldRenderer())
                .UseRegistrar(new TransformerRegistrar())
                .EndBuild();
        }

        private Scene scene;

        protected override void Initialize()
        {

            SceneLoader.Load(new Scene());



            var hero = new GameObject();
            hero.AddGameObject(new GameObject()).AddComponent<Camera>();
            hero.AddComponent<SpriteRenderer>().Sprite = AssetManager.Load<Sprite>("Sprites/main")[0];
            hero.AddComponent<SpriteRenderer>().Order = -100;
            hero.AddComponent<HeroController>();
            hero.AddComponent<BoxCollider>().IsTrigger = false;
            hero.AddComponent<TestComponent>();

            SceneLoader.CurrentScene.AddGameObject(hero);

            hero = new GameObject();
            hero.AddComponent<Transformer>().Local = Transform2D.CreateTranslation(-100f, -100f);
            hero.AddComponent<SpriteRenderer>().Sprite = AssetManager.Load<Sprite>("Sprites/main")[0];
            hero.AddComponent<BoxCollider>().Layer = ColliderLayer.Cat2;

            SceneLoader.CurrentScene.AddGameObject(hero);

            SceneLoader.CurrentScene.AddGameObject(TestNewTilemap());


            base.Initialize();
        }

        private GameObject TestNewTilemap()
        {

            HeroTile heroTile = new HeroTile();
            heroTile.Color = Color.Red;
            heroTile.Sprite = Content.Load<Framework.Rendering.Assets.Sprite>("Sprites/main")[0];
            heroTile.Sprites = new Framework.Rendering.Assets.Sprite[3] { Content.Load<Sprite>("Sprites/main")[0]
                , Content.Load<Sprite>("Sprites/main")[1], Content.Load<Sprite>("Sprites/main")[2] };



            var container = new GameObject();
            container.AddComponent(new TilemapCollider());
            container.AddComponent(new TilemapRenderer());
            container.AddComponent(new TilemapAnimator() { Speed = 10f});
            var tilemap = container.AddComponent(new Tilemap());

            tilemap.Add(new Point(5, -1), heroTile);

            //return container;

            Tile tile = new Tile();
            tile.Sprite = Content.Load<Sprite>("Sprites/main")[0];
            tile.Sprites = new Sprite[9] { Content.Load<Sprite>("Sprites/main")[0], Content.Load<Sprite>("Sprites/main")[1],
                Content.Load<Sprite>("Sprites/main")[2], Content.Load<Sprite>("Sprites/main")[3], Content.Load<Sprite>("Sprites/main")[4]
                , Content.Load<Sprite>("Sprites/main")[5], Content.Load<Sprite>("Sprites/main")[6], Content.Load<Sprite>("Sprites/main")[7]
            , Content.Load<Sprite>("Sprites/main")[8]};


            for (int i = 0; i < 10; i++)
            {
                for (int y = 0; y < 10; y++)
                {
                    tilemap.Add(new Point(i, y), tile);

                }


            }


            return container;

        }

        public class HeroTile : Tile
        {
            public override void Startup(Point position, Tilemap tilemap, GameObject container)
            {
                //container.AddComponent<HeroController>();
                //container.AddGameObject(new GameObject()).AddComponent<Camera>();

                base.Startup(position, tilemap, container);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);



            // TODO: Use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

            // TODO: Unload any non ContentManager content here
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

#if LINUX || WINDOWS

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

#endif
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
    }
}
