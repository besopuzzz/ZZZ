using ZZZ.Framework.Tiling;
using ZZZ.Framework.Tiling.Assets;
using ZZZ.Framework.Tiling.Assets.Physics;
using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Physics.Aether.Components
{
    [RequiredTilemap]
    public sealed class TilemapCollider : GroupCollider, ITilemap
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
                    //item.Enabled = value;
                }
            }
        }

        private Dictionary<Point, PolygonCollider> cache = new Dictionary<Point, PolygonCollider>();
        private ColliderLayer category = ColliderLayer.Cat1;

        private void Refresh(PolygonCollider collider, IColliderTile colliderTile, Point position, Tilemap tilemap)
        {
            TileColliderData tileColliderData = default;

            colliderTile.GetColliderData(position, tilemap, ref tileColliderData);

            collider.Vertices = tileColliderData.Vertices;
            collider.Restitution = tileColliderData.Restitution;
            collider.Friction = tileColliderData.Friction;
            collider.Offset = tilemap.GetPositionFromCell(position) + tileColliderData.Offset;
            collider.Density = tileColliderData.Density;
            collider.Layer = Layer;
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = Add(new PolygonCollider());

            Refresh(collider, colliderTile, position, tilemap);

            cache.Add(position, collider);
        }

        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = cache[position];

            container.RemoveComponent(collider.GetType());

            cache.Remove(position);
        }

        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is not IColliderTile colliderTile)
                return;

            var collider = cache[position];

            Refresh(collider, colliderTile, position, tilemap);
        }
    }
}
