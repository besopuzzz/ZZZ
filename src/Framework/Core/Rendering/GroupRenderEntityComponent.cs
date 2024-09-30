using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using static ZZZ.Framework.Core.Rendering.Entities.RenderQueue;

namespace ZZZ.Framework.Core.Rendering
{
    public class GroupRenderEntityComponent : RenderEntityComponent
    {
        private SpriteBatch localBatch;
        private RenderQueue localQueue;

        public GroupRenderEntityComponent(IGroupRender component) : base(component)
        {
            localQueue = new RenderQueue();
        }

        public override void Render(ICamera camera, SpriteBatch spriteBatch)
        {
            if (localBatch == null)
                localBatch = new SpriteBatch(spriteBatch.GraphicsDevice);

            Owner.Childs.ForEach(x => x.ForEveryComponent((component) =>
            {
                if (component.Enabled)
                    localQueue.Add(component);
            }));

            localQueue.Render(camera, localBatch, RenderMode.ToOneLayer);
        }
    }
}
