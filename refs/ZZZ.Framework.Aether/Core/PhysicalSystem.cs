using nkast.Aether.Physics2D.Diagnostics;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Physics.Aether.Components;
using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Rendering;

namespace ZZZ.Framework.Aether.Core
{
    public sealed class PhysicalSystem : System<PhysicalEntity, PhysicalEntityComponent, IRigidbody>
    {
        public World World { get; } = new World(Vector2.Zero);

        private DebugView debugView;
        private Matrix projection;
        private GraphicsDevice device;
        private Camera camera;

        public PhysicalSystem()
        {
        }

        protected override PhysicalEntity CreateEntity(PhysicalEntity owner)
        {
            return new PhysicalEntity(World);
        }
        
        protected override PhysicalEntityComponent CreateEntityComponent(PhysicalEntity owner, IRigidbody component)
        {
            return new PhysicalEntityComponent(component);
        }

        protected override void Update(GameTime gameTime)
        {
            //ForEveryChild(x => x.UpdateBody());

            World.Step(gameTime.ElapsedGameTime);

            //ForEveryChild(x => x.UpdateTransformer());

            base.Update(gameTime);
        }

        protected override void Initialize()
        {
            camera = SceneLoader.CurrentScene.FindComponent<Camera>();

            device = GameSettings.Instance.Game.Services.GetService<IGraphicsDeviceService>().GraphicsDevice;
            device.DeviceResetting += Device_DeviceResetting;

            debugView = new DebugView(World);
            debugView.Flags = DebugViewFlags.CenterOfMass | DebugViewFlags.Joint | DebugViewFlags.Shape | DebugViewFlags.ContactPoints;
            debugView.LoadContent(device, new ContentManager(this.Game.Services, "Content"));

            SetProject(device.Viewport.Bounds.Size);
        }

        private void Device_DeviceResetting(object sender, EventArgs e)
        {
            SetProject(device.Viewport.Bounds.Size);
        }

        private void SetProject(Point size)
        {
            projection = Matrix.CreateOrthographicOffCenter(0, size.X, size.Y, 0, 0, 1);
        }

        protected override void Draw(GameTime gameTime)
        {
            var trans = Matrix.CreateScale(new Vector3(Vector2.One * PhysicalBody.PixelsPerMeter, 1f));

            debugView.RenderDebugData(Camera.MainCamera.Projection, Camera.MainCamera.View, trans * Camera.MainCamera.World);

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                debugView?.Dispose();
            }

            debugView = null;
            device = null;

            base.Dispose(disposing);
        }
    }
}
