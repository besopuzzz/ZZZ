using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Assets.Tiling.Physics;
using ZZZ.Framework.Physics.Components;

namespace ZZZ.Framework.Components.Tiling
{
    public sealed class TilemapCollider : Component, ITilemap
    {
        public bool IsTrigger
        {
            get => isTrigger;
            set
            {
                if (isTrigger == value)
                    return;

                isTrigger = value;

                foreach (var item in cache.Values)
                {
                    item.IsTrigger = IsTrigger;
                }
            }
        }
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


        private Dictionary<Point, BoxCollider> cache = new Dictionary<Point, BoxCollider>();
        private bool isTrigger = false;
        private Category category = Category.Cat1;

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IColliderTile colliderTile)
            {
                var collider = container.AddComponent(new BoxCollider());
                collider.Size = tilemap.TileSize;
                collider.IsTrigger = isTrigger;
                collider.Layer = Layer;

                cache.Add(position, collider);
            }
        }

        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IColliderTile colliderTile)
            {
                var collider = container.GetComponent<BoxCollider>();
                container.RemoveComponent(collider);

                cache.Remove(position);
            }
        }

        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IColliderTile colliderTile)
            {
                var collider = cache[position];

                TileColliderData tileRenderData = new TileColliderData();
                colliderTile.GetColliderData(position, tilemap, ref tileRenderData);

                collider.Size = tilemap.TileSize;
                collider.Offset = tileRenderData.Offset;
            }
        }
    }
}
