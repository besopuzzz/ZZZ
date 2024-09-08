using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using ZZZ.Framework;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Components.Tiling;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Registrars;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Core.Transforming;
using ZZZ.Framework.Core.Updating;
using ZZZ.Framework.Diagnostics.Components;
using ZZZ.Framework.Physics.Components;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.UserInterfacing;
using ZZZ.KNI.Content.Pipeline.Serializers;

namespace ZZZ.KNI.GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game, IGameInstance
    {
        GraphicsDeviceManager graphics;
        Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

        private static MainGame game;

        public static void SetTitle(string titile)
        {
            game.Window.Title = titile;
        }

        public MainGame()
        {
            ModuleInitializer.Initialize();
            
            game = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            
            GameManagerSettings gameSettings = new GameManagerSettings();
            
            gameSettings.Registrars.Add(new InitializeRegistrar());
            //gameSettings.Registrars.Add(new UpdateRegistrar());
            gameSettings.Registrars.Add(new PhysicRegistrar());
            //gameSettings.Registrars.Add(new RenderRegistrar());
            gameSettings.Registrars.Add(new WorldRenderer());
            //gameSettings.Registrars.Add(new UIRegistrar());
            gameSettings.Registrars.Add(new TransformerRegistrar());

            var main = new GameManager(this, gameSettings);

            //RenderSystem renderSystem = new RenderSystem();
            //renderSystem.Game = this;




            //Components.Add(renderSystem);
        }

        private Scene scene;

        protected override void Initialize()
        {
            SceneLoader.Load(new Scene() {  Name = "Scene"});

            //SceneLoader.CurrentScene.AddGameObject(label);

            var hero = new GameObject();
            hero = new GameObject();
            hero.AddComponent<Transformer>().Local = new Transform2D(100f, 100f, 100f, 2f);
            hero.AddComponent<SpriteRenderer>().Sprite = AssetManager.Load<Sprite>("Sprites/main")[0];

            SceneLoader.CurrentScene.AddGameObject(hero);

            var x = SceneLoader.CurrentScene.AddGameObject(new GameObject());
            //x.AddComponent<GroupRender>();
            x.AddGameObject(TestNewTilemap());


            RenderSystem renderSystem = new RenderSystem();
            renderSystem.Game = this;

            Components.Add(renderSystem);


            UpdateSystem updateSystem  = new UpdateSystem();
            updateSystem.Game = this;

            Components.Add(updateSystem);

            base.Initialize();

        }

        private GameObject TestNewTilemap()
        {

            Sprite main = Content.Load<Sprite>("Sprites/main");

            HeroTile heroTile = new HeroTile();
            heroTile.Color = Color.Red;
            heroTile.Sprite = main[0];
            heroTile.Sprites = [ main[0], main[1], main[2]];


            var container = new GameObject() { Name = "Tilemap" };
            container.AddComponent(new TilemapCollider());
            container.AddComponent(new TilemapRenderer() { RenderMode = TileRenderMode.Stretch, Layer = SortLayer.Layer1 });
            var tilemap = container.AddComponent(new Tilemap() {  TileSize = new Vector2(60)});

            tilemap.Add(new Point(5, -2), heroTile);

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
                main[8]];


            for (int i = 0; i < 100; i++)
            {
                for (int y = 0; y < 10; y++)
                {
                    tilemap.Add(new Point(i, y), tile);
                }
            }

            //container.AddComponent<GroupRender>();

            return container;
        }

        public class HeroTile : Tile
        {
            public override void Startup(Point position, Tilemap tilemap, GameObject container)
            {
                container.AddComponent<HeroController>();
                container.AddGameObject(new GameObject()).AddComponent<Camera>();

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


