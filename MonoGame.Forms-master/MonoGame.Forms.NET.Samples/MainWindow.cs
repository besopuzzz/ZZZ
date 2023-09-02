using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
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
using Point = Microsoft.Xna.Framework.Point;

namespace MonoGame.Forms.NET.Samples
{
    public partial class MainWindow : Form
    {
        #region Welcome

        #endregion

        #region Invalidation Control

        private void textBoxTestText_TextChanged(object sender, System.EventArgs e)
        {
            invalidationTestControl.WelcomeMessage = textBoxTestText.Text;
        }

        private void buttonInvalidate_Click(object sender, EventArgs e)
        {
            invalidationTestControl.Invalidate();
        }

        #endregion

        #region Update Window

        bool CamButtonMouseDown = false;
        System.Drawing.Point CamButtonFirstMouseDownPosition;


        private void monoGameTestControl_VisibleChanged(object sender, EventArgs e)
        {
            trackBarCamZoom.Value = (int)monoGameTestControl.Editor.GetCamZoom()!.Value;
        }

        private void buttonMoveCam_MouseUp(object sender, MouseEventArgs e)
        {
            CamButtonMouseDown = false;
        }

        private void buttonMoveCam_MouseDown(object sender, MouseEventArgs e)
        {
            CamButtonFirstMouseDownPosition = e.Location;
            CamButtonMouseDown = true;
        }

        private void buttonMoveCam_MouseMove(object sender, MouseEventArgs e)
        {            
            if (CamButtonMouseDown)
            {
                int xDiff = CamButtonFirstMouseDownPosition.X - e.Location.X;
                int yDiff = CamButtonFirstMouseDownPosition.Y - e.Location.Y;

                monoGameTestControl.Editor.CamMove(new Vector2(xDiff, yDiff));

                CamButtonFirstMouseDownPosition.X = e.Location.X;
                CamButtonFirstMouseDownPosition.Y = e.Location.Y;
            }
        }

        private void buttonResetCam_Click(object sender, System.EventArgs e)
        {
            monoGameTestControl.Editor.ResetCam();
            trackBarCamZoom.Value = (int)monoGameTestControl.Editor.GetCamZoom()!.Value;
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please press a mouse button directly on the control to test if the different mouse events are working correctly." + Environment.NewLine + Environment.NewLine + "The mouse events are directly delivered to the corresponding classes, so it becomes very easy to work with them in your custom editor!", "Mouse Events 101", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void checkBoxFPS_CheckedChanged(object sender, System.EventArgs e)
        {
            if (monoGameTestControl.Editor.FPSCounter != null)
            {
                monoGameTestControl.Editor.FPSCounter.ShowFPS = checkBoxFPS.Checked;
            }
        }

        private void checkBoxCursor_CheckedChanged(object sender, System.EventArgs e)
        {
            if (monoGameTestControl.Editor.FPSCounter != null)
            {
                monoGameTestControl.Editor.FPSCounter.ShowCursorPosition = checkBoxCursor.Checked;
            }
        }

        private void checkBoxCam_CheckedChanged(object sender, System.EventArgs e)
        {
            if (monoGameTestControl.Editor.FPSCounter != null)
            {
                monoGameTestControl.Editor.FPSCounter.ShowCamPosition = checkBoxCam.Checked;
            }
        }

        private void trackBarCamZoom_Scroll(object sender, System.EventArgs e)
        {
            monoGameTestControl.Editor.CamZoom(1 - ((float)trackBarCamZoom.Value / 10f));
        }

        #endregion

        #region Multiple Controls

        private void buttonHelpControls_Click(object sender, EventArgs e)
        {
            MessageBox.Show("[Left Mouse Button] Move Cam\n[Right Mouse Button] Debug Display\n[Middle Mouse Button] Reset Cam\n[XButton1] Previous Map\n[XButton2] Next Map\n[Mouse Wheel] Zoom Cam\n\nImages copyright (c) by FinTerra\nTile Art copyright (c) by Pixel32\n\nOpenGameArt.org\n\nAttribution 3.0 Unported (CC BY 3.0)", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void splitContainerMapHost_VisibleChanged(object sender, EventArgs e)
        {
            splitContainerMapHost.SplitterDistance = (int)(splitContainerMapHost.ClientSize.Width * 0.5f);
        }

        #endregion

        #region Advanced Input

        private void buttonResetPlayer_Click(object sender, EventArgs e)
        {
            advancedControlsTest.ResetPlayer();
        }

        private void buttonHelpInput_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please plug in your GamePad if you want to switch from Keyboard to GamePad input.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBoxShowStats_CheckedChanged(object sender, EventArgs e)
        {
            advancedControlsTest.ShowStats = checkBoxShowStats.Checked;
        }

        private void checkBoxShowHelp_CheckedChanged(object sender, EventArgs e)
        {
            advancedControlsTest.ShowControls = checkBoxShowHelp.Checked;
        }

        #endregion

        #region Info

        private void toolStripDropDownButtonGitHub_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/BlizzCrafter/MonoGame.Forms") { UseShellExecute = true });
        }

        private void toolStripDropDownButtonWiki_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/BlizzCrafter/MonoGame.Forms/wiki") { UseShellExecute = true });
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            

            Text = "MonoGame.Forms.NET";
        }


        private Camera camera;
        private Scene _gameManager;

        protected void Initialize()
        {
            _gameManager = new Scene(this.welcomeControl.Editor.Content.ServiceProvider);
            _gameManager.Name = "Scene";
            _gameManager.UseZZZ();
            _gameManager.AddChild(CreateLevel());
            _gameManager.AddChild(CreateHero());


            //_gameManager = Scene.FromAsset("Scene1", Content.ServiceProvider);
            //_gameManager.Enabled = false;

            ((IGameComponent)_gameManager).Initialize();

        }

        protected override void OnLoad(EventArgs e)
        {
            Initialize();

            base.OnLoad(e);
        }

        private GameObject CreateHero()
        {
            var sprite = this.welcomeControl.Editor.Content.Load<Sprite>("Sprites/robot");

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
            var sprite = this.welcomeControl.Editor.Content.Load<Sprite>("Sprites/pall");

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

    }
}