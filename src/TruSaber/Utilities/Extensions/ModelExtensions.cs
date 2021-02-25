using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruSaber.Utilities.Extensions
{
    public static class ModelExtensions
    {

        public static void DrawModel(this Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                
                mesh.Draw();
            }
        }

        private static Matrix[] sharedDrawBoneMatrices;
        public static void DrawModelWithExclusions(this Model model, Matrix world, Matrix view, Matrix projection, params string[] meshNamesToExclude)
        {
            var count = model.Bones.Count;
            
            if (sharedDrawBoneMatrices == null || sharedDrawBoneMatrices.Length < count)
                sharedDrawBoneMatrices = new Matrix[count];
            
            model.CopyAbsoluteBoneTransformsTo(sharedDrawBoneMatrices);
            
            foreach (var mesh in model.Meshes)
            {
                if(meshNamesToExclude.Contains(mesh.Name))
                    continue;
                
                foreach (var effect in mesh.Effects)
                {
                    if (!(effect is IEffectMatrices effectMatrices))
                        throw new InvalidOperationException();
                    
                    effectMatrices.World = sharedDrawBoneMatrices[mesh.ParentBone.Index] * world;
                    effectMatrices.View = view;
                    effectMatrices.Projection = projection;
                }
                
                mesh.Draw();
            }
        }
        //
        // public static void DrawModelMesh(this ModelMesh modelMesh, GraphicsDevice graphicsDevice, Matrix world, Matrix view, Matrix projection, params string[] boneExclusions)
        // {
        //     for (int i = 0; i < modelMesh.MeshParts.Count; ++i)
        //     {
        //         var meshPart = modelMesh.MeshParts[i];
        //         var        effect   = meshPart.Effect;
        //         if (meshPart.PrimitiveCount > 0)
        //         {
        //             graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
        //             graphicsDevice.Indices = meshPart.IndexBuffer;
        //             
        //             for (int j = 0; j < effect.CurrentTechnique.Passes.Count; ++j)
        //             {
        //                 effect.CurrentTechnique.Passes[j].Apply();
        //                 graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);
        //             }
        //         }
        //     }   
        // }

    }
}