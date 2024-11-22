using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Assets.Tiling
{
    public interface IRenderTile : ITile
    {
        void GetRenderingData(Point position, Tilemap tilemap, ref TileRenderData renderedTile);
    }
}
