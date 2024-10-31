using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Components.Physics.Aether.Components
{
    public interface IRigidbody : IComponent
    {
        const float PixelsPerMeter = 64f;
        void Attach(Body body);
        void Detach();
    }
}
