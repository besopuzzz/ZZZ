using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Monogame.Tiling.Components.Rendering;
using ZZZ.Framework.Monogame.Tiling.Assets;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Tiling.Components
{
    [RequireComponent(Type = typeof(Transformer))]
    [RequireComponent(Type = typeof(Grid), ToOwner = true)]
    [RequireComponent(Type = typeof(TilemapRenderer), Remove = true)]
    public class Tilemap : UpdateComponent
    {
        public float AnimationSpeed { get; set; } = 1f;
        public Vector2 CellOrigin { get; set; }

        [ContentSerializer(CollectionItemName = "Tile")]
        internal TileCollection Tiles = new();

        public event TilemapEvent TileAdded;
        public event TilemapEvent TileRemoved;
        public event TilemapEvent TileReplaced;

        private List<TileAnimator> animatedTiles = new();
        private Transformer transformer;
        private Grid grid;

        protected override void Startup()
        {
            grid = Owner.Owner.GetComponent<Grid>();
            transformer = GetComponent<Transformer>();
            transformer.WorldChanged += OnWorldChanged;

            foreach (var item in Tiles)
            {
                item.Refresh(this);
            }

            Tiles.TileAdded += Tilemap_TileAdded;
            Tiles.TileRemoved += Tilemap_TileRemoved;
            Tiles.TileReplaced += Tilemap_TileReplaced;

            UpdateWorld();

            base.Startup();
        }
        protected override void Shutdown()
        {
            transformer.WorldChanged -= OnWorldChanged;

            base.Shutdown();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var item in animatedTiles)
            {
                item.Update((float)gameTime.ElapsedGameTime.TotalSeconds * AnimationSpeed);
            }

            base.Update(gameTime);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (IDisposable item in Tiles)
                {
                    item.Dispose();
                }
            }

            Tiles = null;
            animatedTiles = null;

            base.Dispose(disposing);
        }

        private void OnWorldChanged(ITransformer sender, Transform2D args)
        {
            UpdateWorld();
        }
        private void FullRefresh(TileRenderData renderData)
        {
            renderData.Refresh(this);
            renderData.BaseTile.Refresh(renderData.Position, this);
        }
        private void UpdateWorld()
        {
            foreach (var item in Tiles)
            {
                item.UpdateWorld(transformer.World);
            }
        }
        private void Tilemap_TileAdded(TileCollection tiles, TileRenderData renderData)
        {
            FullRefresh(renderData);

            TileAdded?.Invoke(renderData.Position, renderData.BaseTile);
        }
        private void Tilemap_TileReplaced(TileCollection tiles, TileRenderData renderData)
        {
            FullRefresh(renderData);

            TileReplaced?.Invoke(renderData.Position, renderData.BaseTile);
        }
        private void Tilemap_TileRemoved(TileCollection tiles, TileRenderData renderData)
        {
            ClearAnimationTale(renderData);
            renderData.BaseTile.Refresh(renderData.Position, this);

            TileRemoved?.Invoke(renderData.Position, renderData.BaseTile);
        }

        internal void AddAnimatedTile(TileRenderData tileRenderData, AnimatedTile animatedTile)
        {
            var animator = animatedTiles.Find(x=>x.AnimatedTile == animatedTile);

            if (animator == null)
            {
                animator = new TileAnimator(animatedTile);
                animatedTiles.Add(animator);
            }

            animator.Register(tileRenderData);
        }
        internal void RemoveAnimatedTile(TileRenderData tileRenderData, AnimatedTile animatedTile)
        {
            var animator = animatedTiles.Find(x => x.AnimatedTile == animatedTile);

            if (animator == null)
                return;

            if (animator.Unregister(tileRenderData))
                animatedTiles.Remove(animator);
        }
        internal void ClearAnimationTale(TileRenderData tileRenderData)
        {
            var animator = animatedTiles.Find(x => x.Contains(tileRenderData));

            if (animator == null)
                return;

            if(animator.Unregister(tileRenderData))
                animatedTiles.Remove(animator);
        }
        internal TileRenderData GetTileObject(Point position)
        {
            return Tiles.Find(x => x.Position == position);
        }

        public void RemoveTile(Point position)
        {
            var tile = GetTileObject(position);

            if (tile == null)
                return;

            Tiles.Remove(tile);
        }
        public void AddTile(Point position, Tile baseTile)
        {
            var tile = GetTileObject(position);

            tile ??= new TileRenderData(position, baseTile);

            Tiles.Add(tile);
        }
        public void Refresh(Point position)
        {
            GetTileObject(position)?.Refresh(this);
        }
        public Tile GetTile(Point position)
        {
            return GetTileObject(position)?.BaseTile;
        }

        public Vector2 GetPositionFromPoint(Point point)
        {
            return grid.GetPositionFromPoint(point);
        }
        public Point GetPointFromPosition(Vector2 position)
        {
            return grid.GetPointFromPosition(position);
        }

        public Vector2 GetCellSize()
        {
            return grid.CellSize;
        }
    }
}
