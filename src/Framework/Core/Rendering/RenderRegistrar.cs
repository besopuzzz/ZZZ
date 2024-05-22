using ZZZ.Framework.Core.Registrars;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering
{
    public class RenderRegistrar : BaseRegistrar<IGraphics>, IOnlyEnabledRegistrar<IGraphics>
    {
        private readonly List<ICamera> cameras;
        private readonly RenderManager renderManager;
        private readonly RenderComponents components;

        public RenderRegistrar()
        {
            cameras = new List<ICamera>();
            components = new RenderComponents();
            renderManager = new RenderManager();
        }

        protected override void Initialize()
        {
            renderManager.Initialize(GameManager.Game.GraphicsDevice);

            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components.Clear();

            base.Dispose(disposing);
        }

        void IOnlyEnabledRegistrar<IGraphics>.EnabledReception(IGraphics component)
        {
            switch (component)
            {
                case ICamera camera:
                    cameras.Add(camera);
                    break;
                case IRender render:
                    components.Add(render);
                    break;
                default:
                    break;
            }
        }

        void IOnlyEnabledRegistrar<IGraphics>.EnabledDeparture(IGraphics component)
        {
            switch (component)
            {
                case ICamera camera:
                    cameras.Remove(camera);
                    break;
                case IRender render:
                    components.Remove(render);
                    break;
                default:
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var item in cameras)
            {
                item.UpdateMatrix();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            renderManager.SetGameTime(gameTime);

            foreach (var camera in cameras)
            {
                components.Render(camera, renderManager);
            }
        }
    }
}
