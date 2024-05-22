using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Assets.Pipeline.Readers;

namespace ZZZ.KNI.Content.Pipeline.Writers
{
    [ContentTypeWriter]
    public class SceneWriter : ContentTypeWriter<Scene>
    {
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Scene).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SceneReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, Scene value)
        {
            output.WriteObject(value.Name);
            output.WriteObject(value.Enabled);
            output.WriteObject(value.GetComponents());
            output.WriteObject(value.GetGameObjects());
        }
    }
}
