using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Components
{
    public interface IBody : IComponent
    {
        World World { get; set; }
    }
}
