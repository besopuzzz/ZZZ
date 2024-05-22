using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Rendering.Assets.Pipeline;

namespace ZZZ.KNI.Content.Pipeline.Writers
{
    [ContentTypeWriter]
    public class SpriteContentWriter : ContentTypeWriter<SpriteContent>
    {
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Sprite).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, SpriteContent value)
        {
            output.WriteObject(value.Texture);
            output.Write(value.Frames.Count);

            for (int i = 0; i < value.Frames.Count; i++)
            {
                output.WriteObject(value.Frames[i].Bounds);
                output.WriteObject(value.Frames[i].Origin);
            }
        }
    }
}
