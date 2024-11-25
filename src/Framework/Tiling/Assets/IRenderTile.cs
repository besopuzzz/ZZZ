using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Tiling.Assets
{
    public interface IRenderTile : ITile
    {
        void GetRenderingData(Point position, Tilemap tilemap, ref TileRenderData renderedTile);
    }
}
