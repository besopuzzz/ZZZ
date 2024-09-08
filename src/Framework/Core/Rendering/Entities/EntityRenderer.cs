using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public sealed class EntityRenderer : Disposable
    {
        private RenderQueue renderQueue;

        public enum RenderMode
        {
            ToOneLayer,
            ToEveryLayer
        }

        public EntityRenderer() 
        {
            renderQueue = new RenderQueue();
        }

        public void Prepare(IEnumerable<RenderEntity> renderEntities)
        {
            foreach (var renderEntity in renderEntities)
                renderEntity.RegisterToRender(renderQueue);
        }

        public void Render(ICamera camera, SpriteBatch spriteBatch, RenderMode renderMode)
        {
            renderQueue.Render(camera, spriteBatch, renderMode);
        }

        public void Reset()
        {
            renderQueue.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                renderQueue.Clear();

            renderQueue = null;

            base.Dispose(disposing);
        }
    }
}
