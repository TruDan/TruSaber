using RocketUI.Graphics;
using RocketUI.Primitive;

namespace RocketUI
{
    public class GuiCrosshair : GuiElement
    {
        public GuiCrosshair()
        {
            Anchor = Alignment.MiddleCenter;
            Width = 15;
            Height = 15;
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            Background = renderer.GetTexture(GuiTextures.Crosshair);
        }
    }
}
