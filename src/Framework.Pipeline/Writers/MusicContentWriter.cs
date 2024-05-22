using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Auding.Assets;
using ZZZ.Framework.Auding.Assets.Pipeline;

namespace ZZZ.KNI.Content.Pipeline.Writers
{

    [ContentTypeWriter]
    public class MusicContentWriter : ContentTypeWriter<MusicContent>
    {
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Music).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(MusicReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, MusicContent value)
        {
            output.WriteObject(value.SongContent);
        }
    }
}
