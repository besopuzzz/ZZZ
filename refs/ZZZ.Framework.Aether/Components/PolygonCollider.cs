using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;

namespace ZZZ.Framework.Physics.Aether.Components
{
    public class PolygonCollider : Collider<PolygonShape>
    {
        public IEnumerable<Vector2> Vertices
        {
            get => vertices;
            set
            {
                if (vertices == value)
                    return;

                vertices.Clear();
                vertices.AddRange(value);
                BuildVertices();
            }
        }

        private Vertices vertices = new nkast.Aether.Physics2D.Common.Vertices() { new Vector2(), new Vector2(1,0), new Vector2(1)};

        public PolygonCollider() : base(Create())
        {

        }

        public PolygonCollider(Vector2[] vertices) : base(new PolygonShape(new nkast.Aether.Physics2D.Common.Vertices(vertices), 1f))
        {

        }

        protected override void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        {
            var matrix = Matrix.CreateScale(IRigidbody.PixelsPerMeter, IRigidbody.PixelsPerMeter, 1f);


            Shape.Vertices.Translate(-Vector2.Transform(oldValue, matrix));
            Shape.Vertices.Translate(Vector2.Transform(newValue, matrix));
        }

        private static PolygonShape Create()
        {
            Vertices vertices = [new Vector2(), new Vector2(1, 0), new Vector2(1)];

            return new PolygonShape(vertices, 1f);
        }

        private void BuildVertices()
        {
            vertices.Translate(Offset);

            var matrix = Matrix.CreateScale(1 / IRigidbody.PixelsPerMeter, 1 / IRigidbody.PixelsPerMeter, 1f);

            vertices.Transform(ref matrix);

            Shape.Vertices = vertices;
        }
    }
}
