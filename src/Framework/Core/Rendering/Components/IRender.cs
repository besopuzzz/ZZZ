using ZZZ.Framework.Core.Rendering.Entities;

namespace ZZZ.Framework.Core.Rendering.Components
{
    public interface IRender : IGraphics
    {
        int Order { get; }
        void Render(SpriteBatch spriteBatch);
    }
}
