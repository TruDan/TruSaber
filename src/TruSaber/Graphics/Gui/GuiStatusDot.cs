using Microsoft.Xna.Framework;
using RocketUI;

namespace TruSaber.Graphics.Gui
{
    public class GuiStatusDot : RocketElement
    {
        private bool?        _status;
        private GuiTexture2D _defaultStatusBackground;
        private GuiTexture2D _activeStatusBackground;
        private GuiTexture2D _inactiveStatusBackground;

        public GuiTexture2D DefaultStatusBackground
        {
            get
            {
                if (_defaultStatusBackground == null)
                    _defaultStatusBackground = new GuiTexture2D();
                return _defaultStatusBackground;
            }
            set => _defaultStatusBackground = value;
        }

        public GuiTexture2D InactiveStatusBackground
        {
            get
            {
                if (_inactiveStatusBackground == null)
                    _inactiveStatusBackground = new GuiTexture2D();
                return _inactiveStatusBackground;
            }
            set => _inactiveStatusBackground = value;
        }

        public GuiTexture2D ActiveStatusBackground
        {
            get
            {
                if (_activeStatusBackground == null)
                    _activeStatusBackground = new GuiTexture2D();
                return _activeStatusBackground;
            }
            set => _activeStatusBackground = value;
        }

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