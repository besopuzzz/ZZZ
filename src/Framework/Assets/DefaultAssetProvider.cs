namespace ZZZ.Framework.Assets
{
    internal class DefaultAssetProvider : ContentManager, IAssetProvider
    {
        public DefaultAssetProvider(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
        {
        }

        public T LoadPrefab<T>(string path) where T : GameObject
        {
            return default;
        }
    }
}
