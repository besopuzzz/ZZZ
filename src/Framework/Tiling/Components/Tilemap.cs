using ZZZ.Framework.Components;
using ZZZ.Framework.Tiling.Assets;

namespace ZZZ.Framework.Tiling.Components
{
    public class Tilemap : Component
    {
        public Vector2 TileSize
        {
            get => tileSize;
            set
            {
                if (tileSize == value) return;

                tileSize = value;

                RefreshAll();
            }
        }

        [ContentSerializerIgnore]
        internal List<ITilemap> Tilemaps => notAddedTilemaps;

        private Vector2 tileSize = new (32);
        private readonly List<ITilemap> tilemaps = [];
        private readonly EventedList<ITilemap> notAddedTilemaps = [new BaseTilemap()];
        private readonly EventedList<TileComponent> cache = [];

        private sealed class BaseTilemap : ITilemap
        {
            public void Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
            {

            }

            public void Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
            {

            }

            public void SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
            {
                var component = container.GetComponent<TileComponent>();

                component.SetData();
            }
        }

        protected override void Awake()
        {
            tilemaps.AddRange(notAddedTilemaps);

            notAddedTilemaps.ItemAdded += Tilemaps_ItemAdded;
            notAddedTilemaps.ItemRemoved += Tilemaps_ItemRemoved;

            cache.ItemAdded += Cache_ItemAdded;

            for (int i = 0; i < cache.Count; i++)
            {
                var tile = cache[i];

                SetDataTile(tile);
            }

            base.Awake();
        }

        private void Cache_ItemAdded(EventedList<TileComponent> sender, TileComponent e)
        {
            SetDataTile(e);
        }

        private void Tilemaps_ItemAdded(EventedList<ITilemap> sender, ITilemap e)
        {
            for (int i = 0; i < cache.Count; i++)
            {
                var tile = cache[i];

                e.Add(tile.Owner, tile.BaseTile, tile.Position, this);
            }

            for (int i = 0; i < cache.Count; i++)
            {
                var tile = cache[i];

                e.SetData(tile.Owner, tile.BaseTile, tile.Position, this);
            }

            tilemaps.Add(e);
        }

        private void Tilemaps_ItemRemoved(EventedList<ITilemap> sender, ITilemap e)
        {
            tilemaps.Remove(e);

            for (int i = 0; i < cache.Count; i++)
            {
                var tile = cache[i];

                e.Remove(tile.Owner, tile.BaseTile, tile.Position, this);
            }
        }

        protected override void Shutdown()
        {
            cache.Clear();
            cache.ItemAdded -= Cache_ItemAdded;

            notAddedTilemaps.Clear();
            notAddedTilemaps.ItemAdded -= Tilemaps_ItemAdded;
            notAddedTilemaps.ItemRemoved -= Tilemaps_ItemRemoved;

            base.Shutdown();
        }

        public void Add(Point position, ITile tile)
        {
            Remove(position);

            var tileComponent = AddGameObject(new GameObject()).AddComponent<TileComponent>();
            tileComponent.Position = position;
            tileComponent.BaseTile = tile;

            cache.Add(tileComponent);

            AddTile(tileComponent);
        }

        public void Remove(Point position)
        {
            var owner = RemoveAndGetOwner(position);

            if (owner != null)
                RemoveGameObject(owner);
        }

        public void Refresh(Point position)
        {
            if (tilemaps.Count == 0)
                return;

            var tile = GetTileComponent(position);

            if (tile == null)
                return;

            SetDataTile(tile);
        }

        public void RefreshAll()
        {
            if (tilemaps.Count == 0)
                return;

            for (int i = 0; i < cache.Count; i++)
            {
                var tile = cache[i];

                for (int y = 0; y < tilemaps.Count; y++)
                {
                    var tilemap = tilemaps[y];

                    tilemap.SetData(tile.Owner, tile.BaseTile, tile.Position, this);
                }
            }
        }

        public Vector2 GetPositionFromCell(Point point)
        {
            Transform2D local = Transform2D.CreateTranslation(point.ToVector2() * TileSize);

            return local.Position;
        }

        public Point GetCellFromPosition(Vector2 position)
        {
            if (position.X < 0)
                position.X -= TileSize.X;

            if (position.Y < 0)
                position.Y -= TileSize.Y;

            return (position / TileSize).ToPoint();
        }

        private GameObject RemoveAndGetOwner(Point position)
        {
            var tile = GetTileComponent(position);

            if (tile == null)
                return null;

            RemoveTile(tile);

            var gameObject = tile.Owner;

            cache.Remove(tile);

            return gameObject;
        }

        private TileComponent GetTileComponent(Point position)
        {
            foreach (var baseComponent in cache)
                if (baseComponent.Position == position)
                    return baseComponent;

            return null;
        }

        private void AddTile(TileComponent tile)
        {
            foreach (var tilemap in notAddedTilemaps)
            {
                tilemap.Add(tile.Owner, tile.BaseTile, tile.Position, this);
            }
        }

        private void RemoveTile(TileComponent tile)
        {
            foreach (var tilemap in notAddedTilemaps)
            {
                tilemap.Remove(tile.Owner, tile.BaseTile, tile.Position, this);
            }
        }

        private void SetDataTile(TileComponent tile)
        {
            for (int y = 0; y < tilemaps.Count; y++)
            {
                var tilemap = tilemaps[y];

                tilemap.SetData(tile.Owner, tile.BaseTile, tile.Position, this);
            }
        }

        internal void SetReference(TileComponent tile)
        {
            cache.Add(tile);
        }
    }
}
