﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using AnimationAux;

namespace AnimationPipeline
{
    [ContentTypeWriter]
    public class ModelExtraWriter : ContentTypeWriter<ModelExtra>
    {
        protected override void Write(ContentWriter output, ModelExtra extra)
        {
            output.WriteObject(extra.Skeleton);
            output.WriteObject(extra.Clips);
        }
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(ModelExtra).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "AnimationAux.ModelExtraReader, XnaAux";
        }
    }
}
