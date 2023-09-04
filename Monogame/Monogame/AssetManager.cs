using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System;
using System.Reflection;
using System.Xml;

namespace ZZZ.Framework.Monogame
{
    public static class AssetManager
    {
        private static ContentManager contentManager;

        internal static void Initialize(IServiceProvider serviceProvider)
        {
            contentManager = new ContentManager(serviceProvider, "Content");
        }

        public static void SerializeToXmlFile(string path, object obj)
        {
            File.WriteAllText(path, Serialize(obj));
        }
        public static string Serialize(object obj)
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

        public static T DeserializeFromFile<T>(string path)
        {
            return Deserialize<T>(File.ReadAllText(path));
        }
        public static T Deserialize<T>(string xml)
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

        public static async Task<T> CreateCopyAsync<T>(T obj)
        {
            T value = default(T);

            value = await Task.Run(() => {
                return CreateCopy<T>(obj);
            });

            return value;
        }

        public static T CreateCopy<T>(T obj)
        {
            T copy = default(T)!;

            XmlWriterSettings wsettings = new XmlWriterSettings();
            wsettings.Indent = true;

            XmlReaderSettings rsettings = new XmlReaderSettings();

            using (MemoryStream ms = new MemoryStream())
            {
                Serialize(ms, obj, wsettings);

                ms.Position = 0;

                copy = Deserialize<T>(ms, rsettings);
            }
            return copy;
        }
        public static T CreateCopy2<T>(T obj)
        {
            var xml = Serialize(obj);
            return Deserialize<T>(xml);
        }
        public static T LoadPrefab<T>(string name)
        {
            T prefab = Load<T>(name);

            return CreateCopy<T>(prefab);
        }

        public static T Load<T>(string name)
        {
            if (contentManager == null)
                throw new Exception();

            return contentManager.Load<T>(name);
        }

        private static void Serialize(MemoryStream memoryStream, object value, XmlWriterSettings settings)
        {
            using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
            {
                IntermediateSerializer.Serialize(writer, value, null);
            }
        }
        private static void Serialize(TextWriter  textWriter, object value, XmlWriterSettings settings)
        {
            using (XmlWriter writer = XmlWriter.Create(textWriter, settings))
            {
                IntermediateSerializer.Serialize(writer, value, null);
            }
        }

        private static T Deserialize<T>(MemoryStream memoryStream, XmlReaderSettings settings)
        {
            using (XmlReader reader = XmlReader.Create(memoryStream, settings))
            {
                return IntermediateSerializer.Deserialize<T>(reader, null);
            }
        }
        private static T Deserialize<T>(TextReader  textReader, XmlReaderSettings settings)
        {
            using (XmlReader reader = XmlReader.Create(textReader, settings))
            {
               return IntermediateSerializer.Deserialize<T>(reader, null);
            }
        }
    }
}
