using RocketUI.Attributes;

namespace RocketUI.Abstractions
{
    public interface IFocusableElement : IGuiElement
    {
        [DebuggerVisible] bool Focused { get; set; }
    }
}
