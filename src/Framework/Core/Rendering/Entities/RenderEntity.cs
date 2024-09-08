using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public class RenderEntity : Entity<IRender, RenderEntity>
    {
        public GraphicsDevice GraphicsDevice => device;
        public SortLayer SortLayer => Component.Layer;
        public int Order => Component.Order;

        private GraphicsDevice device;

        public RenderEntity(GraphicsDevice graphicsDevice, IRender render) : base(render)
        {
            device = graphicsDevice;
        }

        protected virtual void Render(SpriteBatch spriteBatch, ICamera camera)
        {
            Component.Render(spriteBatch);
        }

        protected virtual void PrepareToRender(RenderQueue queue)
        {
            if (!Enabled)
                return;

            queue.Add(this);
            queue.Add(GetEntities());
        }

        protected override void Dispose(bool disposing)
        {
            device = null;

            base.Dispose(disposing);
        }

        internal void InternalRender(SpriteBatch spriteBatch, ICamera camera)
        {
            Render(spriteBatch, camera);
        }

        internal void RegisterToRender(RenderQueue queue)
        {
            PrepareToRender(queue);
        }

        public override string ToString()
        {
            return $"Type: {Component.ToString()}, Enabled: {Component.Enabled}, Layer: {Component.Layer}";
        }
    }
}
