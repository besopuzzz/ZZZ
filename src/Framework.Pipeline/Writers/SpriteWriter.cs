//using Microsoft.Xna.Framework.Content.Pipeline;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
//using ZZZ.Framework.Rendering.Assets;
//using ZZZ.Framework.Rendering.Assets.Pipeline;

//namespace ZZZ.KNI.Content.Pipeline.Writers
//{
//    //[ContentTypeWriter]
//    public class SpriteWriter : ContentTypeWriter<Sprite>
//    {
//        public override string GetRuntimeType(TargetPlatform targetPlatform)
//        {
//            return typeof(Sprite).AssemblyQualifiedName;
//        }
//        public override string GetRuntimeReader(TargetPlatform targetPlatform)
//        {
//            return typeof(SpriteXmlReader).AssemblyQualifiedName;
//        }

//        protected override void Write(ContentWriter output, Sprite value)
//        {
//            output.WriteObject(value.Name);
//        }
//    }
//}
