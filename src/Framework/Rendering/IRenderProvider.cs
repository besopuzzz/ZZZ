using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Rendering
{
    public interface IRenderProvider
    {
        void RenderText(SpriteFont font, string text, Transform2D transform, Color color, Vector2 origin, SpriteEffects spriteEffect);
        void RenderSprite(Transform2D transform, Sprite sprite, Color color, SpriteEffects spriteEffect);
    }
}
