namespace ZZZ.Framework.Components.Physics.Providers
{
    public interface IPolygonColliderProvider : IColliderProvider
    {
        public List<Vector2> Vertices { get; set; }
    }


}
