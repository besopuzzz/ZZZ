using Microsoft.Xna.Framework.Content.Pipeline;

namespace ZZZ.Framework.Monogame.Asseting.Pipeline.Processors
{
    [ContentProcessor(DisplayName = "Asset Processor - ZZZ")]
    internal class AssetProcessor : ContentProcessor<string, AssetContent>
    {
        public override AssetContent Process(string input, ContentProcessorContext context)
        {
            return new AssetContent() { Xml = input };
        }
    }
}
