namespace ZZZ.Framework.Components.Physics.Aether.Components
{
    public interface ICollider : IRigidbody
    {
        bool UseComposite { get; }
    }
}
