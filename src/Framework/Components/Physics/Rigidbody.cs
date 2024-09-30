using ZZZ.Framework.Components.Physics.Providers;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Components.Physics
{
    public class Rigidbody : Component, IRigidbody
    {
        public Vector2 Velocity
        {
            get => provider.Velocity;
            set => provider.Velocity = value;
        }
        public bool IsKinematic
        {
            get => provider.IsKinematic;
            set => provider.IsKinematic = value;
        }
        public float AngularVelocity
        {
            get => provider.AngularVelocity;
            set => provider.AngularVelocity = value;
        }
        public float Mass
        {
            get => provider.Mass;
            set => provider.Mass = value;
        }
        public float LinearDamping
        {
            get => provider.LinearDamping;
            set => provider.LinearDamping = value;
        }
        public float AngularDamping
        {
            get => provider.AngularDamping;
            set => provider.AngularDamping = value;
        }
        public float Inertia
        {
            get => provider.Inertia;
            set => provider.Inertia = value;
        }
        public bool IsBullet
        {
            get => provider.IsBullet;
            set => provider.IsBullet = value;
        }
        public bool FixedRotation
        {
            get => provider.FixedRotation;
            set => provider.FixedRotation = value;
        }
        public Vector2 Gravity
        {
            get => provider.Gravity;
            set => provider.Gravity = value;
        }

        IRigidbodyProvider IRigidbody.RigidbodyProvider => provider;

        private IRigidbodyProvider provider;

        public Rigidbody()
        {
            provider = GameManager.Instance.Game.Services.GetService<IPhysicsProvider>()?.CreateRigidbodyProvider(this);

            if (provider == null)
                provider = new RigidbodyProvider();
        }

        protected override void OnEnabledChanged()
        {
            provider.Enabled = Enabled;

            base.OnEnabledChanged();
        }

        public void ApplyForce(Vector2 vector)
        {
            provider.ApplyForce(vector);
        }

        public void ApplyAngularImpulse(float impulse)
        {
            provider.ApplyAngularImpulse(impulse);
        }

        public void ApplyLinearImpulse(Vector2 vector)
        {
            provider.ApplyLinearImpulse(vector);
        }

        public void ApplyTorque(float torque)
        {
            provider.ApplyTorque(torque);
        }
    }
}
