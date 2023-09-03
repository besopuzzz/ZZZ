using Microsoft.Xna.Framework;

namespace ZZZ.Framework.Monogame.Asseting.Components
{
    internal interface Empty : IComponent
    {

    }
    internal sealed class AssetRegistrar : MonogameRegistrar<Empty>
    {
        private AssetManager assetManager;

        protected override void Startup()
        {
            assetManager = new AssetManager(Game.Services);

            base.Startup();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                assetManager.Dispose();
            }

            assetManager = null;

            base.Dispose(disposing);
        }
    }
}
