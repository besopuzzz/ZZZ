using ZZZ.Framework.Rendering.Components;
using ZZZ.Framework.Tiling;
using ZZZ.Framework.Tiling.Assets;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.Tiling.Components
{
    [RequiredTilemap]
    public sealed class TilemapRenderer : GroupRender, ITilemap, IUpdater
    {
        public float Speed { get; set; } = 10f;
        public new SortLayer Layer
        {
            get => base.Layer;
            set
            {
                if (base.Layer == value) return;

                base.Layer = value;

                foreach (var item in cache.Values)
                {
                    item.Layer = base.Layer;
                }
            }
        }
        public bool Paused
        {
            get => paused == 0f;
            set => paused = value ? 0f : 1f;
        }
        public TileRenderMode RenderMode { get; set; }

        private float paused = 1f;
        private Dictionary<Point, TileRenderer> cache = new Dictionary<Point, TileRenderer>();
        private List<TileAnimator> animators = new List<TileAnimator>();

        protected override void Shutdown()
        {
            cache.Clear();
            animators.Clear();



            base.Shutdown();
        }

        public void Reset()
        {
            animators.ForEach(x => x.Reset());
        }

        void IUpdater.Update(TimeSpan time)
        {
            animators.ForEach(x => x.Update((float)time.TotalSeconds * Speed * paused));
        }

        private TileAnimator GetAnimator(IAnimatedTile animatedTile)
        {
            return animators.Find(x => x.Tile == animatedTile);
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IRenderTile render)
                return;

            var renderComponent = container.AddComponent<TileRenderer>();
            renderComponent.Layer = Layer;
            renderComponent.Enabled = Enabled;
            renderComponent.Order = position.Y * (int)tilemap.TileSize.Y;

            cache.Add(position, renderComponent);

            if (tile is not IAnimatedTile animatedTile)
                return;

            var animator = GetAnimator(animatedTile);

            if (animator == null)
            {
                animator = new TileAnimator();
                animator.Tile = animatedTile;

                animators.Add(animator);
            }

            animator.Renderers.Add(renderComponent);
        }

        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IRenderTile render)
                return;

            var renderComponent = container.GetComponent<TileRenderer>();

            container.RemoveComponent(renderComponent);

            cache.Remove(position);

            if (tile is not IAnimatedTile animatedTile)
                return;

            var animator = GetAnimator(animatedTile);

            if (animator == null)
                return;

            animator.Renderers.Remove(renderComponent);

            if (animator.Renderers.Count == 0)
                animators.Remove(animator);
        }

        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IRenderTile render)
                return;

            var renderComponent = cache[position];

            TileRenderData tileRenderData = new TileRenderData();
            render.GetRenderingData(position, tilemap, ref tileRenderData);

            renderComponent.Sprite = tileRenderData.Sprite;
            renderComponent.Color = tileRenderData.Color;
            renderComponent.SpriteEffect = tileRenderData.SpriteEffect;
            renderComponent.Layer = Layer;
            renderComponent.Order = position.Y * (int)tilemap.TileSize.Y;

            Transform2D stretch = new Transform2D();

            if (RenderMode == TileRenderMode.Stretch)
                stretch = Transform2D.CreateScale(tilemap.TileSize / tileRenderData.Sprite.Size.ToVector2());

            renderComponent.Transform = stretch;

            if (tile is IAnimatedTile animated)
            {
                GetAnimator(animated).SetData(position, tilemap);
            }
        }

    }
}
