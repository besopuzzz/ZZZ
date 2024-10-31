using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Aether.Core;
using ZZZ.Framework.Components.Physics.Aether.Components;

namespace ZZZ.Framework.Components.Physics.Aether.Core.Entities
{
    internal class ColliderEntityComponent : PhysicalEntityComponent<ICollider>
    {
        public bool UseComposite => Component.UseComposite;

        public ColliderEntityComponent(ICollider component) : base(component)
        {
        }
    }
}
