using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Aether.Core;
using ZZZ.Framework.Components.Physics.Aether.Components;

namespace ZZZ.Framework.Components.Physics.Aether.Core.Entities
{
    internal class RigidbodyEntityComponent : PhysicalEntityComponent<IRigidbody>
    {
        public Body Body => Owner.Body;

        public RigidbodyEntityComponent(IRigidbody component) : base(component)
        {
        }

        public override void Attach(Body body)
        {
            base.Attach(body);


        }
    }
}
