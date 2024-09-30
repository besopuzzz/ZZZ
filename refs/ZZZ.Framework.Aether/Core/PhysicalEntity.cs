using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Aether.Core
{
    public class PhysicalEntity : Entity<PhysicalEntity, PhysicalEntityComponent, IPhysicBody>
    {
        internal PhysicalBody Body => body;

        public PhysicalEntity Main => main;

        private World world;
        private PhysicalBody body;
        private Transformer transformer;
        private PhysicalEntity main;

        public PhysicalEntity(World world)
        {
            this.world = world;

            body = new PhysicalBody();
        }

        protected override void Initialize()
        {
            transformer = Owner.GetComponent<Transformer>();

            base.Initialize();
        }

        protected override void EntityAdded(PhysicalEntity entity)
        {
            base.EntityAdded(entity);
        }

        protected override void EntityRemoved(PhysicalEntity entity)
        {
            base.EntityRemoved(entity);
        }

        protected override void ComponentEntityAdded(PhysicalEntityComponent entityComponent)
        {
            
        }

        protected override void ComponentEntityRemoved(PhysicalEntityComponent entityComponent)
        {
            if (EntityComponents.Count == 0)
                world.Remove(body);
        }

        

        public void UpdateBody()
        {
            body.UpdatePosition(transformer);
        }

        public void UpdateTransformer()
        {
            body.UpdateTransformer(transformer);
        }
    }
}
