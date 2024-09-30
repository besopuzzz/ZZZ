using ChipmunkSharp;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Chipmunk.Core
{
    public abstract class PhysicalEntityComponent : EntityComponent<PhysicalEntity, PhysicalEntityComponent, IPhysicBody>
    {
        public PhysicalEntityComponent(cpSpace cpSpace, IPhysicBody component) : base(component)
        {
        }
    }

    public abstract class PhysicalEntityComponent<T> : PhysicalEntityComponent
        where T : IPhysicBody
    {
        protected PhysicalEntityComponent(cpSpace cpSpace, T component) : base(cpSpace, component)
        {
        }
    }
}
