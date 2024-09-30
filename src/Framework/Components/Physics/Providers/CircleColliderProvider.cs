namespace ZZZ.Framework.Components.Physics.Providers
{
    public class CircleColliderProvider : ColliderProvider, ICircleColliderProvider
    {
        public float Radius
        {
            get => radius;
            set => radius = value;
        }

        private float radius;
    }
}
