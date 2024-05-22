using nkast.Aether.Physics2D.Diagnostics;
using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Physics.Components;

namespace ZZZ.Framework.Diagnostics.Components
{
    public class WorldRenderer : BaseRegistrar<IComponent>
    {
        private DebugView debugView;
        private WorldRegistrar worldController;
        private Matrix projection;
        private GraphicsDevice device;
        private RenderRegistrar renderRegistrar;
        private Camera camera;

        public WorldRenderer()
        {
        }

        protected override void Initialize()
        {
            worldController = base.GameManager.Registrars.FirstOrDefault(x => x is WorldRegistrar) as WorldRegistrar;

            if (worldController == null)
                return;

            camera = SceneLoader.CurrentScene.FindComponent<Camera>();

            device = GameManager.Game.GraphicsDevice;
            device.DeviceResetting += Device_DeviceResetting;

            debugView = new DebugView(worldController.World);
            debugView.Flags = DebugViewFlags.CenterOfMass | DebugViewFlags.Joint | DebugViewFlags.Shape;
            debugView.LoadContent(device, GameManager.Game.Content);

            SetProject(device.Viewport.Bounds.Size);
        }

        private void Device_DeviceResetting(object sender, EventArgs e)
        {
            SetProject(device.Viewport.Bounds.Size);
        }

        public void Shoutdown()
        {
            device.DeviceResetting -= Device_DeviceResetting;
        }

        private void SetProject(Point size)
        {
            projection = Matrix.CreateOrthographicOffCenter(0, size.X, size.Y, 0, 0, 1);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (worldController == null)
                return;

            var trans = Matrix.CreateScale(new Vector3(Vector2.One * IRigidbody.PixelsPerMeter, 1f));

            debugView.RenderDebugData(projection, trans * (camera == null ? Matrix.Identity : camera.Projection));

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                debugView?.Dispose();
            }

            debugView = null;
            worldController = null;
            device = null;

            base.Dispose(disposing);
        }

    }
}
