using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Aether
{
    internal sealed class PhysicalBody : Body
    {
        public new Vector2 LinearVelocity
        {
            get
            {
                return base.LinearVelocity * PixelsPerMeter;
            }
            set
            {
                base.LinearVelocity = value / PixelsPerMeter;
            }
        }
        public new float AngularVelocity
        {
            get
            {
                return base.AngularVelocity * PixelsPerMeter;
            }
            set
            {
                base.AngularVelocity = value / PixelsPerMeter;
            }
        }
        public bool IsKinematic
        {
            get
            {
                return BodyType == BodyType.Kinematic;
            }
            set
            {
                BodyType = value ? BodyType.Kinematic : BodyType.Dynamic;
            }
        }
        public new float LinearDamping
        {
            get
            {
                return base.LinearDamping * PixelsPerMeter;
            }
            set
            {
                base.LinearDamping = value / PixelsPerMeter;
            }
        }
        public new float AngularDamping
        {
            get
            {
                return base.AngularDamping * PixelsPerMeter;
            }
            set
            {
                base.AngularDamping = value / PixelsPerMeter;
            }
        }
        public new float Inertia
        {
            get
            {
                return base.Inertia * PixelsPerMeter;
            }
            set
            {
                base.Inertia = value / PixelsPerMeter;
            }
        }
        public new float Mass
        {
            get => mass;
            set
            {
                mass = value;

                ComputeMass();
            }
        }
        public Vector2 Gravity
        {
            get
            {
                return gravity * PixelsPerMeter;
            }
            set
            {
                gravity = value / PixelsPerMeter;
            }
        }

        public const float PixelsPerMeter = 64f;

        private float mass = 1f;
        private Vector2 gravity;

        public void UpdatePosition(Transformer transformer)
        {
            if (World == null || transformer == null)
                return;

            if (Awake && Enabled)
            {
                LinearVelocity += gravity;
            }

            Position = transformer.World.Position / transformer.World.Scale / PixelsPerMeter;
            Rotation = transformer.World.Rotation;
        }

        public void UpdateTransformer(Transformer transformer)
        {
            if (World == null || transformer == null)
                return;

            Transform2D world = new Transform2D();

            world.Position = Position * PixelsPerMeter * transformer.World.Scale;
            world.Scale = transformer.World.Scale;
            world.Rotation = Rotation;

            transformer.World = world;
        }

        public new void ApplyAngularImpulse(float impulse)
        {
            base.ApplyAngularImpulse(impulse / PhysicalBody.PixelsPerMeter);
        }

        public new void ApplyForce(Vector2 vector)
        {
            base.ApplyForce(vector / PhysicalBody.PixelsPerMeter);
        }

        public new void ApplyLinearImpulse(Vector2 vector)
        {
            base.ApplyLinearImpulse(vector / PhysicalBody.PixelsPerMeter);
        }

        public new void ApplyTorque(float torque)
        {
            base.ApplyTorque(torque / PhysicalBody.PixelsPerMeter);
        }

        public void ComputeMass()
        {
            Awake = true;

            if (FixtureList.Count == 0)
            {
                base.Mass = 1f;

                return;
            }

            var fixtures = FixtureList.Where(x => x.Shape.MassData.Area > 0).ToArray();

            float massOneFixture = mass / fixtures.Length;

            foreach (var item in fixtures)
            {
                item.Shape.Density = massOneFixture / item.Shape.MassData.Area;
            }
        }
    }
}
