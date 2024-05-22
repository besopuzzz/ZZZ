
namespace ZZZ.Framework.Assets.Tiling.Physics
{
    public struct TileColliderData
    {
        public List<Vector2> Vertices { get; set; } = new List<Vector2>();
        public Vector2 Offset { get; set; } = new Vector2();

        public TileColliderData()
        {

        }
    }
}
