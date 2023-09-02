using tainicom.Aether.Physics2D.Diagnostics;
using ZZZ.Framework.Monogame.Content;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Diagnostics.Components
{
    internal class WorldRenderer : Disposable
    {
        private DebugView debugView;
        private WorldController worldController;
        private Matrix projection;
        private Camera camera;
        private GraphicsDevice device;

        public void Startup(Game game, WorldController controller)
        {
            device = ((IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;
            device.DeviceResetting += Device_DeviceResetting;

            camera = controller.Owner.FindComponent<Camera>();
            worldController = controller.Owner.FindComponent<WorldController>();

            debugView = new DebugView(worldController.World);
            debugView.Flags = DebugViewFlags.CenterOfMass | DebugViewFlags.Joint | DebugViewFlags.Shape;
            debugView.LoadContent(device, AssetManager.Instance);

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

        public void Draw(GameTime gameTime)
        {
            debugView.RenderDebugData(projection, camera?.Projection ?? Matrix.Identity);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                debugView.Dispose();
            }

            debugView = null;
            worldController = null;
            camera = null;
            device = null;

            base.Dispose(disposing);
        }
    }
}
