using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;

namespace ZZZ.Framework.Physics.Aether.Components
{
    public class BoxCollider : Collider<PolygonShape>
    {
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size == value)
                    return;

                size = value;

                BuildVertices();
            }
        }

        private Vector2 size = new Vector2(32);

        public BoxCollider() : base(new PolygonShape(PolygonTools.CreateRectangle(32f,32f), 1f))
        {
            BuildVertices();
        }

        protected override void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        {
            var matrix = Matrix.CreateScale(1 / IRigidbody.PixelsPerMeter, 1 / IRigidbody.PixelsPerMeter, 1f);


            Shape.Vertices.Translate(-Vector2.Transform(oldValue, matrix));
            Shape.Vertices.Translate(Vector2.Transform(newValue, matrix));
        }

        private void BuildVertices()
        {
            Vertices vertices = new Vertices();
            vertices.Add(new Vector2(0f - size.X / 2, 0f - size.Y / 2));
            vertices.Add(new Vector2(size.X / 2, 0f - size.Y / 2));
            vertices.Add(new Vector2(size.X / 2, size.Y / 2));
            vertices.Add(new Vector2(0f - size.X / 2, size.Y / 2));

            vertices.AddRange(vertices);
            vertices.Translate(Offset);

            var matrix = Matrix.CreateScale(1 / IRigidbody.PixelsPerMeter, 1 / IRigidbody.PixelsPerMeter, 1f);

            vertices.Transform(ref matrix);

            Shape.Vertices = vertices;
        }
    }
}
