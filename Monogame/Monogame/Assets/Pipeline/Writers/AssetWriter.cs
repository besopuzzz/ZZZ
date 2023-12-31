﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Monogame.Assets.Pipeline;
using ZZZ.Framework.Monogame.Assets.Pipeline.Readers;

namespace ZZZ.Framework.Monogame.Assets.Pipeline.Writers
{
    [ContentTypeWriter]
    internal class AssetWriter : ContentTypeWriter<AssetContent>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AssetReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, AssetContent value)
        {
            output.WriteObject(value.Xml);
        }
    }
}
