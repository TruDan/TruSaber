namespace RocketUI.Abstractions
{
    public interface IGuiScreen : IGuiElement, IGuiFocusContext
    {
        GuiManager GuiManager { get; }
        void UpdateLayout();

    }
}
