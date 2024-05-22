using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    [ContentTypeSerializer]
    public class SharedListTypeSerializer<T> : ContentTypeSerializer<SharedList<T>>
    {
        ContentSerializerAttribute itemFormat = new ContentSerializerAttribute();


        public SharedListTypeSerializer()
        {
            itemFormat.ElementName = "Item";
        }


        protected override void Serialize(IntermediateWriter output, SharedList<T> value, ContentSerializerAttribute format)
        {
            foreach (T t in value)
            {
                output.WriteSharedResource(t, itemFormat);
            }
        }


        protected override SharedList<T> Deserialize(IntermediateReader input, ContentSerializerAttribute format, SharedList<T> existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new SharedList<T>();

            while (input.MoveToElement(itemFormat.ElementName))
            {
                input.ReadSharedResource<T>(itemFormat, existingInstance.Add);
            }

            return existingInstance;
        }
    }
}
