using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.IO;
using System.Xml;

namespace ZZZ.KNI.Content.Pipeline
{
    public partial class AssetSerializer
    {

        static AssetSerializer()
        {
            //AssetTypesGenerator.Generate();



        }

        private class StringWriterWithEncoding : StringWriter
        {
            public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
                : base(sb)
            {
                this.m_Encoding = encoding;
            }
            private readonly Encoding m_Encoding;
            public override Encoding Encoding
            {
                get
                {
                    return this.m_Encoding;
                }
            }
        }

        public static string Serialize<T>(T value)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            StringBuilder builder = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(builder))
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    IntermediateSerializer.Serialize<T>(writer, value, null);
                }
            }

            return builder.ToString();
        }

        public static object Deserialize(string value)
        {
            XmlReaderSettings settings = new XmlReaderSettings();

            object result;

            using (TextReader stream = new StringReader(value))
            {
                using (XmlReader writer = XmlReader.Create(stream, settings))
                {
                    result = IntermediateSerializer.Deserialize<object>(writer, null);
                }
            }

            return result;
        }

    }
}
