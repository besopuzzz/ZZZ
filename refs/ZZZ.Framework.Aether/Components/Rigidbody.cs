using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Physics.Aether.Components
{
    [RequiredComponent<BodyComponent>]
    public class Rigidbody : Component
    {
        public Vector2 Velocity
        {
            get => body.LinearVelocity * BodyComponent.PixelsPerMeter;
            set
            {
                body.LinearVelocity = value / BodyComponent.PixelsPerMeter;
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
            get => body.LinearDamping * BodyComponent.PixelsPerMeter;
            set => body.LinearDamping = value / BodyComponent.PixelsPerMeter;
        }
        public float AngularDamping
        {
            get => body.AngularDamping * BodyComponent.PixelsPerMeter;
            set => body.AngularDamping = value / BodyComponent.PixelsPerMeter;
        }
        public float Inertia
        {
            get => body.Inertia * BodyComponent.PixelsPerMeter;
            set
            {
                body.Inertia = value / BodyComponent.PixelsPerMeter;
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
        public override bool Enabled 
        {
            get => true;
            set { return; } 
        }

        private Body body => bodyComponent.Body;
        private BodyComponent bodyComponent;

        protected override void OnCreated()
        {
            bodyComponent = GetComponent<BodyComponent>();
            bodyComponent.Body.BodyType = BodyType.Dynamic;
        }

        protected override void OnDestroy()
        {
            bodyComponent.Body.BodyType = BodyType.Static;

            base.OnDestroy();
        }

        public void ApplyForce(Vector2 vector)
        {
            body.ApplyForce(vector / BodyComponent.PixelsPerMeter);
        }

        public void ApplyAngularImpulse(float impulse)
        {
            body.ApplyAngularImpulse(impulse / BodyComponent.PixelsPerMeter);
        }

        public void ApplyLinearImpulse(Vector2 vector)
        {
            body.ApplyLinearImpulse(vector / BodyComponent.PixelsPerMeter);
        }

        public void ApplyTorque(float torque)
        {
            body.ApplyTorque(torque);
        }
    }
}
