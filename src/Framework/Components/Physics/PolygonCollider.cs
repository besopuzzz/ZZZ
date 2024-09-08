using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;

namespace ZZZ.Framework.Physics.Components
{
    public class PolygonCollider : Collider<PolygonShape>
    {
        public IEnumerable<Vector2> Vertices
        {
            get => Shape.Vertices;
            set
            {
                if (vertices == value)
                    return;

                vertices = new Vertices(value);

                BuildVertices();
            }
        }

        private Vertices vertices = new nkast.Aether.Physics2D.Common.Vertices();

        public PolygonCollider() : base(Create())
        {

        }

        protected override void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        {
            var matrix = Matrix.CreateScale(1f/ IRigidbody.PixelsPerMeter, 1f / IRigidbody.PixelsPerMeter, 1f);

            Shape.Vertices.Translate(Vector2.Transform(newValue - oldValue, matrix));
        }

        private static PolygonShape Create()
        {
            Vertices vertices = [new Vector2(0), new Vector2(0,1), new Vector2(1)];

            return new PolygonShape(vertices, 1f);
        }

        private void BuildVertices()
        {
            vertices.Translate(Offset);

            var matrix = Matrix.CreateScale(1f / IRigidbody.PixelsPerMeter, 1f / IRigidbody.PixelsPerMeter, 1f);

            vertices.Transform(ref matrix);

            Shape.Vertices = vertices;
        }
    }
}
