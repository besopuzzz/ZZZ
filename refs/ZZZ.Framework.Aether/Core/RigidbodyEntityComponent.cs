using ZZZ.Framework.Components.Physics;

namespace ZZZ.Framework.Aether.Core
{
    internal sealed class RigidbodyEntityComponent : PhysicalEntityComponent<IRigidbody>
    {
        public AetherRigidbodyProvider RigidbodyProvider => Component.RigidbodyProvider as AetherRigidbodyProvider;

        public RigidbodyEntityComponent(IRigidbody component) : base(component)
        {
        }


        protected override void Initialize()
        {
        }

        internal override void Attach(PhysicalBody body)
        {
            RigidbodyProvider.Attach(Owner.Body);
        }

        internal override void Detach(PhysicalBody body)
        {
            RigidbodyProvider.Detach();
        }
    }
}
