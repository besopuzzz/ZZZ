using ChipmunkSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Chipmunk.Core;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Chipmunk
{
    public class PhysicalSystem : System<PhysicalEntity, PhysicalEntityComponent, IPhysicBody>, IPhysicsProvider
    {
        private cpSpace cpSpace = new cpSpace();

        public IRigidbodyProvider CreateRigidbodyProvider(IRigidbody rigidbody)
        {

        }

        protected override PhysicalEntity CreateEntity(PhysicalEntity owner)
        {
            return new PhysicalEntity();
        }

        protected override PhysicalEntityComponent CreateEntityComponent(PhysicalEntity owner, IPhysicBody component)
        {
            if(component is IRigidbody body)
                return new RigidbodyEntityComponent(cpSpace, body);

            if (component is ICollider collider)
                return new ColliderEntityComponent(cpSpace, collider);

            return default;
        }

        TProvider IPhysicsProvider.CreateColliderProvider<TProvider>(ICollider collider)
        {
            throw new NotImplementedException();
        }
    }
}
