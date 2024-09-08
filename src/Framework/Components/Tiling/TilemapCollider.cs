using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Assets.Tiling.Physics;
using ZZZ.Framework.Physics.Components;

namespace ZZZ.Framework.Components.Tiling
{
    public sealed class TilemapColliderArgs : EventArgs
    {
        public Collider TileCollider { get; }
        public Point Position { get; }

        public TilemapColliderArgs(Collider collider) 
        {
            TileCollider = collider;
            Position = collider.Owner.GetComponent<TileComponent>().Position;
        }
    }

    public delegate void TilemapColliderEvent(TilemapColliderArgs args, Collider other);

    public sealed class TilemapCollider : Component, ITilemap
    {
        public ColliderLayer Layer
        {
            get
            {
                return (ColliderLayer)Enum.ToObject(typeof(ColliderLayer), (int)category);
            }
            set
            {
                var newValue = (Category)Enum.ToObject(typeof(Category), (int)value);

                if (newValue == category)
                    return;

                category = newValue;

                foreach (var item in cache.Values)
                {
                    item.Layer = Layer;
                }
            }
        }

        public event TilemapColliderEvent ColliderEnter;
        public event TilemapColliderEvent ColliderExit;

        private Dictionary<Point, PolygonCollider> cache = new Dictionary<Point, PolygonCollider>();
        private Category category = Category.Cat1;

        private Vertices CreateDefaultVertices(Tilemap tilemap)
        {
            Vertices vertices = new Vertices() { new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, 0.5f) };

            return vertices;
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = container.AddComponent(new PolygonCollider());

            collider.Vertices = PolygonTools.CreateRectangle(tilemap.TileSize.X / 2f, tilemap.TileSize.Y / 2f);
            collider.Layer = Layer;
            collider.ColliderEnter += Collider_ColliderEnter;
            collider.ColliderExit += Collider_ColliderExit;

            cache.Add(position, collider);
        }

        private void Collider_ColliderExit(Collider sender, Collider other)
        {
            ColliderExit?.Invoke(new TilemapColliderArgs(sender), other);
        }

        private void Collider_ColliderEnter(Collider sender, Collider other)
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

            Vertices vertices = new Vertices();

            if (tileRenderData.Vertices.Count >= 3) // Triangle or polygon
                vertices.AddRange(tileRenderData.Vertices);
            else vertices.AddRange(PolygonTools.CreateRectangle(tilemap.TileSize.X / 2f, tilemap.TileSize.Y / 2f));

            collider.Vertices = vertices;
            collider.Offset = tileRenderData.Offset;
            collider.Friction = tileRenderData.Friction;
            collider.Restitution = tileRenderData.Restitution;
            collider.Density = tileRenderData.Density;
            collider.IsTrigger = tileRenderData.IsTrigger;
        }


    }
}
