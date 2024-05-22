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
            //AssetTypesGenerator.Generate();



            VirtualAssemblyGenerator virtualAssembly = new VirtualAssemblyGenerator("AssetExtentionAssembly");


            foreach (var item in AssetTypes.Assets)
            {
                virtualAssembly.AddType(item.Name + "TypeSerializer", TypeAttributes.NotPublic | TypeAttributes.Class, typeof(AssetTypeSerializer<>)
                .MakeGenericType(item))
                    .AddCustomAttribute(typeof(ContentTypeSerializerAttribute))
                    .Create();

                virtualAssembly.AddType(item.Name + "TypeWriter", TypeAttributes.NotPublic | TypeAttributes.Class, typeof(AssetTypeWriter<>)
                .MakeGenericType(item))
                    .AddCustomAttribute(typeof(ContentTypeWriterAttribute))
                    .Create();

            }

        }
    }

    //public static class AssetTypesGenerator
    //{
    //    private static bool generated = false;

    //    public static void Generate()
    //    {
    //        if (generated)
    //            return;

    //        AssemblyName assemblyName = new AssemblyName("AssetExtentionAssembly");
    //        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
    //        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("AssetExtentionAssembly");

    //        foreach (var item in AssetTypes.Assets)
    //        {
    //            CreateType(item.Name + "TypeSerializer",typeof(AssetTypeSerializer<>), typeof(ContentTypeSerializerAttribute),item, moduleBuilder);

    //            CreateType(item.Name + "TypeWriter", typeof(AssetTypeWriter<>), typeof(ContentTypeWriterAttribute), item, moduleBuilder);

    //            CreateType(item.Name + "TypeReader", typeof(AssetReader<>), null, item, moduleBuilder);
    //        }

    //        generated = true;
    //    }

    //    private static void CreateType(string name, Type baseType, Type? attribute, Type asset, ModuleBuilder moduleBuilder)
    //    {
    //        var parent = baseType.MakeGenericType(asset);

    //        var tb = moduleBuilder.DefineType(name, TypeAttributes.NotPublic | TypeAttributes.Class, parent);

            

    //        if (attribute != null)
    //            tb.SetCustomAttribute(new CustomAttributeBuilder(attribute.GetConstructor(new Type[0]), new object[0]));

    //        var x = tb.CreateType();
    //    }


    //}
}
