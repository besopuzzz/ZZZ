using ChipmunkSharp;
using ZZZ.Framework.Components.Physics;

namespace ZZZ.Framework.Chipmunk.Core
{
    internal class RigidbodyEntityComponent : PhysicalEntityComponent<IRigidbody>
    {
        public RigidbodyEntityComponent(cpSpace cpSpace, IRigidbody component) : base(cpSpace, component)
        {
        }
    }
}
