using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Tiling.Assets
{
    public interface IAnimatedTile : IRenderTile
    {
        void GetAnimationData(Point position, Tilemap tilemap, ref TileAnimationData data);
    }
}
