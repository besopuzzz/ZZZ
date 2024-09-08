using nkast.Aether.Physics2D.Collision.Shapes;
using ZZZ.Framework.Physics.Components;

namespace ZZZ.Framework.Components.Physics
{
    public class CircleCollider : Collider<CircleShape>
    {
        public float Radius
        {
            get=>Shape.Radius * IRigidbody.PixelsPerMeter;
            set=>Shape.Radius = value / IRigidbody.PixelsPerMeter;
        }

        public CircleCollider() : base(new CircleShape(32f / IRigidbody.PixelsPerMeter, 1f))
        {
        }

        protected override void OnOffsetChanged(Vector2 oldOffset, Vector2 offset)
        {
            Shape.Position -= oldOffset;
            Shape.Position += offset;
        }
    }
}
