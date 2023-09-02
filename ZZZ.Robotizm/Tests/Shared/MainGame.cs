using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Robotizm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZZZ.Framework.Monogame.Animations;
using ZZZ.Framework.Monogame.Animations.Components;
using ZZZ.Framework.Monogame.Animations.Content;
using ZZZ.Framework.Monogame.Animations.Parameters;
using ZZZ.Framework.Monogame.Audio;
using ZZZ.Framework.Monogame.Content;
using ZZZ.Framework.Monogame.Extentions;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Main.UI;
using ZZZ.Framework.Monogame.Rendering;
using ZZZ.Framework.Monogame.Rendering.Components;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Tiled.Components;
using ZZZ.Framework.Monogame.Tiled.Components.Physics;
//using ZZZ.Framework.Tiled.Components.Physics;
using ZZZ.Framework.Monogame.Tiled.Components.Rendering;
using ZZZ.Framework.Monogame.Tiled.Content;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating;
using ZZZ.Framework.Monogame;

namespace ZZZ.Framework.Main
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        private Camera camera;
        private Scene _gameManager;

        protected override void Initialize()
        {
            _gameManager = new Scene(this);
            _gameManager.Name = "Scene";
            _gameManager.UseZZZ();
            _gameManager.AddContainer(CreateLevel());
            _gameManager.AddContainer(CreateHero());


            //_gameManager = Scene.FromAsset("Scene1", Content.ServiceProvider);
            //_gameManager.Enabled = false;

            Components.Add(_gameManager);

            base.Initialize();
        }



        private Container CreateHero()
        {
            var sprite = Content.Load<Sprite>("Sprites/robot");

            Container hero = new Container();
            hero.Name = "Hero";
            hero.AddComponent(new SpriteRenderer() { Sprite = sprite[48] });

            #region Animator

            Animator animator = new Animator();
            AnimatorController controller = new AnimatorController();
            controller.Parameters.Add("velocityX", new Int32Parameter(0));
            controller.Parameters.Add("velocityY", new Int32Parameter(0));

            Stage down = new Stage(controller, "down");
            Stage left = new Stage(controller, "left");
            Stage right = new Stage(controller, "right");
            Stage up = new Stage(controller, "up");

            controller.Stages.Add(down);
            controller.Stages.Add(left);
            controller.Stages.Add(right);
            controller.Stages.Add(up);

            /// Down

            down.Transitions.Add(new Transition(controller, "up",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(-1))));
            down.Transitions.Add(new Transition(controller, "left",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(0))));
            down.Transitions.Add(new Transition(controller, "right",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(0))));


            animator.Animations.Add("down", new Animation(0f, 1f, sprite[48]));

            //// Left

            left.Transitions.Add(new Transition(controller, "down",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));
            left.Transitions.Add(new Transition(controller, "right",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(1))));
            left.Transitions.Add(new Transition(controller, "up",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));

            animator.Animations.Add("left", new Animation(0f, 1f, sprite[18]));


            //// Right

            right.Transitions.Add(new Transition(controller, "down",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));
            right.Transitions.Add(new Transition(controller, "left",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(-1))));
            right.Transitions.Add(new Transition(controller, "up",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));

            animator.Animations.Add("right", new Animation(0f, 1f, sprite[34]));

            //// Top

            up.Transitions.Add(new Transition(controller, "right",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(0))));
            up.Transitions.Add(new Transition(controller, "left",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(0))));
            up.Transitions.Add(new Transition(controller, "down",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(1))));

            animator.Animations.Add("up", new Animation(0f, 1f, sprite[4]));


            controller.StartStage.Transitions.Add(new Transition(controller, "down"));

            animator.Controller = controller;

            hero.AddComponent(animator);

            #endregion

            hero.AddComponent(new Rigidbody() { FixedRotation = true });
            hero.AddComponent(new BoxCollider() { Size = new Vector2(30) });
            hero.AddComponent(new HeroController());


            Container legsGO = new Container();
            legsGO.Name = "Hero legs";
            legsGO.AddComponent(new SpriteRenderer() { Sprite = sprite[56] });

            Animator legs = new Animator();
            legs.Controller = controller;

            Animation UpDownLegs = new Animation(0, 0.05f, sprite[56], sprite[57], sprite[58], sprite[59]);
            legs.Animations.Add("down", UpDownLegs);
            legs.Animations.Add("up", UpDownLegs);

            Animation rightLegs = new Animation(0, 0.05f, sprite[41], sprite[42]);
            legs.Animations.Add("right", rightLegs);

            Animation leftLegs = new Animation(0, 0.05f, sprite[26], sprite[27]);
            legs.Animations.Add("left", leftLegs);

            legsGO.AddComponent(legs);


            hero.AddContainer(legsGO);

            hero.AddComponent(new PrefabsMenuRenderer()
            {
                Prefabs = new List<IPrefabView>()
                {
                    new TilePrefab(){ Name = "Tiles/hero1", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero2", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero3", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero4", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero5", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero6", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero7", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero8", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero9", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero10", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero11", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero12", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero13", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero14", Preview =sprite[48] },
                    new TilePrefab(){ Name = "Tiles/hero15", Preview =sprite[48] },
                }
            });

            return hero;
        }


        private Container CreateLevel()
        {
            var sprite = Content.Load<Sprite>("Sprites/pall");

            Tileset floors = new Tileset();
            floors.Add(new Tile(sprite[0]));
            floors.Add(new Tile(sprite[1]));
            floors.Add(new Tile(sprite[2]));
            floors.Add(new Tile(sprite[3]));
            floors.Add(new Tile(sprite[4]));
            floors.Add(new Tile(sprite[5]));

            Container level = new Container();
            level.AddComponent(new Transformer(100, 100));
            level.Name = "Level";

            Container floorsGO = new Container();
            floorsGO.Name = "Floor";

            level.AddContainer(floorsGO);

            Tilemap floorTilemap = new Tilemap();
            floorTilemap.CellOrigin = new Vector2(16);
            floorTilemap.AddTile(new Point(0), floors[0]);
            floorTilemap.AddTile(new Point(1), floors[1]);
            floorTilemap.AddTile(new Point(2), floors[2]);
            floorTilemap.AddTile(new Point(3), floors[3]);

            floorsGO.AddComponent(floorTilemap);
            floorsGO.AddComponent(new TilemapCollider());

            return level;
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

            Transformer transformer = _gameManager.GetComponent<Transformer>();
            Transform2D local = transformer.Local;

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                IUpdateable updateable = _gameManager as IUpdateable;
                _gameManager.Enabled = !_gameManager.Enabled;
            }

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

            transformer.Local = local;

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
