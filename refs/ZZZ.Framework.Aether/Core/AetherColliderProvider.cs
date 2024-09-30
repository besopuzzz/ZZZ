using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Aether.Core
{
    internal abstract class AetherColliderProvider<TShape> : AetherColliderProvider
        where TShape : Shape
    {
        protected new TShape Shape => base.Shape as TShape;

        protected AetherColliderProvider(ICollider collider, TShape shape) : base(collider, shape)
        {
        }
    }

    internal abstract class AetherColliderProvider : ColliderProvider
    {
        public override Vector2 Offset
        {
            get => base.Offset;
            set
            {
                var lastOffset = base.Offset;

                if (base.Offset == value)
                    return;

                base.Offset = value;

                ApplyScaledOffset((value - lastOffset) / PhysicalBody.PixelsPerMeter);
            }
        }

        public override bool IsTrigger
        {
            get => fixture.IsSensor;
            set
            {
                if (base.IsTrigger == value)
                    return;

                fixture.IsSensor = value;
            }
        }

        public override ColliderLayer Layer
        {
            get => base.Layer;
            set
            {
                if (base.Layer == value)
                    return;

                base.Layer = value;

                if (body != null)
                    fixture.CollidesWith = (Category)Enum.ToObject(typeof(Category), value);
            }
        }

        public override float Friction 
        {
            get => fixture.Friction;
            set
            {
                fixture.Friction = value;
            }
        }

        public override float Restitution 
        { 
            get => fixture.Restitution;
            set
            {
                fixture.Restitution = value;
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

                if (fixture.Body != null)
                {
                    if (!value)
                        body.Remove(fixture);
                }
                else if (value)
                    body.Add(fixture);
            }
        }

        protected Shape Shape => fixture.Shape;

        private PhysicalBody body;
        private Fixture fixture;
        private ICollider collider;

        public AetherColliderProvider(ICollider collider, Shape shape)
        {
            this.collider = collider;
            fixture = (Fixture)Activator.CreateInstance(typeof(Fixture), true);
            fixture.Tag = this;

            Type type = typeof(Fixture);

            type.GetProperty(nameof(fixture.Shape)).SetValue(fixture, shape);
            type.GetProperty(nameof(fixture.Proxies)).SetValue(fixture, new FixtureProxy[shape.ChildCount]);

            fixture.OnCollision = OnCollision;
            fixture.OnSeparation = OnSeparation;
        }

        private void OnSeparation(Fixture sender, Fixture other, Contact contact)
        {
            if (sender == fixture & sender.IsSensor)
                InvokeExit(collider, other.Tag as ICollider);
        }

        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (sender == fixture & sender.IsSensor & contact.IsTouching & Enabled)
                InvokeEnter(collider, other.Tag as ICollider);

            return Enabled;
        }

        public void Attach(PhysicalBody body)
        {
            this.body = body;

            body.Add(fixture);

            ComputeBody();

            fixture.CollidesWith = (Category)Enum.ToObject(typeof(Category), Layer);
        }

        public void Detach()
        {
            body.Remove(fixture);
            body = null;
        }

        protected void ComputeBody()
        {
            if (body == null)
                return;

            body.Awake = true;
            body.ComputeMass();
        }

        protected abstract void ApplyScaledOffset(Vector2 offset);
    }
}
