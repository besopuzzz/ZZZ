using Microsoft.Xna.Framework;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.KNI
{
    internal sealed class KNIRenderer : DrawableGameComponent, IKNISystem
    {
        public System System { get; }
        public ISystemRenderer Renderer { get; }

        public KNIRenderer(ISystemRenderer systemRenderer, Game game) : base(game)
        {
            System = systemRenderer as System;
            Renderer = systemRenderer;
        }

        public override void Draw(GameTime gameTime)
        {
            Renderer.Render();

            base.Draw(gameTime);
        }
    }
}
