using ZZZ.Framework.Core;

namespace ZZZ.Framework.Components.Physics.Providers
{
    public interface IColliderProvider : IComponentProvider
    { 
        Vector2 Offset { get; set; }
        bool IsTrigger { get; set; }
        ColliderLayer Layer { get; set; }
        float Friction { get; set; }
        float Restitution { get; set; }

        event ColliderEvent ColliderEnter;
        event ColliderEvent ColliderExit;
    }


}
