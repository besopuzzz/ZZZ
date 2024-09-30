using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Components.Physics
{
    public partial class CircleCollider : Collider<ICircleColliderProvider>
    {
        public float Radius
        {
            get => Provider.Radius;
            set => Provider.Radius = value;
        }

        public CircleCollider()
        {
        }

        protected override ICircleColliderProvider CreateEmptyProvider()
        {
            return new CircleColliderProvider();
        }
    }
}
