using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using ZZZ.Framework;
using ZZZ.Framework.Assets;

namespace ZZZ.KNI.GameProject.Services
{
    internal sealed class AssetProvider : IAssetProvider
    {
        private ContentManager content;

        public AssetProvider(Game game) 
        {
            content = game.Content;
        }

        public T Load<T>(string path)
        {
            return content.Load<T>(path);
        }

        public T LoadPrefab<T>(string path) where T : GameObject
        {
            throw new NotImplementedException();
        }
    }
}
