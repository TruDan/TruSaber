using Microsoft.Xna.Framework;
using RocketUI;

namespace TruSaber.Graphics.Gui
{
    public class GuiStatusDot : GuiElement
    {
        private bool? _status;

        public GuiTexture2D DefaultStatusBackground;
        public GuiTexture2D InactiveStatusBackground;
        public GuiTexture2D ActiveStatusBackground;
        
        public bool? Status
        {
            get => _status;
            set
            {
                _status = value;
            }
        }
        protected override void OnInit(IGuiRenderer renderer)
        {
            base.OnInit(renderer);
            DefaultStatusBackground.TryResolveTexture(renderer);
            InactiveStatusBackground.TryResolveTexture(renderer);
            ActiveStatusBackground.TryResolveTexture(renderer);
        }

        public GuiStatusDot()
        {
            DefaultStatusBackground = GuiTextures.DotShadow;
            ActiveStatusBackground = GuiTextures.DotGreen;
            InactiveStatusBackground = GuiTextures.DotWhite;

            DefaultStatusBackground.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;
            ActiveStatusBackground.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;
            InactiveStatusBackground.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;
            
            SetFixedSize(15,15);
        }

        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            if (_status.HasValue)
            {
                if (_status.Value)
                {
                    graphics.FillRectangle(RenderBounds, ActiveStatusBackground);
                }
                else
                {
                    graphics.FillRectangle(RenderBounds, InactiveStatusBackground);
                }
            }
            else
            {
                base.OnDraw(graphics, gameTime);
            }
        }
    }
}