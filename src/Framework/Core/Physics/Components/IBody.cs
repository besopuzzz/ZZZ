using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Physics.Components
{
    /// <summary>
    /// Представляет интерфейс помогающего физического компонента Aether.
    /// </summary>
    public interface IBody : IComponent
    {
        internal Body Body { get; }
        internal void UpdateBody();
        internal void UpdateTransformer();
    }

    /// <summary>
    /// Представляет интерфейс любого физического компонента, который участвует в <see cref="PhysicRegistrar"/>.
    /// </summary>
    public interface IPhysicBody : IComponent
    {
        const float PixelsPerMeter = 64f;
        void Attach(Body body);
        void Detach();
    }

    /// <summary>
    /// Представляет интерфейс физического объекта, на который может действовать скорость.
    /// </summary>
    public interface IRigidbody : IPhysicBody
    {
        Vector2 Gravity { get; }
    }

    /// <summary>
    /// Представляет интерфейс жесткого физического коллайдера, на который могут взаимодействовать другие объекты <see cref="ICollider"/>.
    /// </summary>
    public interface ICollider : IPhysicBody
    {

    }
}
