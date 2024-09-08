using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    public class AssetTypeSerializer<T> : ContentTypeSerializer<T>
        where T : IAsset
    {
        private ReflectiveTypeSerializer anyTypeSerializer;

        protected override void Initialize(IntermediateSerializer serializer)
        {
            anyTypeSerializer = new ReflectiveTypeSerializer();
            anyTypeSerializer.Initialize(serializer, TargetType);

            base.Initialize(serializer);
        }

        protected override T Deserialize(IntermediateReader input, ContentSerializerAttribute format, T existingInstance)
        {
            var name = input.Xml.GetAttribute("Asset");

            if (existingInstance == null)
                existingInstance = (T)Activator.CreateInstance(typeof(T), true)!;

            existingInstance.Name = name;

            if (string.IsNullOrEmpty(name))
            {
                anyTypeSerializer.Deserialize(input, format, existingInstance, TargetType);

                return existingInstance;
            }

            var loaded = AssetManager.Load<T>(name);

            if (loaded != null)
                return loaded;


            return existingInstance;
        }

        protected override void Serialize(IntermediateWriter output, T value, ContentSerializerAttribute format)
        {
            if (string.IsNullOrWhiteSpace(value.Name))
            {
                anyTypeSerializer.Serialize(output, value, format, TargetType);
            }
            else
            {
                ExternalReference<T> externalReference = new ExternalReference<T>(value.Name);

                var method = output.GetType().GetField("_filePath",  System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                method.SetValue(output, "Content/");

                output.WriteExternalReference(externalReference);
                //output.Xml.WriteAttributeString("Asset", value.Name);
            }
        }
    }
}
