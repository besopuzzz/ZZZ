using ZZZ.Framework.Core;

namespace ZZZ.Framework.Components.Physics.Providers
{
    public interface IPhysicsProvider : ISystemProvider
    {
        IRigidbodyProvider CreateRigidbodyProvider(IRigidbody rigidbody);
        TProvider CreateColliderProvider<TProvider>(ICollider collider) where TProvider : class, IColliderProvider;
    }
}
