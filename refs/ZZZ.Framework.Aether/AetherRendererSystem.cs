using nkast.Aether.Physics2D.Diagnostics;
using ZZZ.Framework.Aether;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Components;

namespace ZZZ.Framework.Physics.Aether
{
    [RequiredComponent<AetherSystem>]
    public class AetherRendererSystem : System, ISystemRenderer
    {
        private AetherSystem aetherSystem;
        private DebugView debugView;
        private GraphicsDevice device;

        public void Render()
        {
            var viewport = device.Viewport;

            var view = Matrix.CreateScale(new Vector3(Vector2.One * PhysicalBody.PixelsPerMeter, 1f));

            var projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 10);

            debugView.RenderDebugData(projection, view * Camera.MainCamera.View.GetMatrix());
        }

        protected override void Awake()
        {
            aetherSystem = GetComponent<AetherSystem>();

            var services = Services.Get<IServiceProvider>();

            device = (services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager).GraphicsDevice;

            debugView = new DebugView(aetherSystem.World);
            debugView.Flags = DebugViewFlags.CenterOfMass | DebugViewFlags.Joint | DebugViewFlags.Shape | DebugViewFlags.ContactPoints;
            debugView.LoadContent(device, new ContentManager(services, "Content"));

            base.Awake();
        }
    }
}
