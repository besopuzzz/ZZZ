
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Tiling;

namespace ZZZ.Framework.Assets.Tiling.Physics
{
    public struct TileColliderData
    {
        public List<Vector2> Vertices { get; set; } = new List<Vector2>();
        public Vector2 Offset { get; set; } = new Vector2();
        public bool IsTrigger { get; set; } = false;
        public float Friction { get; set; } = 0.2f;

        /// <summary>
        /// Упругость материала.
        /// </summary>
        public float Restitution { get; set; } = 0f;

        /// <summary>
        /// Плотность материала.
        /// </summary>
        public float Density { get; set; } = 1f;

        public TileColliderData()
        {

        }
    }
}
