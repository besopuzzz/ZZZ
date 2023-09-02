using Microsoft.Xna.Framework.Graphics;

namespace ZZZ.Framework.Monogame.Rendering.Components
{
    public interface IShaderComponent : IRenderComponent
    {
        void Draw(RenderBatch renderBatch, RenderTarget2D renderTarget);
    }
}
