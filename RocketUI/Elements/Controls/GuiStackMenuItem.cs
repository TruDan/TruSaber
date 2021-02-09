using System;

namespace RocketUI.Controls
{
    public class GuiStackMenuItem : GuiButton
    {
        public GuiStackMenuItem(string text, Action action, bool isTranslationKey = false) : base(text, action, isTranslationKey)
        {
        }
        
        public GuiStackMenuItem(){}
    }
}
