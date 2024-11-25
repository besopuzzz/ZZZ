using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Tiling.Assets
{
    public interface ITile
    {
        void Startup(Point position, Tilemap tilemap, GameObject container);
        void Shutdown(Point position, Tilemap tilemap, GameObject container);
        void GetData(Point position, Tilemap tilemap, ref Transform2D offset);
        void Refresh(Point position, Tilemap tilemap);
    }
}
