using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruSaber.Graphics
{
    public static class BoundingBoxRenderer
    {
        private static BasicEffect _effect;
        public static void Draw(this BoundingBox bb, GraphicsDevice graphicsDevice, Color color)
        {
            if (_effect == null)
            {
                _effect = new BasicEffect(graphicsDevice);
                _effect.LightingEnabled = false;
                _effect.TextureEnabled = false;
                _effect.VertexColorEnabled = true;
            }
            
            VertexPositionColor[] verticies = new VertexPositionColor[8];
            var                   indecies  = new short[12 * 2];

            /*
             * new Vector3[8]
             * {
             *   new Vector3(this.Min.X, this.Max.Y, this.Max.Z),   0, 1, 1
             *   new Vector3(this.Max.X, this.Max.Y, this.Max.Z),   1, 1, 1
             *   new Vector3(this.Max.X, this.Min.Y, this.Max.Z),   1, 0, 1
             *   new Vector3(this.Min.X, this.Min.Y, this.Max.Z),   0, 0, 1
             *   new Vector3(this.Min.X, this.Max.Y, this.Min.Z),   0, 1, 0
             *   new Vector3(this.Max.X, this.Max.Y, this.Min.Z),   1, 1, 0
             *   new Vector3(this.Max.X, this.Min.Y, this.Min.Z),   1, 0, 0
             *   new Vector3(this.Min.X, this.Min.Y, this.Min.Z)    0, 0, 0
             * };
             */
            var corners = bb.GetCorners();
            // var corners = new Vector3[]
            // {
            //     new Vector3(0, 1, 1),
            //     new Vector3(1, 1, 1),
            //     new Vector3(1, 0, 1),
            //     new Vector3(0, 0, 1),
            //     new Vector3(0, 1, 0),
            //     new Vector3(1, 1, 0),
            //     new Vector3(1, 0, 0),
            //     new Vector3(0, 0, 0),
            // };
            for (int i = 0; i < corners.Length; i++)
            {
                verticies[i] = new VertexPositionColor(corners[i], Color.White);
            }

            int j = 0;
            indecies[j++] = 0; indecies[j++] = 1;
            indecies[j++] = 1; indecies[j++] = 2;
            indecies[j++] = 2; indecies[j++] = 3;
            indecies[j++] = 3; indecies[j++] = 0;
            
            indecies[j++] = 4; indecies[j++] = 5;
            indecies[j++] = 5; indecies[j++] = 6;
            indecies[j++] = 6; indecies[j++] = 7;
            indecies[j++] = 7; indecies[j++] = 4;
            
            indecies[j++] = 0; indecies[j++] = 4;
            indecies[j++] = 1; indecies[j++] = 5;
            indecies[j++] = 2; indecies[j++] = 6;
            indecies[j++] = 3; indecies[j++] = 7;

            _effect.World = Matrix.Identity;
            var cam = (TruSaberGame.Instance.Camera);
            if(cam == null) return;
            
            _effect.View = cam.View;
            _effect.Projection = cam.Projection;
            
                foreach (var pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    
                    graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verticies, 0, verticies.Length, indecies,
                        0, 12, VertexPositionColor.VertexDeclaration);
                }
            

        }
    }
}