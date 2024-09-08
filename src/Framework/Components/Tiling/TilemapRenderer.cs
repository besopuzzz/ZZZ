using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Core.Updating.Components;

namespace ZZZ.Framework.Components.Tiling
{
    public sealed class TilemapRenderer : GroupRender, ITilemap, IUpdateComponent, IGroupRender
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

        public event EventHandler<SortLayerArgs> LayerChanged;

        protected override void OnEnabledChanged()
        {
            foreach (var item in cache)
            {
                item.Value.Enabled = Enabled;
            }

            base.OnEnabledChanged();
        }

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

        void IUpdateComponent.Update(GameTime gameTime)
        {
            animators.ForEach(x => x.Update((float)gameTime.ElapsedGameTime.TotalSeconds * Speed * paused));
        }

        private TileAnimator GetAnimator(IAnimatedTile animatedTile)
        {
            return animators.Find(x => x.Tile == animatedTile);
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IRenderTile render)
                return;

            var renderComponent = container.AddComponent(new TileRenderer());
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

        public void Render(EntityRenderer renderContext)
        {

        }

    }
}
