namespace ZZZ.Framework.Components.Physics.Providers
{
    public class RigidbodyProvider : IRigidbodyProvider
    {
        public virtual bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value)
                    return;

                enabled = value;
            }
        }
        public virtual Vector2 Velocity
        {
            get => linearVelocity;
            set
            {
                if (value == linearVelocity)
                    return;

                linearVelocity = value;
            }
        }
        public virtual float AngularVelocity
        {
            get => angularVelocity;
            set
            {
                if (value == angularVelocity)
                    return;

                angularVelocity = value;
            }
        }
        public virtual bool IsKinematic
        {
            get => isKinematic;
            set
            {
                if (isKinematic == value)
                    return;

                isKinematic = value;
            }
        }
        public virtual float Mass
        {
            get => mass;
            set
            {
                if (value == mass)
                    return;

                mass = value;
            }
        }
        public virtual float LinearDamping
        {
            get => linearDamping;
            set
            {
                if (value == linearDamping)
                    return;

                linearDamping = value;
            }
        }
        public virtual float AngularDamping
        {
            get => angularDamping;
            set
            {
                if (value == angularDamping)
                    return;

                angularDamping = value;
            }
        }
        public virtual float Inertia
        {
            get => inertia;
            set
            {
                if (value == inertia)
                    return;

                inertia = value;
            }
        }
        public virtual bool IsBullet
        {
            get => isBullet;
            set
            {
                if (value == isBullet)
                    return;

                isBullet = value;
            }
        }
        public virtual bool FixedRotation
        {
            get => fixedRotation;
            set
            {
                if (value == fixedRotation)
                    return;

                fixedRotation = value;
            }
        }
        public virtual Vector2 Gravity
        {
            get => gravity;
            set
            {
                if (value == gravity)
                    return;

                gravity = value;
            }
        }

        private bool enabled = true;
        private bool isKinematic = false;
        private float angularVelocity = 0f;
        private float mass = 10f;
        private float linearDamping = 64f;
        private float angularDamping = 64f;
        private float inertia = 0.1f;
        private Vector2 linearVelocity;
        private bool isBullet;
        private bool fixedRotation;
        private Vector2 gravity = new Vector2(0f, 9.8f);

        public virtual void ApplyForce(Vector2 vector)
        {

        }

        public virtual void ApplyAngularImpulse(float impulse)
        {

        }

        public virtual void ApplyLinearImpulse(Vector2 vector)
        {

        }

        public virtual void ApplyTorque(float torque)
        {

        }
    }

}