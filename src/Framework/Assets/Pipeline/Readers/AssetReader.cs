using System.Reflection;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Assets.Pipeline.Readers
{
    public class AssetReader : ContentTypeReader<IAsset>
    {
        private Reader serializer = new Reader();

        protected override void Initialize(ContentTypeReaderManager manager)
        {
            serializer.Initialize(manager, TargetType);

            base.Initialize(manager);
        }


        public override bool CanDeserializeIntoExistingObject
        {
            get { return TargetType.IsClass; }
        }

        protected override IAsset Read(ContentReader input, IAsset existingInstance)
        {
            if(existingInstance == null)
                existingInstance = (IAsset)Activator.CreateInstance(TargetType, true);

            string name = input.ReadObject<string>();

            if(string.IsNullOrEmpty(name))
            {
                serializer.Read(input, existingInstance);

                return existingInstance;
            }

            if (TargetType == typeof(Sprite))
            {
                var sprite = Read(name, input.ContentManager);

                return sprite as IAsset;
            }
            else return input.ContentManager.Load<IAsset>(name);
        }

        private Sprite Read(string name, ContentManager contentManager)
        {
            string assetName = name;

            int index = assetName.LastIndexOf('_');

            if (index == -1)
                return contentManager.Load<Sprite>(assetName);

            string strId = assetName.Substring(index + 1);
            string original = assetName.Substring(0, index);

            int id;

            if (int.TryParse(strId, out id))
                return contentManager.Load<Sprite>(original)[id];
            else throw new Exception("Имя ассета имеет символ нижнего подчеркивания, но дальнейшие символы не являются числом.");
        }
    }
}
