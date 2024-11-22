using nkast.Aether.Physics2D.Common;
using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Assets.Tiling.Physics;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Tiling;
using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Components.Physics.Aether.Components
{
    public sealed class TilemapColliderArgs : EventArgs
    {
        public ICollider TileCollider { get; }
        public Point Position { get; }

        public TilemapColliderArgs(ICollider collider) 
        {
            TileCollider = collider;
            Position = ((Component)collider).Owner.GetComponent<TileComponent>().Position;
        }
    }

    public delegate void TilemapColliderEvent(TilemapColliderArgs args, ICollider other);

    public sealed class TilemapCollider : Component, ITilemap
    {
        public ColliderLayer Layer
        {
            get
            {
                return category;
            }
            set
            {
                if (value == category)
                    return;

                category = value;

                foreach (var item in cache.Values)
                {
                    item.Layer = Layer;
                }
            }
        }

        public override bool Enabled 
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;

                foreach (var item in cache.Values)
                {
                    item.Enabled = value;
                }
            }
        }

        public event TilemapColliderEvent ColliderEnter;
        public event TilemapColliderEvent ColliderExit;

        private Dictionary<Point, PolygonCollider> cache = new Dictionary<Point, PolygonCollider>();
        private ColliderLayer category = ColliderLayer.Cat1;

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = container.AddComponent<PolygonCollider>();

            collider.Vertices = PolygonTools.CreateRectangle(tilemap.TileSize.X / 2, tilemap.TileSize.Y / 2).ToArray();
            collider.Layer = Layer;
            collider.ColliderEnter += Collider_ColliderEnter;
            collider.ColliderExit += Collider_ColliderExit;

            cache.Add(position, collider);
        }

        private void Collider_ColliderExit(ICollider sender, ICollider other)
        {
            ColliderExit?.Invoke(new TilemapColliderArgs(sender), other);
        }

        private void Collider_ColliderEnter(ICollider sender, ICollider other)
        {
            ColliderEnter?.Invoke(new TilemapColliderArgs(sender), other);
        }

        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = cache[position];

            container.RemoveComponent(collider);

            collider.ColliderEnter -= Collider_ColliderEnter;
            collider.ColliderExit -= Collider_ColliderExit;

            cache.Remove(position);
        }

        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = cache[position];

            TileColliderData tileRenderData = new TileColliderData();

            colliderTile.GetColliderData(position, tilemap, ref tileRenderData);

            List<Vector2> vertices = new List<Vector2>();

            if (tileRenderData.Vertices.Count >= 3) // Triangle or polygon
                vertices.AddRange(tileRenderData.Vertices);
            else vertices.AddRange(PolygonTools.CreateRectangle(tilemap.TileSize.X / 2, tilemap.TileSize.Y / 2));

            collider.Vertices = vertices;
            collider.Offset = tileRenderData.Offset;
            collider.Restitution = tileRenderData.Restitution;
            collider.Friction = tileRenderData.Friction;
            collider.IsTrigger = tileRenderData.IsTrigger;
        }


    }
}
