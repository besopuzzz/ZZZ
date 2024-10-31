using nkast.Aether.Physics2D.Collision.Shapes;

namespace ZZZ.Framework.Components.Physics.Aether.Components
{
    public partial class CircleCollider : Collider<CircleShape>
    {
        public float Radius
        {
            get => Shape.Radius;
            set => Shape.Radius = value;
        }

        public CircleCollider() : base(new CircleShape(0.5f, 1f))
        {
        }

        protected override void OnOffsetChanged(Vector2 oldOffset, Vector2 offset)
        {
            var newValue = (offset - oldOffset) / IRigidbody.PixelsPerMeter; 
            Shape.Position += newValue;
        }
    }
}
