﻿using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;

namespace ZZZ.Framework.Physics.Components
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

                BuildVertices();
            }
        }

        private Vertices vertices = new nkast.Aether.Physics2D.Common.Vertices();

        public PolygonCollider() : base(Create())
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
            Vertices vertices = [new Vector2(), new Vector2(), new Vector2()];

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