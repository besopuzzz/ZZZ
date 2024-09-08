using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Physics;

namespace ZZZ.Framework.Physics.Components
{
    [RequireComponent(typeof(BodyController))]
    public class Rigidbody : Component, IRigidbody
    {
        public Vector2 Velocity
        {
            get => body.LinearVelocity;
            set
            {
                if (value == body.LinearVelocity)
                    return;

                body.LinearVelocity = value;
            }
        }
        public float AngularVelocity
        {
            get => body.AngularVelocity;
            set
            {
                if (value == body.AngularVelocity)
                    return;

                body.AngularVelocity = value;
            }
        }
        public bool IsKinematic
        {
            get => body.BodyType == BodyType.Kinematic;
            set 
            {
                if (value)
                    body.BodyType = BodyType.Kinematic;
                else body.BodyType = BodyType.Dynamic;
            }
        }
        public float Mass
        {
            get => body.Mass;
            set
            {
                if (value == body.Mass)
                    return;

                body.Mass = value;
            }
        }
        public float LinearDamping
        {
            get => body.LinearDamping;
            set => body.LinearDamping = value;
        }
        public float AngularDamping
        {
            get => body.AngularDamping;
            set => body.AngularDamping = value;
        }
        public float Inertia
        {
            get => body.Inertia;
            set
            {
                if (value == body.Inertia)
                    return;

                body.Inertia = value;
            }
        }
        public bool IsBullet
        {
            get => body.IsBullet;
            set => body.IsBullet = value;
        }
        public bool FixedRotation
        {
            get => body.FixedRotation;
            set => body.FixedRotation = value;
        }
        public Vector2 Gravity { get; set; }


        private Body body;
        private Body spoof;

        public Rigidbody()
        { 
            body = new Body();
            body.BodyType = BodyType.Dynamic;
            body.Mass = 1f;
            body.LinearDamping = 1f;
            body.AngularDamping = 1f;
            body.IgnoreGravity = false;
            body.LocalCenter = Vector2.Zero;
            body.Enabled = Enabled;
        }

        protected override void OnEnabledChanged()
        {
            body.Enabled = Enabled;

            base.OnEnabledChanged();
        }

        public void ApplyForce(Vector2 vector)
        {
            body.ApplyForce(vector / IRigidbody.PixelsPerMeter);
        }
        public void ApplyAngularImpulse(float impulse)
        {
            body.ApplyAngularImpulse(impulse);
        }
        public void ApplyLinearImpulse(Vector2 vector)
        {
            body.ApplyLinearImpulse(vector / IRigidbody.PixelsPerMeter);
        }
        public void ApplyTorque(float torque)
        {
            body.ApplyTorque(torque);
        }

        void IPhysicBody.Attach(Body spoof)
        {
            spoof.BodyType = body.BodyType;
            spoof.Mass = body.Mass;
            spoof.LinearDamping = body.LinearDamping;
            spoof.AngularDamping = body.AngularDamping;
            spoof.IgnoreGravity = body.IgnoreGravity;
            spoof.LocalCenter = body.LocalCenter;
            spoof.Enabled = body.Enabled;

            this.spoof = body;
            body = spoof;
        }

        void IPhysicBody.Detach()
        {
            spoof.BodyType = body.BodyType;
            spoof.Mass = body.Mass;
            spoof.LinearDamping = body.LinearDamping;
            spoof.AngularDamping = body.AngularDamping;
            spoof.IgnoreGravity = body.IgnoreGravity;
            spoof.LocalCenter = body.LocalCenter;
            spoof.Enabled = body.Enabled;

            this.spoof = null;
            body = spoof;
        }
    }
}
