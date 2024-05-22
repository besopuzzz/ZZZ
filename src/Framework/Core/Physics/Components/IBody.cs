using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Physics.Components
{
    public interface IBody : IComponent
    {
        internal Body Body { get; }
        internal void UpdateBody();
        internal void UpdateTransformer();
    }

    public interface IRigidbody : IComponent
    {
        const float PixelsPerMeter = 64f;
        void Attach(Body body);
        void Detach();
    }

    public interface ICollider : IRigidbody
    {

    }
}
