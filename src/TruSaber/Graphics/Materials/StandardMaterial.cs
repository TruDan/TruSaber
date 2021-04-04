using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Graphics.Shaders.Deferred;
using TruSaber.Graphics.Shaders.Forward;

namespace TruSaber.Graphics.Materials
{
    public class StandardMaterial : Material
    {
        public Texture2D NormalMap { get; set; }

        // Cutout
        public bool CutoutEnabled { get; set; }
        public float Cutout { get; set; }
        // Specular
        public Texture2D SpecularMap { get; set; }
        public Color SpecularColor { get; set; } = Color.Black;
        public int SpecularPower { get; set; } = 16;
        public float SpecularIntensity { get; set; } = 1.0f; 
        // Emissive
        public Texture2D EmissiveMap { get; set; }
        public float EmissiveIntensity { get; set; } = 0.0f;
        public Color EmissiveColor { get; set; } = Color.White;
        // Reflection
        public TextureCube ReflectionMap { get; set; }
        public float ReflectionIntensity { get; set; } = 0.0f;

        public StandardMaterial() : base() { }

        protected override void SetupShaderMaterial(ContentManager content)
        {
            _shaderMaterial = new ForwardStandard(this);

            _shaderMaterial.LoadEffect(content);
        }
    }
}
