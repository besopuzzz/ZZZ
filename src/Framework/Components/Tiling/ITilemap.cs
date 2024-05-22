using ZZZ.Framework.Assets.Tiling;

namespace ZZZ.Framework.Components.Tiling
{
    public interface ITilemap : IComponent
    {
        void Add(GameObject container, ITile tile, Point position, Tilemap tilemap);
        void Remove(GameObject container, ITile tile, Point position, Tilemap tilemap);
        void SetData(GameObject container, ITile tile, Point position, Tilemap tilemap);
    }
}
