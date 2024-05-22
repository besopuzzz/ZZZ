using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Assets.Pipeline.Readers;

namespace ZZZ.KNI.Content.Pipeline.Writers
{
    public abstract class AssetTypeWriter<TAsset> : ContentTypeWriter<TAsset>
        where TAsset : Asset
    {
        private AssetCompilerWriter compilerWriter = new AssetCompilerWriter();

        private bool isReflectiveReader = false;

        public override bool CanDeserializeIntoExistingObject
        {
            get { return TargetType.IsClass; }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TAsset).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AssetReader<TAsset>).AssemblyQualifiedName;
        }

        protected override void Initialize(ContentCompiler compiler)
        {
            compilerWriter.Initialize(compiler, TargetType);

            base.Initialize(compiler);
        }

        protected override void Write(ContentWriter output, TAsset value)
        {


            if(string.IsNullOrWhiteSpace(value.Name))
            {
                isReflectiveReader = true;
                output.WriteObject("");

                compilerWriter.OnAddedToContentWriter(output);
                compilerWriter.Write(output, value);
            }
            else output.WriteObject(value.Name);
        }

        

        //protected abstract void WriteObject(ContentWriter output, TAsset value);
    }
}
