using ZZZ.Framework.Assets.Physics;

namespace ZZZ.Framework.Assets.Tiling.Physics
{
    public struct TileColliderData
    {
        public List<Vector2> Vertices { get; set; } = new List<Vector2>();
        public Vector2 Offset { get; set; } = new Vector2();
        public bool IsTrigger { get; set; } = false;
        public float Density { get; set; } = 0.01f;
        public float Friction { get; set; } = 0.1f;
        public float Restitution { get; set; } = 0f;
        public TileColliderData()
        {

        }
    }
}
