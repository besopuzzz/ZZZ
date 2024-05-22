using ZZZ.Framework.Physics.Components;

namespace ZZZ.Framework.Core.Physics
{
    public struct RaycastResult
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public Collider Collider { get; set; }
        public Vector2 Normal { get; set; }
        public Vector2 Point { get; set; }
        public float Fraction { get; set; }

        public RaycastResult(Collider collider, Vector2 normal, Vector2 point, float fraction)
        {
            Collider = collider;
            Normal = normal;
            Point = new Vector2();
            Fraction = fraction;
        }
    }
}
