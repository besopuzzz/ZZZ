using nkast.Aether.Physics2D.Collision.Shapes;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Aether.Core
{
    internal class AetherCircleColliderProvider : AetherColliderProvider<CircleShape>, ICircleColliderProvider
    {
        public virtual float Radius
        {
            get => Shape.Radius * PhysicalBody.PixelsPerMeter;
            set
            {
                Shape.Radius = value / PhysicalBody.PixelsPerMeter;

                ComputeBody();
            }
        }

        public AetherCircleColliderProvider(ICollider collider) : base(collider, new CircleShape(10f / PhysicalBody.PixelsPerMeter, 1f))
        {
        }

        protected override void ApplyScaledOffset(Vector2 offset)
        {
            Shape.Position += offset;
        }
    }
}
