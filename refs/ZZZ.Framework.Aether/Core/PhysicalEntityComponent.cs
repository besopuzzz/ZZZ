using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Aether.Core
{
    public abstract class PhysicalEntityComponent<TBody> : PhysicalEntityComponent
        where TBody : IPhysicBody
    {
        public new TBody Component
        {
            get => (TBody)base.Component;
        }

        public PhysicalEntityComponent(TBody component) : base(component)
        {
        }
    }

    public abstract class PhysicalEntityComponent : EntityComponent<PhysicalEntity, PhysicalEntityComponent, IPhysicBody>
    {
        public PhysicalEntityComponent(IPhysicBody component) : base(component)
        {

        }

        public override void Foreach(Func<PhysicalEntityComponent> func)
        {

        }

        internal abstract void Attach(PhysicalBody body);
        internal abstract void Detach(PhysicalBody body);
    }
}
