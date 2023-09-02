using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using ZZZ.Framework.Monogame.Content;
using ZZZ.Framework.Monogame.Content.TypeSerializers;

namespace ZZZ.Framework.Monogame.Rendering.Content
{
    [ContentTypeSerializer]
    public class SpriteTypeSerializer : ContentTypeSerializer<Sprite>
    {
        protected override Sprite Deserialize(IntermediateReader input, ContentSerializerAttribute format, Sprite existingInstance)
        {
            string assetName = input.Xml.GetAttribute("AssetName");

            int index = assetName.LastIndexOf('_');

            if (index == -1)
                return AssetManager.Instance.Load<Sprite>(assetName);

            string strId = assetName.Substring(index + 1);
            string original = assetName.Substring(0, index);

            int id;

            if (int.TryParse(strId, out id))
                return AssetManager.Instance.Load<Sprite>(original)[id];
            else throw new Exception("Имя ассета имеет символ нижнего подчеркивания, но дальнейшие символы не являются числом.");
        }

        protected override void Serialize(IntermediateWriter output, Sprite value, ContentSerializerAttribute format)
        {
            output.Xml.WriteAttributeString("AssetName", value.Name);
        }
    }
}
