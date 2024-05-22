using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    public class AssetTypeSerializer<T> : ContentTypeSerializer<T>
        where T : Asset
    {
        private AssetIntermediateSerializer anyTypeSerializer;

        protected override void Initialize(IntermediateSerializer serializer)
        {
            anyTypeSerializer = new AssetIntermediateSerializer();

            base.Initialize(serializer);
        }

        protected override T Deserialize(IntermediateReader input, ContentSerializerAttribute format, T existingInstance)
        {
            var name = input.Xml.GetAttribute("Asset");

            if (existingInstance == null)
                existingInstance = (T)Activator.CreateInstance(typeof(T), true);

            if (string.IsNullOrEmpty(name))
            {
                return (T)anyTypeSerializer.Deserialize(input, format, existingInstance, TargetType);
            }

            existingInstance.Name = name;

            return existingInstance;
        }

        protected override void Serialize(IntermediateWriter output, T value, ContentSerializerAttribute format)
        {
            if(string.IsNullOrWhiteSpace(value.Name))
            {
                anyTypeSerializer.Serialize(output, value, format, TargetType);
            }
            else output.Xml.WriteAttributeString("Asset", value.Name);
        }
    }
}
