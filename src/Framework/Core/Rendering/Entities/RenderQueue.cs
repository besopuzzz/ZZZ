using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public sealed class RenderQueue
    {
        public enum RenderMode
        {
            ToOneLayer,
            ToEveryLayer
        }

        private readonly Dictionary<SortLayer, List<RenderEntityComponent>> entities = new Dictionary<SortLayer, List<RenderEntityComponent>>();

        internal RenderQueue()
        {
            foreach (var sortLayer in Enum.GetValues<SortLayer>())
                entities.Add(sortLayer, new List<RenderEntityComponent>());
        }

        public void Add(RenderEntityComponent entity)
        {
            entities[entity.SortLayer].Add(entity);
        }

        private void SortAndRender(ICamera camera, List<RenderEntityComponent> entitiesToRender, SpriteBatch spriteBatch)
        {
            entitiesToRender.Sort();

            foreach (var entity in entitiesToRender)
                entity.Render(camera, spriteBatch);
        }

        internal void Reset()
        {
            foreach (var entity in entities)
                entity.Value.Clear();
        }

        internal void Render(ICamera camera, SpriteBatch spriteBatch, RenderMode renderMode)
        {
            if (renderMode == RenderMode.ToOneLayer)
            {
                //camera.Apply(spriteBatch);

                foreach (var entity in entities)
                {
                    SortAndRender(camera, entity.Value, spriteBatch);

                    entity.Value.Clear();
                }

                spriteBatch.End();
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (!camera.LayerMask.HasFlag(entity.Key))
                        continue;

                    //camera.Apply(spriteBatch);

                    SortAndRender(camera, entities[entity.Key], spriteBatch);

                    spriteBatch.End();

                    entity.Value.Clear();
                }
            }
        }

    }
}
