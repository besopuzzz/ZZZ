namespace ZZZ.Framework.Components.Physics.Providers
{
    public class PolygonColliderProvider : ColliderProvider, IPolygonColliderProvider
    {
        public List<Vector2> Vertices
        {
            get => vertices;
            set => vertices = value;
        }

        private List<Vector2> vertices;
    }
}
