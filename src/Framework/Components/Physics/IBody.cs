using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Components.Physics
{
    /// <summary>
    /// Представляет интерфейс любого физического компонента, который участвует в <see cref="PhysicRegistrar"/>.
    /// </summary>
    public interface IPhysicBody : IComponent
    {

    }

    /// <summary>
    /// Представляет интерфейс физического объекта, на который может действовать скорость.
    /// </summary>
    public interface IRigidbody : IPhysicBody
    {
        IRigidbodyProvider RigidbodyProvider { get; }
    }

    /// <summary>
    /// Представляет интерфейс жесткого физического коллайдера, на который могут взаимодействовать другие объекты <see cref="ICollider"/>.
    /// </summary>
    public interface ICollider : IPhysicBody
    {
        IColliderProvider ColliderProvider { get; }        
    }
}
