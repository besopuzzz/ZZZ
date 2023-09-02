using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ZZZ.Framework.Monogame.Content
{
    public interface IAsset : IDisposable
    {
        string Name { get; }
    }
}
