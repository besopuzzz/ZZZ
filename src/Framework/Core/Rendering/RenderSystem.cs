using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using static ZZZ.Framework.Core.Rendering.Entities.RenderQueue;

namespace ZZZ.Framework.Core.Rendering
{
    public class RenderSystem : System<RenderEntity, RenderEntityComponent, IRender>
    {
        public ICamera MainCamera => Camera.MainCamera;

        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private RenderQueue localQueue = new RenderQueue();

        protected override RenderEntity CreateEntity(RenderEntity owner)
        {
            return new RenderEntity();
        }

        protected override RenderEntityComponent CreateEntityComponent(RenderEntity owner, IRender component)
        {
            if (component is IGroupRender group)
                return new GroupRenderEntityComponent(group);

            return new RenderEntityComponent(component);
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

            ForEveryComponent(component =>
            {
                if (camera.LayerMask.HasFlag(component.SortLayer) && component.Enabled)
                    localQueue.Add(component);
            });

            localQueue.Render(camera, spriteBatch, RenderMode.ToEveryLayer);

            base.Draw(gameTime);
        }

    }
}
