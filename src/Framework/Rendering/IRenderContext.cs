using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Rendering
{
    public interface IRenderContext
    {
        void RenderSprite(Transform2D transform, Sprite sprite, Color color, SpriteEffects spriteEffect);
    }
}
