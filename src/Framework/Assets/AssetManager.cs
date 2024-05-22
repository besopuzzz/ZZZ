namespace ZZZ.Framework.Assets
{
    public sealed class AssetManager
    {
        private static AssetManager instance;
        public IAssetProvider provider;

        internal AssetManager(IAssetProvider assetProvider)
        {
            instance ??= this;

            instance.provider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
        }

        private static void CheckOrThrow()
        {
            if (instance == null)
                throw new InvalidOperationException("AssetManager not initialized. Use GameManagerBuilder.UseStaticAssetManager for initialize.");
        }

        public static T Load<T>(string path)
        {
            CheckOrThrow();

            return instance.provider.Load<T>(path);
        }

        public static T LoadPrefab<T>(string path) where T : GameObject
        {
            CheckOrThrow();

            return instance.provider.LoadPrefab<T>(path);
        }
    }
}
