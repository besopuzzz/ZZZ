using ZZZ.Framework.Components.Physics;

namespace ZZZ.Framework.Aether.Core
{
    public struct RaycastResult
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public ICollider Collider { get; set; }
        public Vector2 Normal { get; set; }
        public Vector2 Point { get; set; }
        public float Fraction { get; set; }

        public RaycastResult(ICollider collider, Vector2 normal, Vector2 point, float fraction)
        {
            Collider = collider;
            Normal = normal;
            Point = new Vector2();
            Fraction = fraction;
        }
    }
}
