using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Core.Physics;

namespace ZZZ.Framework.Physics.Components
{
    public delegate void ColliderEvent(Collider sender, Collider other);

    public abstract class Collider<T> : Collider
        where T : Shape
    {
        protected new T Shape => (T)base.Shape;

        protected Collider(T shape) : base(shape)
        {
        }
    }

    [RequireComponent(typeof(BodyController))]
    public abstract class Collider : Component, ICollider
    {

        public Vector2 Offset
        {
            get => offset;
            set
            {
                if (offset == value)
                    return;

                var old = offset;

                offset = value;

                OnOffsetChanged(old, offset);
            }
        }
        public bool IsTrigger
        {
            get => fixture.IsSensor;
            set => fixture.IsSensor = value;
        }
        public ColliderLayer Layer
        {
            get => layer;
            set
            {
                layer = value;

                if (body != null)
                    fixture.CollidesWith = (Category)(int)value;
            }
        }
        public float Friction
        {
            get => fixture.Friction;
            set => fixture.Friction = value;
        }

        /// <summary>
        /// Упругость материала.
        /// </summary>
        public float Restitution
        {
            get => fixture.Restitution;
            set => fixture.Restitution = value;
        }

        /// <summary>
        /// Плотность материала.
        /// </summary>
        public float Density
        {
            get => shape.Density;
            set => shape.Density = value;
        }

        protected virtual Shape Shape => shape;

        public event ColliderEvent ColliderEnter;
        public event ColliderEvent ColliderExit;

        private ColliderLayer layer = ColliderLayer.Cat1;
        private readonly Shape shape;
        private Fixture fixture;
        private Vector2 offset;
        private Body body;

        protected Collider(Shape shape)
        {
            fixture = (Fixture)Activator.CreateInstance(typeof(Fixture), true); 

            Type type = typeof(Fixture);

            type.GetProperty(nameof(fixture.Shape)).SetValue(fixture, shape);
            type.GetProperty(nameof(fixture.Proxies)).SetValue(fixture, new FixtureProxy[shape.ChildCount]);

            fixture.Tag = this;
            fixture.OnCollision = OnCollision;
            fixture.OnSeparation = OnSeparation;

            this.shape = shape;
        }

        private void OnSeparation(Fixture sender, Fixture other, Contact contact)
        {
            if (sender == fixture & sender.IsSensor)
                ColliderExit?.Invoke(this, other.Tag as Collider);

        }
        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (sender == fixture & sender.IsSensor & contact.IsTouching & Enabled)
                ColliderEnter?.Invoke(this, other.Tag as Collider);

            return Enabled;
        }

        protected abstract void OnOffsetChanged(Vector2 oldOffset, Vector2 offset);

        void IPhysicBody.Attach(Body body)
        {
            this.body = body;
            body.Add(fixture);

            body.LocalCenter = Vector2.Zero;
            
            fixture.CollidesWith = (Category)(int)layer; // Layer change throw error if Body is null...
        }

        void IPhysicBody.Detach()
        {
            body.Remove(fixture);
            body = null;
        }
    }

}
