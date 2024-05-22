using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Core.Rendering
{
    public interface IRenderProvider
    {

        void StartRendering();
        void Render(Sprite sprite, float positionX, float positionY,
            float rotation, float scaleX, float scaleY, uint color, bool flipX, bool flipY, float order);
        void RenderText(string text, float positionX, float positionY,
            float rotation, float scaleX, float scaleY, float originX, float originY, uint color, bool flipX, bool flipY, float order);
        void EndRendering();
    }
}