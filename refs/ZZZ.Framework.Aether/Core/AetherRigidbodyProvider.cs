using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Aether.Core
{
    internal sealed class AetherRigidbodyProvider : RigidbodyProvider
    {
        public override Vector2 Velocity
        {
            get
            {
                if (body != null)
                    return body.LinearVelocity;

                return base.Velocity;
            }
            set
            {
                base.Velocity = value;

                if (body == null)
                    return;

                body.LinearVelocity = value;
            }
        }
        public override float AngularVelocity
        {
            get
            {
                if (body != null)
                    return body.AngularVelocity;

                return base.AngularVelocity;
            }
            set
            {
                base.AngularVelocity = value;

                if (body == null)
                    return;

                body.AngularVelocity = value;
            }
        }
        public override bool IsKinematic
        {
            get
            {
                if (body != null)
                    return body.BodyType == BodyType.Kinematic;

                return base.IsKinematic;
            }
            set
            {
                base.IsKinematic = value;

                if (body == null)
                    return;

                body.BodyType = value ? BodyType.Kinematic : BodyType.Dynamic;
            }
        }
        public override float Mass
        {
            get
            {
                if (body != null)
                    return body.Mass;

                return base.Mass;
            }
            set
            {
                base.Mass = value;

                if (body == null)
                    return;

                body.Mass = value;
            }
        }
        public override float LinearDamping
        {
            get
            {
                if (body != null)
                    return body.LinearDamping;

                return base.LinearDamping;
            }
            set
            {
                base.LinearDamping = value;

                if (body == null)
                    return;

                body.LinearDamping = value;
            }
        }
        public override float AngularDamping
        {
            get
            {
                if (body != null)
                    return body.AngularDamping;

                return base.AngularDamping;
            }
            set
            {
                base.AngularDamping = value;

                if (body == null)
                    return;

                body.AngularDamping = value;
            }
        }
        public override float Inertia
        {
            get
            {
                if (body != null)
                    return body.Inertia;

                return base.Inertia;
            }
            set
            {
                base.Inertia = value;

                if (body == null)
                    return;

                body.Inertia = value;
            }
        }
        public override bool IsBullet
        {
            get
            {
                if (body != null)
                    return body.IsBullet;

                return base.IsBullet;
            }
            set
            {
                base.IsBullet = value;

                if (body == null)
                    return;

                body.IsBullet = value;
            }
        }
        public override bool FixedRotation
        {
            get
            {
                if (body != null)
                    return body.FixedRotation;

                return base.FixedRotation;
            }
            set
            {
                base.FixedRotation = value;

                if (body == null)
                    return;

                body.FixedRotation = value;
            }
        }
        public override Vector2 Gravity
        {
            get
            {
                if (body != null)
                    return body.Gravity;

                return base.Gravity;
            }
            set
            {
                base.Gravity = value;

                if (body == null)
                    return;

                body.Gravity = value;
            }
        }
        public override bool Enabled 
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;

                if (body == null)
                    return;

                if (Enabled)
                    body.BodyType = (IsKinematic ? BodyType.Kinematic : BodyType.Dynamic);
                else body.BodyType = BodyType.Static;
            }
        }

        private PhysicalBody body;

        public AetherRigidbodyProvider(IRigidbody rigidbody)
        {

        }

        public void Attach(PhysicalBody body)
        {
            body.LinearVelocity = Velocity;
            body.AngularVelocity = AngularVelocity;
            body.BodyType = IsKinematic ? BodyType.Kinematic : BodyType.Dynamic;
            body.LinearDamping = LinearDamping;
            body.AngularDamping = AngularDamping;
            body.Inertia = Inertia;
            body.IsBullet = IsBullet;
            body.FixedRotation = FixedRotation;
            body.IgnoreGravity = true;
            body.LocalCenter = Vector2.Zero;
            body.Gravity = Gravity;
            body.Mass = Mass;
            this.body = body;
        }

        public void Detach()
        {
            body.BodyType = BodyType.Static;
            body = null;
        }

        public override void ApplyAngularImpulse(float impulse)
        {
            if (body == null)
                return;

            body.ApplyAngularImpulse(impulse);
        }

        public override void ApplyForce(Vector2 vector)
        {
            if (body == null)
                return;

            body.ApplyForce(vector);
        }

        public override void ApplyLinearImpulse(Vector2 vector)
        {
            if (body == null)
                return;

            body.ApplyLinearImpulse(vector);
        }

        public override void ApplyTorque(float torque)
        {
            if (body == null)
                return;

            body.ApplyTorque(torque);
        }
    }
}
