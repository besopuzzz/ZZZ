using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Physics.Aether;
using ZZZ.Framework.Components.Physics.Aether.Components;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Aether.Core
{
    public class PhysicalEntityComponent<TBody> : PhysicalEntityComponent
        where TBody : IRigidbody
    {
        public new TBody Component
        {
            get => (TBody)base.Component;
        }

        public PhysicalEntityComponent(TBody component) : base(component)
        {
        }
    }

    public class PhysicalEntityComponent : EntityComponent<PhysicalEntity, PhysicalEntityComponent, IRigidbody>
    {
        public PhysicalEntityComponent(IRigidbody component) : base(component)
        {

        }

        public override void Foreach(Func<PhysicalEntityComponent> func)
        {

        }

        public virtual void Attach(Body body)
        {
            Component.Attach(body);
        }
        public virtual void Detach(Body body)
        {
            Component.Detach();
        }
    }
}
