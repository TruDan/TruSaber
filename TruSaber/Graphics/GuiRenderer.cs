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
            
            LoadTexturesFromContent();
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

        private void LoadTexturesFromContent()
        {
            var c        = TruSaberGame.Instance.Game.Content;
            var basePath = "Textures/UI/";

            var buttons = c.Load<Texture2D>(basePath + "Buttons");
            LoadTextureFromSpriteSheet(GuiTextures.ButtonDefault, buttons, ButtonBackgroundDefault, new Thickness(50));
            LoadTextureFromSpriteSheet(GuiTextures.ButtonHover, buttons, ButtonBackgroundHover, new Thickness(50));
            LoadTextureFromSpriteSheet(GuiTextures.ButtonFocused, buttons, ButtonBackgroundFocus, new Thickness(50));
            LoadTextureFromSpriteSheet(GuiTextures.ButtonDisabled, buttons, ButtonBackgroundDisabled, new Thickness(50));
            
            LoadTextureFromSpriteSheet(GuiTextures.PanelGeneric, buttons, PanelSolid, new Thickness(15));
            LoadTextureFromSpriteSheet(GuiTextures.Crosshair, buttons, CrosshairWhite, Thickness.Zero);
            
            LoadTextureFromSpriteSheet(GuiTextures.DotBlue, buttons,StatusDotBlue, Thickness.Zero);
            LoadTextureFromSpriteSheet(GuiTextures.DotGreen, buttons,StatusDotGreen, Thickness.Zero);
            LoadTextureFromSpriteSheet(GuiTextures.DotOrange, buttons,StatusDotOrange, Thickness.Zero);
            LoadTextureFromSpriteSheet(GuiTextures.DotYellow, buttons,StatusDotYellow, Thickness.Zero);
            LoadTextureFromSpriteSheet(GuiTextures.DotWhite, buttons,StatusDotWhite, Thickness.Zero);
            LoadTextureFromSpriteSheet(GuiTextures.DotShadow, buttons,StatusDotShadow, Thickness.Zero);
            
            LoadTextureFromSpriteSheet(GuiTextures.ProgressBar, buttons,ProgressBar, Thickness.Zero);
            
            var scrollbar = c.Load<Texture2D>(basePath + "ScrollBar");
        }


        #region Impl

        private static readonly Rectangle ProgressBar = new Rectangle(632, 112, 12, 12);

        #region Buttons

        // Buttons Region: 0,0 -> 432,416
        private static readonly Rectangle ButtonBackgroundDefault  = new Rectangle(0, 0, 432, 104);
        private static readonly Rectangle ButtonBackgroundHover    = new Rectangle(0, 104, 432, 104);
        private static readonly Rectangle ButtonBackgroundFocus    = new Rectangle(0, 208, 432, 104);
        private static readonly Rectangle ButtonBackgroundDisabled = new Rectangle(0, 312, 432, 104);
        
        #endregion

        #region Panels

        private static readonly Point PanelsOffset = new Point(432, 0);
        private static readonly Rectangle PanelSolid = new Rectangle(PanelsOffset.X + 0, PanelsOffset.Y + 0, 100, 100);
        private static readonly Rectangle PanelGlassTL = new Rectangle(PanelsOffset.X + 0, PanelsOffset.Y + 100, 100, 100);
        private static readonly Rectangle PanelGlassBL = new Rectangle(PanelsOffset.X + 0, PanelsOffset.Y + 200, 100, 100);
        private static readonly Rectangle PanelGlassTR = new Rectangle(PanelsOffset.X + 100, PanelsOffset.Y + 100, 100, 100);
        private static readonly Rectangle PanelGlassBR = new Rectangle(PanelsOffset.X + 100, PanelsOffset.Y + 200, 100, 100);
        private static readonly Rectangle PanelGlassInset = new Rectangle(PanelsOffset.X + 0, PanelsOffset.Y + 300, 100, 100);
        private static readonly Rectangle PanelGlass = new Rectangle(PanelsOffset.X + 100, PanelsOffset.Y + 300, 100, 100);

        #endregion

        #region Crosshairs

        private static readonly Point     CrosshairsOffset = new Point(632, 0);
        private static readonly Size      CrosshairsSize   = new Size(32, 32);
        private static readonly Rectangle CrosshairWhite = new Rectangle(CrosshairsOffset + Point.Zero, CrosshairsSize);
        private static readonly Rectangle CrosshairBlack = new Rectangle(CrosshairsOffset + new Point(CrosshairsSize.Width*1, 0), CrosshairsSize);
        private static readonly Rectangle CrosshairRed = new Rectangle(CrosshairsOffset + new Point(CrosshairsSize.Width*2, 0), CrosshairsSize);
        private static readonly Rectangle CrosshairBlue = new Rectangle(CrosshairsOffset + new Point(CrosshairsSize.Width*3, 0), CrosshairsSize);

        #endregion

        #region Dots

        public static readonly Rectangle StatusDotBlue   = new Rectangle(632, 88, 24, 24);
        public static readonly Rectangle StatusDotGreen  = new Rectangle(632 + 24, 88, 24, 24);
        public static readonly Rectangle StatusDotOrange = new Rectangle(632 + (2 * 24), 88, 24, 24);
        public static readonly Rectangle StatusDotYellow = new Rectangle(632 + (3 * 24), 88, 24, 24);
        public static readonly Rectangle StatusDotWhite  = new Rectangle(632 + (4 * 24), 88, 24, 24);
        public static readonly Rectangle StatusDotShadow = new Rectangle(632 + (5 * 24), 88, 24, 24);
        
        #endregion

        #region ScrollBar

        public static readonly Rectangle ScrollBarBackgroundDefault  = new Rectangle(0, 0, 10, 10);
        public static readonly Rectangle ScrollBarBackgroundHover    = new Rectangle(0, 0, 10, 10);
        public static readonly Rectangle ScrollBarBackgroundFocus    = new Rectangle(0, 0, 10, 10);
        public static readonly Rectangle ScrollBarBackgroundDisabled = new Rectangle(0, 0, 10, 10);

        public static readonly Rectangle ScrollBarTrackDefault  = new Rectangle(10, 10, 10, 10);
        public static readonly Rectangle ScrollBarTrackHover    = new Rectangle(10, 10, 10, 10);
        public static readonly Rectangle ScrollBarTrackFocus    = new Rectangle(10, 10, 10, 10);
        public static readonly Rectangle ScrollBarTrackDisabled = new Rectangle(10, 10, 10, 10);

        public static readonly Rectangle ScrollBarUpButtonDefault  = new Rectangle(20, 20, 10, 10);
        public static readonly Rectangle ScrollBarUpButtonHover    = new Rectangle(20, 20, 10, 10);
        public static readonly Rectangle ScrollBarUpButtonFocus    = new Rectangle(20, 20, 10, 10);
        public static readonly Rectangle ScrollBarUpButtonDisabled = new Rectangle(20, 20, 10, 10);

        public static readonly Rectangle ScrollBarDownButtonDefault  = new Rectangle(30, 30, 10, 10);
        public static readonly Rectangle ScrollBarDownButtonHover    = new Rectangle(30, 30, 10, 10);
        public static readonly Rectangle ScrollBarDownButtonFocus    = new Rectangle(30, 30, 10, 10);
        public static readonly Rectangle ScrollBarDownButtonDisabled = new Rectangle(30, 30, 10, 10);
        
        #endregion

        #endregion
        
        
        private TextureSlice2D LoadTextureFromEmbeddedResource(GuiTextures guiTexture, byte[] resource)
        {
            //_textureCache[guiTexture] = TextureUtils.ImageToTexture2D(_graphicsDevice, resource);
            return _textureCache[guiTexture];
        }
        
        private void LoadTextureFromSpriteSheet(GuiTextures guiTexture, Texture2D spriteSheet, Rectangle sliceRectangle, Thickness ninePatchThickness)
        {
            _textureCache[guiTexture] = new NinePatchTexture2D(spriteSheet.Slice(sliceRectangle), ninePatchThickness);
        }

        private void LoadTextureFromSpriteSheet(GuiTextures guiTexture, Texture2D spriteSheet, Rectangle sliceRectangle)
        {
            _textureCache[guiTexture] = spriteSheet.Slice(sliceRectangle);
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