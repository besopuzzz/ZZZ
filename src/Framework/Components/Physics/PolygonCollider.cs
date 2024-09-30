using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Components.Physics
{
    public partial class PolygonCollider : Collider<IPolygonColliderProvider>
    {
        public List<Vector2> Vertices
        {
            get => Provider.Vertices;
            set=> Provider.Vertices = value;
        }

        public PolygonCollider()
        {

        }

        protected override IPolygonColliderProvider CreateEmptyProvider()
        {
            return new PolygonColliderProvider();
        }
    }
}
