using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;
using RocketUI.Graphics;
using RocketUI.Graphics.Textures;
using RocketUI.Graphics.Typography;

namespace TruSaber.Graphics
{
    public class GuiRenderer : IGuiRenderer
    {
        public GuiScaledResolution ScaledResolution { get; set; }
        public void Init(GraphicsDevice graphics, IServiceProvider serviceProvider)
        {
            
        }

        public IFont Font { get; set; }
        public TextureSlice2D GetTexture(GuiTextures guiTexture)
        {
            return (TextureSlice2D) GpuResourceManager.CreateTexture2D(1, 1);
        }

        public Texture2D GetTexture2D(GuiTextures    guiTexture)
        {
            return GetTexture(guiTexture).Texture;
        }

        public string GetTranslation(string          key)
        {
            return key;
        }

        public Vector2 Project(Vector2               point)
        {
            return Vector2.Transform(point, ScaledResolution.TransformMatrix);
        }

        public Vector2 Unproject(Vector2             screen)
        {
            return Vector2.Transform(screen, ScaledResolution.InverseTransformMatrix);
        }
    }
}