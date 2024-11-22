//using Microsoft.Xna.Framework.Content.Pipeline;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
//using ZZZ.Framework.Assets.Pipeline.Readers;

//namespace ZZZ.KNI.Content.Pipeline.Writers
//{
//    [ContentTypeWriter]
//    public class GameObjectWriter : ContentTypeWriter<GameObject>
//    {
//        public override string GetRuntimeReader(TargetPlatform targetPlatform)
//        {
//            return typeof(GameObjectReader).AssemblyQualifiedName;
//        }

//        protected override void Write(ContentWriter output, GameObject value)
//        {
//            output.WriteObject(value.Name);
//            output.WriteObject(value.Enabled);
//            output.WriteObject(value.GetComponents());
//            output.WriteObject(value.GetGameObjects());
//        }
//    }
//}
