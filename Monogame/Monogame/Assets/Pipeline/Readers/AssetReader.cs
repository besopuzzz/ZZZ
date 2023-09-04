using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;
using ZZZ.Framework.Monogame.Assets;

namespace ZZZ.Framework.Monogame.Assets.Pipeline.Readers
{
    internal class AssetReader : ContentTypeReader
    {
        public AssetReader() : base(typeof(object))
        {
        }

        protected override object Read(ContentReader input, object existingInstance)
        {
            string xml = input.ReadObject<string>();
            object content = null;

            XmlReaderSettings settings = new XmlReaderSettings();

            using (TextReader stream = new StringReader(xml))
            using (XmlReader writer = XmlReader.Create(stream, settings))
            {
                content = IntermediateSerializer.Deserialize<object>(writer, null);
            }

            if (content is Asset asset)
                asset.Name = input.AssetName;

            return content;
        }
    }
}
