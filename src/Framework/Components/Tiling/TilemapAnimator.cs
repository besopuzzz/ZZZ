using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Updating.Components;

namespace ZZZ.Framework.Components.Tiling
{
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapAnimator : Component, ITilemap, IUpdateComponent
    {
        public float Speed { get; set; } = 1f;

        private List<TileAnimator> cache = new List<TileAnimator>();

        void IUpdateComponent.Update(GameTime gameTime)
        {
            cache.ForEach(x => x.Update((float)gameTime.ElapsedGameTime.TotalSeconds * Speed));
        }

        private TileAnimator GetAnimator(IAnimatedTile animatedTile)
        {
            return cache.Find(x => x.Tile == animatedTile);
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IAnimatedTile animatedTile)
            {
                var renderComponent = container.GetComponent<SpriteRenderer>();

                var animator = GetAnimator(animatedTile);

                if(animator == null)
                {
                    animator = new TileAnimator();
                    animator.Tile = animatedTile;

                    cache.Add(animator);
                }

                animator.Renderers.Add(renderComponent);
            }
        }

        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IAnimatedTile animatedTile)
            {
                var renderComponent = container.GetComponent<SpriteRenderer>();
                container.RemoveComponent(renderComponent);

                var animator = GetAnimator(animatedTile);

                animator.Renderers.Remove(renderComponent);

                if(animator.Renderers.Count == 0)
                    cache.Remove(animator);
            }
        }

        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IAnimatedTile render)
            {
                GetAnimator(render).SetData(position, tilemap);
            }
        }

    }
}
