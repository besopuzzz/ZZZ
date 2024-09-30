using nkast.Aether.Physics2D.Collision.Shapes;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Aether.Core
{
    internal class AetherPolygonColliderProvider : AetherColliderProvider<PolygonShape>, IPolygonColliderProvider
    {
        public List<Vector2> Vertices
        {
            get => vertices;
            set
            {
                vertices = value;

                PolygonShape shape = Shape as PolygonShape;

                var newVertices = new nkast.Aether.Physics2D.Common.Vertices(vertices);

                newVertices.Translate(Offset);

                var scale = Matrix.CreateScale(1f / PhysicalBody.PixelsPerMeter);

                newVertices.Transform(ref scale);

                shape.Vertices = newVertices;

                ComputeBody();
            }
        }

        private List<Vector2> vertices;

        public AetherPolygonColliderProvider(ICollider collider) : base(collider, new PolygonShape([new Vector2(0), new Vector2(0,1), new Vector2(1)], 1f))
        {
        }

        protected override void ApplyScaledOffset(Vector2 offset)
        {
            Shape.Vertices.Translate(offset);
        }
    }
}
