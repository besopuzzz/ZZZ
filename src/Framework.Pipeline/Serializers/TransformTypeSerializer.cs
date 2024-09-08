using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace ZZZ.Framework.Pipeline.Serializers
{
    [ContentTypeSerializer]
    public class TransformTypeSerializer : ContentTypeSerializer<Transform2D>
    {
        protected override Transform2D Deserialize(IntermediateReader input, ContentSerializerAttribute format, Transform2D existingInstance)
        {
            var str = input.Xml.ReadString();

            string[] elements = str.Split(" ");

            existingInstance.Position.X = XmlConvert.ToSingle(elements[0]);
            existingInstance.Position.Y = XmlConvert.ToSingle(elements[1]);
            existingInstance.Scale.X = XmlConvert.ToSingle(elements[2]);
            existingInstance.Scale.Y = XmlConvert.ToSingle(elements[3]);
            existingInstance.Rotation = XmlConvert.ToSingle(elements[4]);

            return existingInstance;
        }

        protected override void Serialize(IntermediateWriter output, Transform2D value, ContentSerializerAttribute format)
        {
            string[] elements = new string[5];

            elements[0] = XmlConvert.ToString(value.Position.X);
            elements[1] = XmlConvert.ToString(value.Position.Y);
            elements[2] = XmlConvert.ToString(value.Scale.X);
            elements[3] = XmlConvert.ToString(value.Scale.Y);
            elements[4] = XmlConvert.ToString(value.Rotation);

            var str = string.Join(" ", elements);

            output.Xml.WriteString(str);
        }
    }
}
