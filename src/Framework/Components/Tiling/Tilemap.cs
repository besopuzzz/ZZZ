using System.ComponentModel;
using ZZZ.Framework.Assets.Tiling;

namespace ZZZ.Framework.Components.Tiling
{
    public class Tilemap : Component, ITilemap
    {
        public Vector2 TileSize
        {
            get => tileSize;
            set
            { 
                if(tileSize ==  value) return;
            
                tileSize = value;

                RefreshAll();
            }
        }

        [ContentSerializer]
        private List<GameObject> NotAdded { get; } = new List<GameObject>();

        private Dictionary<TileComponent, ITile> cache = new Dictionary<TileComponent, ITile>();
        private List<ITilemap> tilemaps = new List<ITilemap>();
        private Vector2 tileSize = new Vector2(32);

        protected override void Awake()
        {
            tilemaps.AddRange(GetComponents<ITilemap>()); // Find all ITilemaps

            foreach (var item in GetGameObjects()) // Find added tiles (after deserialize)
            {
                var tile = item.GetComponent<TileComponent>();

                if (tile == null) continue;

                AddTile(tile);
            }

            foreach (var item in NotAdded) // Create new TileBaseComponent, added from Add(Point, ITile) nethod
            {
                AddGameObject(item);
                AddTile(item.GetComponent<TileComponent>());
            }

            NotAdded.Clear(); // Forget all new tiles

            Owner.ComponentAdded += Owner_ComponentAdded;
            Owner.ComponentRemoved += Owner_ComponentRemoved;
            Owner.GameObjectRemoved += Owner_GameObjectRemoved;

            base.Awake();
        }
        protected override void Shutdown()
        {
            Owner.ComponentAdded -= Owner_ComponentAdded;
            Owner.ComponentRemoved -= Owner_ComponentRemoved;
            Owner.GameObjectRemoved -= Owner_GameObjectRemoved;

            foreach (var tile in cache.Keys) // Clear all tiles from tilemap hadlers
            {
                foreach (var tilemap in tilemaps)
                {
                    tilemap.Remove(tile.Owner, tile.BaseTile, tile.Position, this);
                }
            }

            cache.Clear(); // Clear tiles

            base.Shutdown();
        }

        private void Owner_GameObjectRemoved(GameObject sender, GameObject e) // Remove TileBaseComponent from cache, if container removed from other place
        {
            var tile = e.GetComponent<TileComponent>();

            if(tile == null) return;

            foreach (var tilemap in tilemaps)
            {
                tilemap.Remove(tile.Owner, tile.BaseTile, tile.Position, this);

                cache.Remove(tile);
            }
        }
        private void Owner_ComponentAdded(GameObject sender, IComponent e) // Check component for TilemapHandler and send all tiles to add
        {
            if (e is ITilemap tilemap)
            {
                tilemaps.Add(tilemap);

                foreach (var tile in cache.Keys)
                {
                    tilemap.Add(tile.Owner, tile.BaseTile, tile.Position, this);
                    tilemap.SetData(tile.Owner, tile.BaseTile, tile.Position, this);
                }
            }
        }
        private void Owner_ComponentRemoved(GameObject sender, IComponent e) // Check component for TilemapHandler and send all tiles to remove
        {
            if (e is ITilemap tilemap)
            {
                foreach (var tile in cache.Keys)
                    tilemap.Remove(tile.Owner, tile.BaseTile, tile.Position, this);

                tilemaps.Remove(tilemap);
            }
        }

        public void Add(Point position, ITile tile)
        {
            Remove(position);

            GameObject container = new GameObject();

            TileComponent tileComponent = container.AddComponent(new TileComponent());
            tileComponent.BaseTile = tile;
            tileComponent.Position = position;

            if (Started)
            {
                AddGameObject(container);
                AddTile(tileComponent);
            }
            else NotAdded.Add(container);
        }
        public void Remove(Point position)
        {
            var tile = GetTileComponent(position);

            if (tile == null)
                return;

            if (Started)
                RemoveGameObject(tile.Owner);
            else NotAdded.Remove(tile.Owner);
        }
        public void Refresh(Point position)
        {
            var component = GetTileComponent(position);
            component.SetData();

            foreach (var tilemap in tilemaps)
            {
                tilemap.SetData(component.Owner, component.BaseTile, component.Position, this);
            }
        }

        public void RefreshAll()
        {
            if (!Started)
                return;

            foreach (var item in GetGameObjects())
            {
                var component = item.GetComponent<TileComponent>();

                if (component == null) continue;

                component.SetData();

                foreach (var tilemap in tilemaps)
                {
                    tilemap.SetData(component.Owner, component.BaseTile, component.Position, this);
                }
            }
        }

        public TTile GetTile<TTile>(Point position) where TTile : ITile
        {
            return (TTile)GetTileComponent(position).BaseTile;
        }
        public Vector2 GetPositionFromPoint(Point point)
        {
            Transform2D local = Transform2D.CreateTranslation(point.ToVector2() * TileSize);

            return local.Position;
        }
        public Point GetPointFromPosition(Vector2 position)
        {
            if (position.X < 0)
                position.X -= TileSize.X;

            if (position.Y < 0)
                position.Y -= TileSize.Y;

            return (position / TileSize).ToPoint();
        }

        private TileComponent GetTileComponent(Point position)
        {
            if (!Started)
            {
                foreach (var item in NotAdded)
                {
                    var baseComponent = item.GetComponent<TileComponent>();

                    if(baseComponent != null)
                    {
                        if(baseComponent.Position == position)
                            return baseComponent;
                    }
                }

                return null;
            }

            foreach (var item in GetGameObjects())
            {
                var tile = item.GetComponent<TileComponent>();

                if (tile == null) continue;

                if (tile.Position == position)
                    return tile;
            }

            return null;
        }
        private void AddTile(TileComponent tileComponent)
        {
            foreach (var tilemap in tilemaps)
            {
                tilemap.Add(tileComponent.Owner, tileComponent.BaseTile, tileComponent.Position, this);
                tilemap.SetData(tileComponent.Owner, tileComponent.BaseTile, tileComponent.Position, this);
            }
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            cache.Add(container.GetComponent<TileComponent>(), tile);
        }
        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            cache.Remove(container.GetComponent<TileComponent>());
        }
        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            var component = container.GetComponent<TileComponent>();

            component.SetData();
        }
    }
}
