namespace ZZZ.Framework.Assets
{
    public sealed class AssetManager
    {
        private static AssetManager instance;
        private static IAssetProvider provider;

        internal AssetManager(IAssetProvider assetProvider)
        {
            instance ??= this;

            provider = assetProvider;
        }

        public static T Load<T>(string path)
        {
            if (instance == null)
                return default;

            return provider.Load<T>(path);
        }

        public static T LoadPrefab<T>(string path) where T : GameObject
        {
            if (instance == null)
                return default;

            return provider.Load<T>(path);
        }
    }
}
