using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public class GroupEntity : RenderEntity
    {
        private SpriteBatch localBatch;
        private EntityRenderer localRenderer;

        public GroupEntity(GraphicsDevice graphicsDevice, IGroupRender render) : base(graphicsDevice, render)
        {
            localBatch = new SpriteBatch(graphicsDevice);
            localRenderer = new EntityRenderer();
        }

        protected override void Render(SpriteBatch spriteBatch, ICamera camera)
        {
            localRenderer.Render(camera, localBatch, EntityRenderer.RenderMode.ToOneLayer);
        }

        protected override void PrepareToRender(RenderQueue queue)
        {
            if (!Enabled)
                return;

            queue.Add(this);

            localRenderer.Reset();
            localRenderer.Prepare(base.GetEntities());
        }

        protected override IEnumerable<RenderEntity> GetEntities()
        {
            return Enumerable.Empty<RenderEntity>();
        }
    }
}
