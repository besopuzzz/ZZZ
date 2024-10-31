using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Rendering
{
    public class RenderContext : IRenderContext
    {
        public enum RenderMode
        {
            ToOneLayer,
            ToEveryLayer
        }
        public ICamera MainCamera => Camera.MainCamera;

        private readonly SpriteBatch spriteBatch;
        private readonly Dictionary<SortLayer, List<RenderComponent>> entities = new Dictionary<SortLayer, List<RenderComponent>>();

        public RenderContext(GraphicsDevice graphicsDevice) 
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            foreach (var sortLayer in Enum.GetValues<SortLayer>())
                entities.Add(sortLayer, new List<RenderComponent>());
        }

        public RenderContext(RenderContext renderContext) : this(renderContext.spriteBatch.GraphicsDevice)
        {

        }

        public void AddToQueue(RenderComponent entity)
        {
            entities[entity.Layer].Add(entity);
        }

        public void Start(SpriteSortMode sortMode, BasicEffect effect, SamplerState samplerState)
        {
            spriteBatch.Begin(sortMode: sortMode, effect: effect, samplerState: samplerState);
        }

        public void End()
        {
            spriteBatch.End();
        }

        public void RenderSprite(Transform2D transform, Sprite sprite, Color color, SpriteEffects spriteEffect)
        {
            spriteBatch.DrawSprite(sprite, transform, color, spriteEffect);
        }

        private void SortAndRender(List<RenderComponent> entitiesToRender)
        {
            entitiesToRender.Sort();

            foreach (var entity in entitiesToRender)
                entity.InternalRender(this);
        }

        internal void Reset()
        {
            foreach (var entity in entities)
                entity.Value.Clear();
        }

        internal void RenderQueue(RenderMode renderMode)
        {
            if (renderMode == RenderMode.ToOneLayer)
            {
                MainCamera.Apply(this);

                foreach (var entity in entities)
                {
                    SortAndRender(entity.Value);

                    entity.Value.Clear();
                }

                End();
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (!MainCamera.LayerMask.HasFlag(entity.Key))
                        continue;

                    MainCamera.Apply(this);

                    SortAndRender(entities[entity.Key]);

                    entity.Value.Clear();

                    End();
                }
            }
        }

    }
}
