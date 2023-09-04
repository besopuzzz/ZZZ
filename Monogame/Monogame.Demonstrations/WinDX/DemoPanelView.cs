using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework;
using ZZZ.Framework.Monogame;
using ZZZ.Framework.Monogame.Animations;
using ZZZ.Framework.Monogame.Animations.Assets;
using ZZZ.Framework.Monogame.Animations.Components;
using ZZZ.Framework.Monogame.Animations.Parameters;
using ZZZ.Framework.Monogame.Auding.Assets;
using ZZZ.Framework.Monogame.Auding.Components;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.Rendering.Components;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Tiling.Assets;
using ZZZ.Framework.Monogame.Tiling.Components;
using ZZZ.Framework.Monogame.Tiling.Components.Physics;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace WinDX
{
    public class DemoPanelView : UpdateComponent
    {
        public DemoMode Mode
        {
            get => demoMode;
            private set
            {
                if (demoMode == value)
                    return;

                demoMode = value;
            }
        }

        private TextRenderer currentMode;
        private TextRenderer currentControlText;
        private DemoMode demoMode = DemoMode.Animating;
        private Container hero = new Container();
        private Container tiling = new Container();
        private Container auding = new Container();
        private Container asseting = new Container();
        private TilemapCollider tilemapCollider;
        private SceneTransformerController sceneTransformerController;

        public DemoPanelView()
        {

        }

        protected override void Startup()
        {
            var font = new Font(AssetManager.Load<SpriteFont>("DiagnosticsFont"));
            currentMode = new TextRenderer(font);
            currentControlText = new TextRenderer(font) { Offset = new Vector2(0, 15) };

            AddComponent(currentMode);
            AddComponent(currentControlText);

            #region Animating

            var robotSprite = AssetManager.Load<Sprite>("Sprites/robot");

            hero.Name = "Hero";
            hero.AddComponent(new SpriteRenderer() { Sprite = robotSprite[48] });

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


            animator.Animations.Add("down", new Animation(0f, 1f, robotSprite[48]));

            //// Left

            left.Transitions.Add(new Transition(controller, "down",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));
            left.Transitions.Add(new Transition(controller, "right",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(1))));
            left.Transitions.Add(new Transition(controller, "up",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));

            animator.Animations.Add("left", new Animation(0f, 1f, robotSprite[18]));


            //// Right

            right.Transitions.Add(new Transition(controller, "down",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));
            right.Transitions.Add(new Transition(controller, "left",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(-1))));
            right.Transitions.Add(new Transition(controller, "up",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(0))));

            animator.Animations.Add("right", new Animation(0f, 1f, robotSprite[34]));

            //// Top

            up.Transitions.Add(new Transition(controller, "right",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(1)),
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(0))));
            up.Transitions.Add(new Transition(controller, "left",
                new Condition(controller, "velocityX", ParameterCondition.Equals, new Int32Parameter(-1)),
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(0))));
            up.Transitions.Add(new Transition(controller, "down",
                new Condition(controller, "velocityY", ParameterCondition.Equals, new Int32Parameter(1))));

            animator.Animations.Add("up", new Animation(0f, 1f, robotSprite[4]));


            controller.StartStage.Transitions.Add(new Transition(controller, "down"));

            animator.Controller = controller;

            hero.AddComponent(animator);

            #endregion

            hero.AddComponent(new Rigidbody() { FixedRotation = false });
            hero.AddComponent(new BoxCollider() { Size = new Vector2(20, 30) });
            hero.AddComponent(new HeroController());
            hero.AddComponent(new SoundListener());


            Container legsGO = new Container();
            legsGO.Name = "Hero legs";
            legsGO.AddComponent(new SpriteRenderer() { Sprite = robotSprite[56] });

            Animator legs = new Animator();
            legs.Controller = controller;

            Animation UpDownLegs = new Animation(0, 0.05f, robotSprite[56], robotSprite[57], robotSprite[58], robotSprite[59]);
            legs.Animations.Add("down", UpDownLegs);
            legs.Animations.Add("up", UpDownLegs);

            Animation rightLegs = new Animation(0, 0.05f, robotSprite[41], robotSprite[42]);
            legs.Animations.Add("right", rightLegs);

            Animation leftLegs = new Animation(0, 0.05f, robotSprite[26], robotSprite[27]);
            legs.Animations.Add("left", leftLegs);

            legsGO.AddComponent(legs);
            hero.AddContainer(legsGO);

            AddContainer(hero);


            #endregion

            #region Tiling

            var pallSprite = AssetManager.Load<Sprite>("Sprites/pall");

            Tileset floors = new Tileset();
            floors.Add(new Tile(pallSprite[0]));
            floors.Add(new AnimatedTile(1, 0, pallSprite[0], pallSprite[1], pallSprite[2], pallSprite[3], pallSprite[4], pallSprite[5]));
            floors.Add(new Tile(pallSprite[2]));
            floors.Add(new Tile(pallSprite[3]));
            floors.Add(new AnimatedTile(1, 0, pallSprite[0], pallSprite[1], pallSprite[2], pallSprite[3], pallSprite[4], pallSprite[5]));
            floors.Add(new Tile(pallSprite[5]));

            tiling.AddComponent(new Transformer(100, 100));
            tiling.Name = "Level";

            Container floorsGO = new Container();
            floorsGO.Name = "Floor";

            tiling.AddContainer(floorsGO);

            Tilemap floorTilemap = new Tilemap();
            floorTilemap.CellOrigin = new Vector2(16);
            floorTilemap.AddTile(new Point(0,0), floors[0]);
            floorTilemap.AddTile(new Point(1,0), floors[0]);
            floorTilemap.AddTile(new Point(2,0), floors[0]);
            floorTilemap.AddTile(new Point(3,0), floors[0]);

            floorTilemap.AddTile(new Point(2, 1), floors[1]);
            floorTilemap.AddTile(new Point(1,2), floors[1]);

            floorTilemap.AddTile(new Point(0, 3), floors[2]);
            floorTilemap.AddTile(new Point(1, 3), floors[2]);
            floorTilemap.AddTile(new Point(2, 3), floors[2]);
            floorTilemap.AddTile(new Point(3, 3), floors[2]);

            floorsGO.AddComponent(floorTilemap);
            tilemapCollider = floorsGO.AddComponent(new TilemapCollider());

            AddContainer(tiling);

            #endregion

            auding.AddComponent(new TextRenderer(font)).Text.Append("Sound emmiter here!");
            auding.AddComponent(new SoundEmitter() { Sound = new Sound(AssetManager.Load<SoundEffect>("Musics/kalambur")), IsLooped = true});

            AddContainer(auding);

            sceneTransformerController = AddComponent(new SceneTransformerController());

            asseting.AddComponent(new PrefabCreaterComponent() { Prefab = hero, DropContainer = Owner });

            AddContainer(asseting);

            ChangeMode(demoMode);

            base.Startup();
        }



        private void ChangeMode(DemoMode mode)
        {
            DisableAll();
            hero.GetComponent<Transformer>().Local = new Transform2D(10, 10);
            StringBuilder text = currentControlText.Text;
            text.Clear();

            text.AppendLine("Use keys WASD to control character.");

            switch (mode)
            {
                case DemoMode.Animating:
                    text.AppendLine("Character animation depends on the direction.");
                    break;
                case DemoMode.Tiling:
                    text.AppendLine("Tiles have a static and animated appearance.");
                    tiling.Enabled = true;
                    break;
                case DemoMode.FarseerPhysics:
                    text.AppendLine("Farseer physics also works on Tilemap.");
                    tiling.Enabled = true;
                    tilemapCollider.Enabled = true;
                    break;
                case DemoMode.Auding:
                    text.AppendLine("The sound volume level depends on the distance of the character.");
                    auding.Enabled = true;
                    break;
                case DemoMode.Transforming:
                    text.AppendLine("Use arrows to move scene.");
                    text.AppendLine("Use X and Z to rotate scene.");
                    text.AppendLine("Use Plus and Minus to scale scene.");
                    sceneTransformerController.Enabled = true;
                    break;
                case DemoMode.Asseting:
                    asseting.Enabled = true;
                    break;
                default:
                    break;
            }

            currentMode.Text.Clear();
            currentMode.Text.Append($"Mode: {mode}");
        }

        private void DisableAll()
        {
            tiling.Enabled = false;
            tilemapCollider.Enabled = false;
            auding.Enabled = false;
            asseting.Enabled = false;
            sceneTransformerController.Enabled = false;
        }

        private KeyboardState keyboardState;
        private KeyboardState oldkeyboardState;

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Tab) & oldkeyboardState.IsKeyUp(Keys.Tab))
            {
                var typeEnum = typeof(DemoMode);
                var names = Enum.GetNames(typeEnum);

                int index = Array.IndexOf(names, Enum.GetName(typeEnum, demoMode));

                ++index;

                if (index >= names.Length)
                    index = 0;

                Mode = (DemoMode)Enum.Parse(typeEnum, names[index]);
                ChangeMode(Mode);
            }

            oldkeyboardState = keyboardState;

            base.Update(gameTime);
        }

    }

    public enum DemoMode
    {
        Animating,
        Tiling,
        FarseerPhysics,
        Auding,
        Transforming,
        Asseting
    }
}
