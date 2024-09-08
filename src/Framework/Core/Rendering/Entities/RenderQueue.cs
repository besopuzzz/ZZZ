using ZZZ.Framework.Core.Rendering.Components;
using static ZZZ.Framework.Core.Rendering.Entities.EntityRenderer;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public sealed class RenderQueue
    {
        private readonly Dictionary<SortLayer, List<RenderEntity>> entities = new Dictionary<SortLayer, List<RenderEntity>>();
        private static Comparer<int> comparer = Comparer<int>.Default;

        internal RenderQueue()
        {
            foreach (var sortLayer in Enum.GetValues<SortLayer>())
                entities.Add(sortLayer, new List<RenderEntity>());
        }

        public void Add(RenderEntity entity)
        {
            entities[entity.SortLayer].Add(entity);
        }

        public void Add(IEnumerable<RenderEntity> entities)
        {
            foreach (RenderEntity child in entities)
                child.RegisterToRender(this);
        }

        private void SortAndRender(ICamera camera, List<RenderEntity> entitiesToRender, SpriteBatch spriteBatch)
        {
            entitiesToRender.Sort((x, y) => comparer.Compare(x.Order, y.Order));

            foreach (var entity in entitiesToRender)
                entity.InternalRender(spriteBatch, camera);
        }

        internal void Reset()
        {
            foreach (var entity in entities)
                entity.Value.Clear();
        }

        internal void Clear()
        {
            entities.Clear();
        }

        internal void Render(ICamera camera, SpriteBatch spriteBatch, RenderMode renderMode)
        {
            if (renderMode == RenderMode.ToOneLayer)
            {
                camera.Render(spriteBatch);

                foreach (var layer in entities.Values)
                {
                    SortAndRender(camera, layer, spriteBatch);
                }

                spriteBatch.End();
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (!camera.Layer.HasFlag(entity.Key))
                        continue;

                    camera.Render(spriteBatch);

                    SortAndRender(camera, entities[entity.Key], spriteBatch);

                    spriteBatch.End();
                }
            }

            Reset();
        }

    }
}
