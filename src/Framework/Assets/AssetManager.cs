using ZZZ.Framework.Core;

namespace ZZZ.Framework.Assets
{
    public sealed class AssetManager
    {
        private static AssetManager instance;
        private static ContentManager contentManager;

        internal AssetManager(GameManager gameManager)
        {
            instance ??= this;

            contentManager = new ContentManager(gameManager.Game.Services, "Content");
        }

        public static T Load<T>(string path)
        {
            if (instance == null)
                return default;

            return contentManager.Load<T>(path);
        }

        public static T LoadPrefab<T>(string path) where T : GameObject
        {
            if (instance == null)
                return default;

            return contentManager.Load<T>(path);
        }
    }
}
