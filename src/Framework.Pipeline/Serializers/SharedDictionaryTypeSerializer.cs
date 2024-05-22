using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Collections.Generic;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    [ContentTypeSerializer]
    public class SharedDictionaryTypeSerializer<T, K> : ContentTypeSerializer<SharedDicitionary<T, K>>
    {
        static ContentSerializerAttribute itemFormat =
            new ContentSerializerAttribute()
            {
                ElementName = "Item"
            };

        static ContentSerializerAttribute keyFormat =
            new ContentSerializerAttribute()
            {
                ElementName = "Key"
            };

        static ContentSerializerAttribute valueFormat =
            new ContentSerializerAttribute()
            {
                ElementName = "Value"
            };



        protected override void Serialize(IntermediateWriter output, SharedDicitionary<T, K> value, ContentSerializerAttribute format)
        {
            foreach (KeyValuePair<T, K> item in value)
            {
                output.Xml.WriteStartElement(
                    itemFormat.ElementName);

                output.WriteObject(
                    item.Key,
                    keyFormat);

                output.WriteSharedResource(
                    item.Value,
                    valueFormat);

                output.Xml.WriteEndElement();
            }
        }


        protected override SharedDicitionary<T, K> Deserialize(IntermediateReader input, ContentSerializerAttribute format, SharedDicitionary<T, K> existingInstance)
        {
            if (existingInstance == null)
            {
                existingInstance =
                    new SharedDicitionary<T, K>();
            }

            while (input.MoveToElement(itemFormat.ElementName))
            {
                T key;

                input.Xml.ReadToDescendant(
                    keyFormat.ElementName);

                key = input.ReadObject<T>(keyFormat);

                input.Xml.ReadToNextSibling(
                    valueFormat.ElementName);

                input.ReadSharedResource(
                    valueFormat,
                    (K value) =>
                    {
                        existingInstance.Add(key, value);
                    });
                input.Xml.ReadEndElement();
            }

            return existingInstance;
        }
    }
}
