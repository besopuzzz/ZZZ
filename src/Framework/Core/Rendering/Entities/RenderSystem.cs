using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public class RenderSystem : System<IRender, RenderEntity>
    {
        public ICamera MainCamera => Camera.MainCamera;

        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private EntityRenderer entityRenderer = new EntityRenderer();

        public RenderSystem()
        {

        }

        protected override void Initialize()
        {
            graphicsDevice = Game.Services.GetService<IGraphicsDeviceService>().GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (MainCamera != null && MainCamera.Enabled)
                MainCamera.UpdateMatrix();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var camera = Camera.MainCamera;

            if (camera == null || !camera.Enabled)
                return;

            entityRenderer.Prepare(Entities);
            entityRenderer.Render(camera, spriteBatch, EntityRenderer.RenderMode.ToEveryLayer);

            base.Draw(gameTime);
        }

        protected override RenderEntity OnProcess(IRender component)
        {
            if (component is IGroupRender group)
                return new GroupEntity(graphicsDevice, group);

            return new RenderEntity(graphicsDevice, component);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                ((IDisposable)entityRenderer).Dispose();
                spriteBatch.Dispose();
            }

            entityRenderer = null;
            spriteBatch = null;
            graphicsDevice = null;

            base.Dispose(disposing);
        }
    }
}
