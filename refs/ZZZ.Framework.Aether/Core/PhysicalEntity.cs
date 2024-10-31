using nkast.Aether.Physics2D.Dynamics;
using System;
using ZZZ.Framework.Components.Physics.Aether;
using ZZZ.Framework.Components.Physics.Aether.Components;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Aether.Core
{
    public class PhysicalEntity : Entity<PhysicalEntity, PhysicalEntityComponent, IRigidbody>
    {
        public Body Body => body;
        public bool IsComposite
        {
            get { return rigidbody == null ? false : false; }
        }

        private Body body;
        private Transformer transformer;
        private List<PhysicalEntityComponent> colliders;
        private PhysicalEntityComponent rigidbody;
        private World world;

        public PhysicalEntity(World world)
        {
            this.world = world;

            //body = new Body();
            //body.BodyType = BodyType.Static;
            colliders = new List<PhysicalEntityComponent>();
        }

        protected override void Initialize()
        {


            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                colliders.Clear();
            }

            base.Dispose(disposing);
        }

        protected override void ComponentEntityAdded(PhysicalEntityComponent entityComponent)
        {
            if (body?.World == null)
            {
                transformer = Owner.GetComponent<Transformer>();
                body = new Body(transformer);
                //body.Position = (transformer.World.Position / transformer.World.Scale) / IRigidbody.PixelsPerMeter;
                //body.Rotation = transformer.World.Rotation;

                world.Add(body);
            }

            entityComponent.Attach(body);

            if (entityComponent.Component is ICollider collider)
            {
                colliders.Add(entityComponent);
            }
            else
            {
                rigidbody = entityComponent;
            }
        }



        protected override void ComponentEntityRemoved(PhysicalEntityComponent entityComponent)
        {
            entityComponent.Detach(body);

            if (entityComponent.Component is ICollider collider)
            {
                colliders.Remove(entityComponent);
            }
            else
            {
                rigidbody = null;
                body.BodyType = BodyType.Static;
            }

            if (colliders.Count == 0 & rigidbody == null)
            {
                world.Remove(body);
                transformer = null;
            }
        }
        
        public void UpdateBody()
        {
            if (transformer == null)
                return;

            body.Position = (transformer.World.Position / transformer.World.Scale) / IRigidbody.PixelsPerMeter;
            body.Rotation = transformer.World.Rotation;

            if (IsComposite)
            {
                Childs.ForEach(x=>x.UpdateBody());
            }
        }

        public override void ForEveryChild(Action<PhysicalEntity> action)
        {
            if(IsComposite)
                action.Invoke(this);
            else base.ForEveryChild(action);
        }

        public void UpdateTransformer()
        {
            if (transformer == null)
                return;

            Transform2D world = new(body.Position * IRigidbody.PixelsPerMeter * transformer.World.Scale, transformer.World.Scale, body.Rotation);

            if (world == transformer.World)
                return;

            body.LocalCenter = Vector2.Zero;
            transformer.World = world;
        }
    }
}
