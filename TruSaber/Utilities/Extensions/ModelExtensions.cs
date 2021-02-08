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

    }
}