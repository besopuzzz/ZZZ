namespace ZZZ.Framework.Components.Physics
{
    public class BoxCollider : PolygonCollider
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
                                
                List<Vector2> vertices = new List<Vector2>();
                vertices.Add(new Vector2(0f - size.X / 2, 0f - size.Y / 2));
                vertices.Add(new Vector2(size.X / 2, 0f - size.Y / 2));
                vertices.Add(new Vector2(size.X / 2, size.Y / 2));
                vertices.Add(new Vector2(0f - size.X / 2, size.Y / 2));

                vertices.AddRange(vertices);

                Vertices = vertices;
            }
        }

        private Vector2 size = new Vector2(32);

        //protected override void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        //{
        //    var matrix = Matrix.CreateScale(1 / IRigidbody.PixelsPerMeter, 1 / IRigidbody.PixelsPerMeter, 1f);


        //    Shape.Vertices.Translate(-Vector2.Transform(oldValue, matrix));
        //    Shape.Vertices.Translate(Vector2.Transform(newValue, matrix));
        //}

        //private void BuildVertices()
        //{
        //    Vertices vertices = new Vertices();
        //    vertices.Add(new Vector2(0f - size.X / 2, 0f - size.Y / 2));
        //    vertices.Add(new Vector2(size.X / 2, 0f - size.Y / 2));
        //    vertices.Add(new Vector2(size.X / 2, size.Y / 2));
        //    vertices.Add(new Vector2(0f - size.X / 2, size.Y / 2));

        //    vertices.AddRange(vertices);
        //    vertices.Translate(Offset);

        //    var matrix = Matrix.CreateScale(1 / IRigidbody.PixelsPerMeter, 1 / IRigidbody.PixelsPerMeter, 1f);

        //    vertices.Transform(ref matrix);

        //    Shape.Vertices = vertices;
        //}
    }
}
