using Microsoft.Xna.Framework;

namespace ZZZ.Framework.Monogame.Rendering.Components
{
    public interface ICamera : IRenderComponent
    {
        Matrix Matrix { get; }
    }
}
