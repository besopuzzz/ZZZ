using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Components
{
    public class BoxCollider : Collider
    {
        public Vector2 Size
        {
            get => size;
            set
            {
                if (size == value)
                    return;

                size = value;

                if (fixture == null)
                    return;

                Rebuild();
            }
        }
        public event EventHandler<ColliderEventArgs> Collision;

        private Fixture fixture = null!;
        private Vector2 size = Vector2.Zero;

        protected override void Create()
        {
            fixture = Body.CreateRectangle(Size.X, Size.Y, 0.01f, Offset.ToAether());
            fixture.IsSensor = IsTrigger;
            fixture.Friction = Material.Friction;
            fixture.Restitution = Material.Restitution;
            fixture.CollisionCategories = Category;
            fixture.CollidesWith = Category;
            fixture.Tag = this;

            fixture.OnCollision = OnCollision;
        }

        protected override void Clear()
        {
            fixture.OnCollision = null;
            fixture.Body.Remove(fixture);
        }

        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (Collision == null)
                return true;

            if (sender == fixture)
            {
                var args = new ColliderEventArgs((Collider)other.Tag);

                Collision?.Invoke(this, args);

                return !args.Ignore;
            }

            return true;
        }

        protected override void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        {
            if (fixture == null)
                return;

            var shape = fixture.Shape as PolygonShape;

            shape?.Vertices.Translate(oldValue.ToAether());
            shape?.Vertices.Translate(-newValue.ToAether());
        }

        protected override void OnIsTriggerChanged()
        {
            if (fixture == null)
                return;

            fixture.IsSensor = IsTrigger;
        }

        protected override void OnMaterialChanged()
        {
            if (fixture == null)
                return;

            fixture.Friction = Material.Friction;
            fixture.Restitution = Material.Restitution;
        }

        protected override void OnLayerChanged()
        {
            if (fixture == null)
                return;

            fixture.CollisionCategories = Category;
            fixture.CollidesWith = Category;

            base.OnLayerChanged();
        }
    }
}
