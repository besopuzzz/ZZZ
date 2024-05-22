using ZZZ.Framework.Assets;
using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public sealed class GameManagerBuilder
    {
        private readonly GameManager gameManager;

        internal GameManagerBuilder(GameManager manager)
        {
            ArgumentNullException.ThrowIfNull(manager);

            gameManager = manager;

            _ = new SceneLoader(gameManager);
        }

        public GameManagerBuilder UseRegistrar(IRegistrar registrar)
        {
            gameManager.UseRegistrar(registrar);

            return this;
        }

        public GameManagerBuilder UseStaticAssetManager(IAssetProvider assetProvider)
        {
            _ = new AssetManager(assetProvider);

            return this;
        }
        public GameManagerBuilder UseStaticAssetManager(string rootDirectory = "Content")
        {
            _ = new AssetManager(new DefaultAssetProvider(gameManager.Game.Services, rootDirectory));

            return this;
        }

        public GameManager EndBuild()
        {
            gameManager.InternalInitialize();

            return gameManager;
        }
    }
}
