using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using ZZZ.Framework;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Physics.Aether;
using ZZZ.Framework.Physics.Aether.Components;
using ZZZ.Framework.Extensions;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Rendering.Components;
using ZZZ.Framework.Tiling.Assets.Physics;
using ZZZ.Framework.Tiling.Components;
using ZZZ.GameProject.Tiles.Components;
using ZZZ.KNI.Content.Pipeline.Serializers;
using ZZZ.Framework.Designing.UnityStyle;
using ZZZ.Framework.KNI;
using ZZZ.Framework.Designing.UnityStyle.Systems;

namespace ZZZ.KNI.GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

        public static MainGame instance;

        private static MainGame game;
        private Scene scene;
        private Framework.IEngine root;

        public static void SetTitle(string titile)
        {
            game.Window.Title = titile;
        }

        public MainGame()
        {
            ModuleInitializer.Initialize();
            
            instance = this;
            game = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

        }


        protected override void Initialize()
        {
            root = new EngineBuilder()
                .UseUnityStyle()
                .UseKNISystemHandler(this)
                .UseKNIStaticAssetManager(Services)
                .RegisterKNIRenderManagerService(GraphicsDevice)
                .RegisterService<IServiceProvider>(Services)
                .UseSystemBottom<AetherSystem>()
                .UseSystemBottom<AetherRendererSystem>()
                .Build();

            scene = new Scene() { Name = "Scene" };

            SceneLoader.Load(scene);


            GameObject gameObject = new GameObject();
            var cameraGB = gameObject.AddGameObject(new GameObject());
            cameraGB.AddComponent<Camera>();
            //cameraGB.GetComponent<Transformer>().Local = new Transform2D(0, 100);
            gameObject.AddComponent<Transformer>().Local = new Transform2D(-200f, -200f);
            gameObject.AddComponent<GroupCollider>().Add(
                new BoxCollider() { Size = new Vector2(32), Friction = 1f }
                //, new BoxCollider() {Offset = new Vector2(62), Size = new Vector2(32) }
                );
            gameObject.AddComponent<HeroController>();
            gameObject.AddComponent<SpriteRenderer>().Sprite = Content.Load<Sprite>("Sprites/main")[0];

            //var testCollider = gameObject.AddGameObject(new GameObject()).AddGameObject(new GameObject());

            //testCollider.AddComponent<Transformer>().Local = new Transform2D(-100f, -100f);
            //testCollider.AddComponent<SpriteRenderer>().Sprite = Content.Load<Sprite>("Sprites/main")[0];
            //testCollider.AddComponent<GroupCollider>().Add(new BoxCollider() { Size = new Vector2(32) });
            //testCollider.AddComponent<Rigidbody>().FixedRotation = true;

            //testCollider = testCollider.AddGameObject(new GameObject());


            //testCollider.AddComponent<Transformer>().Local = new Transform2D(-50f, -50f);
            //testCollider.AddComponent<SpriteRenderer>().Sprite = Content.Load<Sprite>("Sprites/main")[2];

            gameObject.SetParent(scene);

            //scene.AddGameObject(gameObject);


            var x = scene.AddGameObject(new GameObject());

            x.AddGameObject(TestNewTilemap());

            base.Initialize();

        }

        Texture2D quad;

        private GameObject TestNewTilemap()
        {
            quad = Content.Load<Texture2D>("square");
            Sprite main = Content.Load<Sprite>("Sprites/main");

            HeroTile heroTile = new HeroTile();
            heroTile.Color = Color.Red;
            heroTile.Sprite = main[0];
            heroTile.Sprites = [ main[0], main[1], main[2]];


            var container = new GameObject() { Name = "Tilemap" };
            FPSCounter comp = container.AddComponent<FPSCounter>();
            comp.Font = AssetManager.Load<SpriteFont>("Fonts/System");
            container.AddComponent<TilemapCollider>();

            var renderer = container.AddComponent<TilemapRenderer>();
            renderer.RenderMode = TileRenderMode.Stretch;
            renderer.Layer = SortLayer.Layer1;

            var tilemap = container.AddComponent<Tilemap>();
            tilemap.TileSize = new Vector2(32);

            container.AddComponent<TilemapEditor>();

            //tilemap.Add(new Point(5, -2), heroTile);

            //return container;

            Tile tile = new Tile();
            tile.Sprite = main[0];
            tile.Sprites = [main[0],
                main[1],
                main[2],
                main[3],
                main[4],
                main[5],
                main[6],
                main[7],
                main[8],
                main[7],
                main[6],
                main[5],
                main[4],
                main[3],
                main[2],
                main[1],
                main[0]];


            for (int i = 0; i < 100; i++)
                for (int y = 0; y < 10; y++)
                    tilemap.Add(new Point(i, y), tile);

            //container.AddComponent<GroupRender>();

            return container;
        }

        public class HeroTile : Tile
        {
            public override void Startup(Point position, Tilemap tilemap, GameObject container)
            {
                //container.AddGameObject(new GameObject()).AddComponent<Camera>();
                //container.AddComponent<CircleCollider>().Offset = new Vector2(64f);
                //container.AddComponent<HeroController>();



                //var gameObj = container.AddGameObject(new GameObject());
                //gameObj.AddComponent<Transformer>().Local = new Transform2D(200f, 0f);
                ////gameObj.AddComponent<CircleCollider>();
                //gameObj.AddComponent<Rigidbody>();

                //gameObj = container.AddGameObject(new GameObject());
                //gameObj.AddComponent<Transformer>().Local = new Transform2D(100f, 0f);
                //gameObj.AddComponent<SpriteRenderer>().Sprite = this.Sprite;
                ////gameObj.AddComponent<CircleCollider>();

                //base.Startup(position, tilemap, container);
            }

            public override void GetColliderData(Point position, Tilemap tilemap, ref TileColliderData renderedTile)
            {
                base.GetColliderData(position, tilemap, ref renderedTile);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

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

        protected override bool BeginDraw()
        {
            return base.BeginDraw();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

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


