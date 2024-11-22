namespace ZZZ.Framework.Assets
{
    public sealed class AssetManager : Disposable
    {
        internal static AssetManager Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException("AssetManager not initialized!");

                return instance;
            }
        }

        private static AssetManager instance;
        private static IAssetProvider provider;

        internal AssetManager(IAssetProvider assetProvider)
        {
            if (instance != null)
                throw new InvalidOperationException("AssetManager already initialize!");

            instance = this;

            provider = assetProvider;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                ((IDisposable)provider)?.Dispose();
            }

            provider = null;
            instance = null;

            base.Dispose(disposing);
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
