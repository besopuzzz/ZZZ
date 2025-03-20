namespace ZZZ.Framework.Physics.Aether.Components
{
    public interface ICollider : IRigidbody
    {
        bool UseComposite { get; }
    }
}
