using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.IO;
using System.Xml;

namespace ZZZ.KNI.Content.Pipeline
{
    public partial class AssetSerializer
    {
        public static string Serialize<T>(T value)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            string result = "";

            using (TextWriter stringWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    IntermediateSerializer.Serialize<T>(writer, value, null);
                }

                result = stringWriter.ToString();
            }

            return result;
        }

        public static object Deserialize(string value)
        {
            object result;

            using (TextReader stream = new StringReader(value))
            {
                using (XmlReader writer = XmlReader.Create(stream))
                {
                    result = IntermediateSerializer.Deserialize<object>(writer, null);
                }
            }

            return result;
        }

    }
}
