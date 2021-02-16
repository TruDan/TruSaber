using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;
using RocketUI.Graphics;
using RocketUI.Graphics.Textures;
using RocketUI.Graphics.Typography;
using RocketUI.Primitive;

namespace TruSaber.Graphics
{
    public class GuiRenderer : IGuiRenderer
    {
        public  GuiScaledResolution                     ScaledResolution { get; set; }
        
        private GraphicsDevice                          _graphicsDevice;
        private Dictionary<GuiTextures, TextureSlice2D> _textureCache = new Dictionary<GuiTextures, TextureSlice2D>();
        
        
        public void Init(GraphicsDevice graphics, IServiceProvider serviceProvider)
        {
            _graphicsDevice = graphics;
            Font = (WrappedSpriteFont) TruSaberGame.Instance.Game.Content.Load<SpriteFont>("Fonts/Default");
            
            LoadEmbeddedTextures();
        }

        public IFont Font { get; set; }
        public TextureSlice2D GetTexture(GuiTextures guiTexture)
        {
            if (_textureCache.TryGetValue(guiTexture, out var texture))
            {
                return texture;
            }

            return (TextureSlice2D) GpuResourceManager.CreateTexture2D(1, 1);
        }

        public Texture2D GetTexture2D(GuiTextures    guiTexture)
        {
            return GetTexture(guiTexture).Texture;
        }

        #region Texture Loading

        private void LoadEmbeddedTextures()
        {

        }
        
        private TextureSlice2D LoadTextureFromEmbeddedResource(GuiTextures guiTexture, byte[] resource)
        {
            //_textureCache[guiTexture] = TextureUtils.ImageToTexture2D(_graphicsDevice, resource);
            return _textureCache[guiTexture];
        }
        
        private void LoadTextureFromSpriteSheet(GuiTextures guiTexture, Texture2D spriteSheet, Rectangle sliceRectangle, Thickness ninePatchThickness, Size originalSize)
        {
            var widthScaler  = spriteSheet.Width / originalSize.Width;
            var heightScaler = spriteSheet.Height / originalSize.Height;
			
            _textureCache[guiTexture] = new NinePatchTexture2D(spriteSheet.Slice(new Rectangle(sliceRectangle.X * widthScaler,
                sliceRectangle.Y * heightScaler, sliceRectangle.Width * widthScaler,
                sliceRectangle.Height * heightScaler)), ninePatchThickness);
        }

        private void LoadTextureFromSpriteSheet(GuiTextures guiTexture, Texture2D spriteSheet, Rectangle sliceRectangle, Size originalSize)
        {
            var widthScaler  = spriteSheet.Width / originalSize.Width;
            var heightScaler = spriteSheet.Height / originalSize.Height;

            _textureCache[guiTexture] = spriteSheet.Slice(new Rectangle(sliceRectangle.X * widthScaler,
                sliceRectangle.Y * heightScaler, sliceRectangle.Width * widthScaler,
                sliceRectangle.Height * heightScaler));
        }

        #endregion

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