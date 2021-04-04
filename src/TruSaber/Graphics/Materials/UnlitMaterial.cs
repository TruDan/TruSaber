using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using TruSaber.Graphics.Shaders.Deferred;
using TruSaber.Graphics.Shaders.Forward;

namespace TruSaber.Graphics.Materials
{
    public class UnlitMaterial : Material
    {
        public bool  CutoutEnabled { get; set; }
        public float Cutout        { get; set; } = 0.25f;

        public UnlitMaterial() : base()
        {
        }

        protected override void SetupShaderMaterial(ContentManager content)
        {
            _shaderMaterial = new ForwardUnlit(this);

            _shaderMaterial.LoadEffect(content);
        }
    }
}