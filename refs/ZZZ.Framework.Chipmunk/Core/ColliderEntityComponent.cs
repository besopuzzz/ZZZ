using ChipmunkSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Components.Physics;

namespace ZZZ.Framework.Chipmunk.Core
{
    internal class ColliderEntityComponent : PhysicalEntityComponent<ICollider>
    {


        public ColliderEntityComponent(cpSpace cpSpace, ICollider component) : base(cpSpace, component)
        {
        }
    }
}
