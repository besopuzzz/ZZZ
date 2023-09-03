using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ZZZ.Framework.Monogame.Asseting.Assets
{
    public interface IAsset : IDisposable
    {
        string Name { get; }
    }
}
