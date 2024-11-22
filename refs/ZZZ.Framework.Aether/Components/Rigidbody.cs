using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Transforming;

namespace ZZZ.Framework.Components.Physics.Aether.Components
{
    [RequiredComponent<Transformer>]
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
        public bool IgnoreGravity
        {
            get => body.IgnoreGravity;
            set => body.IgnoreGravity = value;
        }

        public bool IsComposite
        { 
            get => isComposite;
            set => isComposite = value;
        }

        private Body body;
        private Body spoof;
        private bool isComposite;

        public Rigidbody()
        {
            body = new Body();
            body.BodyType = BodyType.Dynamic;
            body.Mass = 1f;
            body.LinearDamping = 1f;
            body.AngularDamping = 1f;
            body.IgnoreGravity = false;
            body.Enabled = Enabled;
        }

        void IRigidbody.Attach(Body spoof)
        {
            spoof.BodyType = body.BodyType;
            spoof.Mass = body.Mass;
            spoof.LinearDamping = body.LinearDamping;
            spoof.AngularDamping = body.AngularDamping;
            spoof.IgnoreGravity = body.IgnoreGravity;
            spoof.Enabled = body.Enabled;

            this.spoof = body;
            body = spoof;
        }

        void IRigidbody.Detach()
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

        public void ApplyForce(Vector2 vector)
        {
            body.ApplyForce(vector);
        }

        public void ApplyAngularImpulse(float impulse)
        {
            body.ApplyAngularImpulse(impulse);
        }

        public void ApplyLinearImpulse(Vector2 vector)
        {
            body.ApplyLinearImpulse(vector);
        }

        public void ApplyTorque(float torque)
        {
            body.ApplyTorque(torque);
        }
    }
}
