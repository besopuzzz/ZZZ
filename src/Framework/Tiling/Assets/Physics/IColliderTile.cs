using ZZZ.Framework.Tiling.Assets;
using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Tiling.Assets.Physics
{
    public interface IColliderTile : ITile
    {
        void GetColliderData(Point position, Tilemap tilemap, ref TileColliderData renderedTile);
    }
}
