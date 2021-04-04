using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Graphics.Shaders;

namespace TruSaber.Graphics.Materials
{ 
    public abstract class Material : IDisposable
    {
        protected internal Vector3 _diffuseColor;
        protected internal bool _hasAlpha;
        protected internal ShaderMaterial _shaderMaterial;

        public string Name { get; set; }

        public Color DiffuseColor
        {
            get => new Color(_diffuseColor);
            set => _diffuseColor = value.ToVector3();
        }

        public Texture2D MainTexture { get; set; }

        public Vector2 Tiling { get; set; }

        public Vector2 Offset { get; set; }

        public Material()
        {
            Name = $"Material_{Guid.NewGuid()}";
            Tiling = Vector2.One;
            Offset = Vector2.Zero;
            _diffuseColor = Color.White.ToVector3();
            _hasAlpha = false;
            //Scene.current?.SetMaterial(this, true);
        }

        public virtual void LoadContent(ContentManager content)
        {
            SetupShaderMaterial(content);
        }

        protected abstract void SetupShaderMaterial(ContentManager content);

        public virtual void Dispose() { }
    }
}
