using Microsoft.Xna.Framework;

namespace RocketUI
{
    public class GuiCrosshair : GuiElement
    {
        public GuiCrosshair()
        {
            AutoSizeMode = AutoSizeMode.None;
            Anchor = Alignment.TopLeft;
            Width = 15;
            Height = 15;
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            Background = GuiTextures.Crosshair;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Screen.GuiManager != null)
            {
                var pos = Screen.GuiManager.FocusManager.CursorPosition.ToPoint();
                Margin = new Thickness(pos.X, pos.Y, 0, 0);
            }
            base.OnUpdate(gameTime);
        }
    }
}
