using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Extensions
{
    internal sealed class KNIAssetProvider : ContentManager, IAssetProvider
    {
        public KNIAssetProvider(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
        {
        }

        public T LoadPrefab<T>(string path) where T : GameObject
        {
            throw new NotImplementedException();
        }
    }
}
