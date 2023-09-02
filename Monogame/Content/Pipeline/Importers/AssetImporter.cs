using Microsoft.Xna.Framework.Content.Pipeline;

namespace ZZZ.Framework.Monogame.Content.Pipeline.Importers
{
    [ContentImporter(".xml", DisplayName = "Asset Importer - ZZZ", DefaultProcessor = "AssetProcessor")]
    internal class AssetImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
        }
    }
}
