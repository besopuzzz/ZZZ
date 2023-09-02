using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.NET.Controls;
using MonoGame.Forms.NET.Samples.Utils;
using ZZZ.Framework;
using ZZZ.Framework.Animations;
using ZZZ.Framework.Animations.Components;
using ZZZ.Framework.Animations.Content;
using ZZZ.Framework.Animations.Parameters;
using ZZZ.Framework.Audio;
using ZZZ.Framework.Content;
using ZZZ.Framework.Extentions;
using ZZZ.Framework.FarseerPhysics.Components;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Components;
using ZZZ.Framework.Rendering.Content;
using ZZZ.Framework.Tiled.Components;
using ZZZ.Framework.Tiled.Components.Physics;
//using ZZZ.Framework.Tiled.Components.Physics;
using ZZZ.Framework.Tiled.Components.Rendering;
using ZZZ.Framework.Tiled.Content;
using ZZZ.Framework.Transforming;
using ZZZ.Framework.Transforming.Components;
using Animation = ZZZ.Framework.Animations.Content.Animation;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGame.Forms.NET.Samples.Tests
{
    public class Welcome : MonoGameControl
    {
        string WelcomeMessage = "Welcome to MonoGame.Forms!";

        private Camera camera;
        private Scene _gameManager;

        protected override void Initialize()
        {
            _gameManager = new Scene(Services);
            _gameManager.Name = "Scene";
            _gameManager.UseZZZ();
            _gameManager.AddChild(CreateLevel());
            _gameManager.AddChild(CreateHero());


            //_gameManager = Scene.FromAsset("Scene1", Content.ServiceProvider);
            //_gameManager.Enabled = false;

            Components.Add(_gameManager);

        }



        private GameObject CreateHero()
        {
            var sprite = AssetManager.Instance.Load<Sprite>("Sprites/robot");

            GameObject hero = new GameObject();
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


            GameObject legsGO = new GameObject();
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


            hero.AddChild(legsGO);

            return hero;
        }
        private GameObject CreateLevel()
        {
            var sprite = AssetManager.Instance.Load<Sprite>("Sprites/pall");

            Tileset floors = new Tileset();
            floors.Add(new Tile(sprite[0]));
            floors.Add(new Tile(sprite[1]));
            floors.Add(new Tile(sprite[2]));
            floors.Add(new Tile(sprite[3]));
            floors.Add(new Tile(sprite[4]));
            floors.Add(new Tile(sprite[5]));

            GameObject level = new GameObject(new Transform2D(new Vector2(100)));
            level.Name = "Level";

            GameObject floorsGO = new GameObject();
            floorsGO.Name = "Floor";

            level.AddChild(floorsGO);

            Tilemap floorTilemap = new Tilemap();
            floorTilemap.CellSize = new Vector2(32);
            floorTilemap.CellOrigin = new Vector2(16);
            floorTilemap.AddTile(new Point(0), floors[0]);
            floorTilemap.AddTile(new Point(1), floors[1]);
            floorTilemap.AddTile(new Point(2), floors[2]);
            floorTilemap.AddTile(new Point(3), floors[3]);

            floorsGO.AddComponent(floorTilemap);
            floorsGO.AddComponent(new TilemapRenderer());
            floorsGO.AddComponent(new TilemapCollider());

            return level;
        }

        protected override void Update(GameTime gameTime) {
        
        
        }

        protected override void Draw()
        {
            
        }
    }
}
