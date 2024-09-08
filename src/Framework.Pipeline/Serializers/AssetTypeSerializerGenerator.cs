using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Reflection;
using ZZZ.Framework.Extentions;
using ZZZ.KNI.Content.Pipeline.Writers;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            VirtualAssemblyGenerator virtualAssembly = new VirtualAssemblyGenerator("AssetExtentionAssembly");


            foreach (var item in AssetTypes.Assets)
            {
                virtualAssembly.AddType(item.Name + "TypeSerializer", TypeAttributes.Public | TypeAttributes.Class, typeof(AssetTypeSerializer<>)
                .MakeGenericType(item))
                    .AddCustomAttribute(typeof(ContentTypeSerializerAttribute))
                    .Create();

                virtualAssembly.AddType(item.Name + "TypeWriter", TypeAttributes.Public | TypeAttributes.Class, typeof(AssetTypeWriter<>)
                .MakeGenericType(item))
                    .AddCustomAttribute(typeof(ContentTypeWriterAttribute))
                    .Create();

            }

        }
    }
}
