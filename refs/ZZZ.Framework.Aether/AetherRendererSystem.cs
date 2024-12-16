using nkast.Aether.Physics2D.Diagnostics;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Aether;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Rendering.Components;

namespace ZZZ.Framework.Components.Physics.Aether
{
    [RequiredComponent<AetherSystem>]
    public class AetherRendererSystem : System, IDrawable, IGameComponent
    {
        public int DrawOrder
        {
            get
            {
                return order;
            }

            set
            {
                if (order == value)
                    return;

                order = value;

                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Visible
        {
            get
            {
                return base.Enabled;
            }

            set
            {
                if (Enabled == value)
                    return;

                Enabled = value;

                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        private int order = 0;
        private AetherSystem aetherSystem;
        private DebugView debugView;
        private GraphicsDevice device;

        void IDrawable.Draw(GameTime gameTime)
        {
            var trans = Matrix.CreateScale(new Vector3(Vector2.One * PhysicalBody.PixelsPerMeter, 1f));

            debugView.RenderDebugData(Camera.MainCamera.Projection, Camera.MainCamera.View, trans * Camera.MainCamera.World);

        }

        void IGameComponent.Initialize()
        {

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
