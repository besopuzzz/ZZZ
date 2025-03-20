using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Assets;

namespace ZZZ.Framework.KNI
{
    public class KNIAssetProvider : ContentManager, IAssetProvider
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
