using Microsoft.Xna.Framework.Content;

namespace ZZZ.Framework.Monogame.Assets
{
    public class Asset : Disposable, IAsset
    {
        [ContentSerializer]
        public string Name { get; set; } = "";
    }
}
