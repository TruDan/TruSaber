namespace RocketUI.Abstractions
{
    public interface IGuiScreen : IGuiElement, IGuiFocusContext
    {

        void UpdateLayout();

    }
}
