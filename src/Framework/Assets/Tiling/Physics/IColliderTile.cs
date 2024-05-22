using ZZZ.Framework.Components.Tiling;

namespace ZZZ.Framework.Assets.Tiling.Physics
{
    public interface IColliderTile : ITile
    {
        void GetColliderData(Point position, Tilemap tilemap, ref TileColliderData renderedTile);
    }
}
