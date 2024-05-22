using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Auding.Assets;
using ZZZ.Framework.Auding.Assets.Pipeline;

namespace ZZZ.KNI.Content.Pipeline.Writers
{

    [ContentTypeWriter]
    public class SoundContentWriter : ContentTypeWriter<SoundContent>
    {
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Sound).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SoundReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, SoundContent value)
        {
            output.WriteObject(value.SoundEffectContent);
        }
    }
}
