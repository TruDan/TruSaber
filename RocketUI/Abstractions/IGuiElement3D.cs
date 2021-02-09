using RocketUI.Graphics;

namespace RocketUI.Abstractions
{
    public interface IGuiElement3D : IGuiElement
    {

        void Draw3D(GuiRenderArgs renderArgs);
    }
}
