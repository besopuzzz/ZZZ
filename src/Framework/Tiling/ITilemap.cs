using ZZZ.Framework.Tiling.Assets;
using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Tiling
{
    public interface ITilemap
    {
        void Add(GameObject container, ITile tile, Point position, Tilemap tilemap);
        void Remove(GameObject container, ITile tile, Point position, Tilemap tilemap);
        void SetData(GameObject container, ITile tile, Point position, Tilemap tilemap);
    }
}
