//namespace ZZZ.Framework.Rendering.Assets.Pipeline
//{
//    public class SpriteXmlReader : ContentTypeReader<Sprite>
//    {
//        protected override Sprite Read(ContentReader input, Sprite existingInstance)
//        {
//            var name = input.ReadObject<string>();

//            string assetName = name;

//            int index = assetName.LastIndexOf('_');

//            if (index == -1)
//                return input.ContentManager.Load<Sprite>(assetName);

//            string strId = assetName.Substring(index + 1);
//            string original = assetName.Substring(0, index);

//            int id;

//            if (int.TryParse(strId, out id))
//                return input.ContentManager.Load<Sprite>(original)[id];
//            else throw new Exception("Имя ассета имеет символ нижнего подчеркивания, но дальнейшие символы не являются числом.");
//        }
//    }
//}
