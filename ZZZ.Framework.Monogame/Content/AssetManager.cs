using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace ZZZ.Framework.Monogame.Content
{
    public class AssetManager : ContentManager
    {
        public static AssetManager Instance => instance;

        private static AssetManager instance = null!;

        internal AssetManager(IServiceProvider serviceProvider) : base(serviceProvider, "Content")
        {
            if (instance != null)
                return;

            instance = this;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                instance = null;
            }

            base.Dispose(disposing);
        }

        public void SerializeToFile(object obj, string path)
        {
            File.WriteAllText(path, Serialize(obj));
        }
        public string Serialize(object obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            string result = "";

            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    Serialize(sw, obj, settings);

                    result = sw.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public T DeserializeFromFile<T>(string path)
        {
            return Deserialize<T>(File.ReadAllText(path));
        }
        public T Deserialize<T>(string xml)
        {
            XmlReaderSettings settings = new XmlReaderSettings();

            T result = default(T);

            try
            {
                using (var sw = new StringReader(xml))
                {
                    result = Deserialize<T>(sw, settings);
                }
            }
            catch (Exception)
            {

                throw;
            }

            
            return result;
        }
        public T CreateCopy<T>(T instance)
        {
            T copy = default(T)!;

            XmlWriterSettings wsettings = new XmlWriterSettings();
            wsettings.Indent = true;

            XmlReaderSettings rsettings = new XmlReaderSettings();

            using (MemoryStream ms = new MemoryStream())
            {
                Serialize(ms, instance!, wsettings);

                ms.Position = 0;

                copy = Deserialize<T>(ms, rsettings);
            }
            return copy;
        }
        public T LoadPrefab<T>(string name)
        {
            T prefab = Load<T>(name);

            return CreateCopy<T>(prefab);
        }

        private void Serialize(MemoryStream memoryStream, object value, XmlWriterSettings settings)
        {
            using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
            {
                IntermediateSerializer.Serialize(writer, value, null);
            }
        }
        private void Serialize(TextWriter  textWriter, object value, XmlWriterSettings settings)
        {
            using (XmlWriter writer = XmlWriter.Create(textWriter, settings))
            {
                IntermediateSerializer.Serialize(writer, value, null);
            }
        }

        private T Deserialize<T>(MemoryStream memoryStream, XmlReaderSettings settings)
        {
            using (XmlReader reader = XmlReader.Create(memoryStream, settings))
            {
                return IntermediateSerializer.Deserialize<T>(reader, null);
            }
        }
        private T Deserialize<T>(TextReader  textReader, XmlReaderSettings settings)
        {
            using (XmlReader reader = XmlReader.Create(textReader, settings))
            {
               return IntermediateSerializer.Deserialize<T>(reader, null);
            }
        }
    }
}
