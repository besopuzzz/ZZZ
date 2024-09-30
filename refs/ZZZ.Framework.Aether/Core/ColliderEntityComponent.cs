using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Aether.Core
{
    internal class ColliderEntityComponent : PhysicalEntityComponent<ICollider>
    {
        public AetherColliderProvider ColliderProvider => Component.ColliderProvider as AetherColliderProvider;

        public ColliderEntityComponent(ICollider component) : base(component)
        {
        }

        internal override void Attach(PhysicalBody body)
        {
            ColliderProvider.Attach(Owner.Body);
        }

        internal override void Detach(PhysicalBody body)
        {
            ColliderProvider.Detach();
        }
    }
}
