using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Physics.Components;

namespace ZZZ.Framework.Components.Physics
{
    [RequireComponent(typeof(Transformer))]
    internal sealed class BodyController : Component, IBody
    {
        Body IBody.Body => body;

        private readonly Body body;
        private Transformer transformer;
        private List<ICollider> colliders;
        private IRigidbody rigidbody;

        public BodyController()
        {
            body = new Body();
            body.IgnoreGravity = true;
            body.BodyType = BodyType.Static;
            colliders = new List<ICollider>();
        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();
            rigidbody = GetComponent<Rigidbody>();

            colliders.AddRange(GetComponents<ICollider>());

            if (rigidbody != null)
                rigidbody.Attach(body);

            foreach (var item in colliders)
                item.Attach(body);

            Owner.ComponentAdded += Owner_ComponentAdded;
            Owner.ComponentRemoved += Owner_ComponentRemoved;

            base.Awake();
        }

        protected override void Shutdown()
        {
            Owner.ComponentAdded -= Owner_ComponentAdded;
            Owner.ComponentRemoved -= Owner_ComponentRemoved;

            if (rigidbody != null)
                rigidbody.Detach();

            foreach (var item in colliders)
                item.Detach();

            colliders.Clear();

            base.Shutdown();
        }

        private void Owner_ComponentAdded(GameObject sender, IComponent e)
        {
            if (e is not IPhysicBody rb)
                return;

            rb.Attach(body);

            if(rb is ICollider collider)
            {
                colliders.Add(collider);
            }
            else
            {
                rigidbody = (IRigidbody)rb;
            }
        }

        private void Owner_ComponentRemoved(GameObject sender, IComponent e)
        {
            if (e is not IPhysicBody rb)
                return;

            rb.Detach();

            if (rb is ICollider collider)
            {
                colliders.Remove(collider);
            }
            else
            {
                rigidbody = null;
                body.BodyType = BodyType.Static;
            }
        }

        void IBody.UpdateBody()
        {
            body.Position = (transformer.World.Position / transformer.World.Scale) / IRigidbody.PixelsPerMeter;
            body.Rotation = transformer.World.Rotation;

            if (rigidbody != null && rigidbody.Gravity != Vector2.Zero && body.Awake && body.Enabled)
            {
                body.LinearVelocity += (rigidbody.Gravity) / IRigidbody.PixelsPerMeter;
            }
        }

        void IBody.UpdateTransformer()
        {
            Transform2D world = new Transform2D();
            world.Position = body.Position * IRigidbody.PixelsPerMeter * transformer.World.Scale;
            world.Scale = transformer.World.Scale;
            world.Rotation = body.Rotation;

            transformer.Local = world / transformer.World * transformer.Local;
        }

    }
}
