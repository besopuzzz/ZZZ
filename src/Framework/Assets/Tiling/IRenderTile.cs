using ZZZ.Framework.Components.Tiling;

namespace ZZZ.Framework.Assets.Tiling
{
    public interface IRenderTile : ITile
    {
        void GetRenderingData(Point position, Tilemap tilemap, ref TileRenderData renderedTile);
    }
}
