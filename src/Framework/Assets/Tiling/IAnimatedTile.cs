using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Assets.Tiling
{
    public interface IAnimatedTile : IRenderTile
    {
        void GetAnimationData(Point position, Tilemap tilemap, ref TileAnimationData data);
    }
}
