using ZZZ.Framework.Core;

namespace ZZZ.Framework.Components.Physics.Providers
{
    public interface IRigidbodyProvider : IComponentProvider
    {
        Vector2 Velocity { get; set; }
        bool IsKinematic { get; set; }
        float AngularVelocity { get; set; }
        float Mass { get; set; }
        float LinearDamping { get; set; }
        float AngularDamping { get; set; }
        float Inertia { get; set; }
        bool IsBullet { get; set; }
        bool FixedRotation { get; set; }
        Vector2 Gravity { get; set; }

        void ApplyForce(Vector2 vector);
        void ApplyAngularImpulse(float impulse);
        void ApplyLinearImpulse(Vector2 vector);
        void ApplyTorque(float torque);
    }


}
