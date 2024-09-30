using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Core.Rendering.Entities
{
    public interface IRenderContext : IDisposable
    {
        Rectangle Viewport { get; }
        Sprite Pixel { get; }
        void Render(Transform2D world, Sprite sprite, Color color, bool flipX, bool flipY);
        void DrawSprite(Sprite sprite, Transform2D world, Color color, SpriteEffects spriteEffects);
        void DrawText(SpriteFont font, string text, Transform2D world, Color color, Vector2 origin, SpriteEffects effects, bool rtl);
        IRenderContext Copy(IRenderContext context);
    }
}