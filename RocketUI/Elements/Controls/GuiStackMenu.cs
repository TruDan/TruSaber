using System;
using System.Linq;
using RocketUI.Layout;

namespace RocketUI.Controls
{
    public class GuiStackMenu : GuiScrollableStackContainer
    {
        public GuiStackMenu()
        {
        }

        public void AddMenuItem(string label, Action action, bool enabled = true, bool isTranslationKey = false)
        {
            AddChild(new GuiStackMenuItem(label, action, isTranslationKey)
            {
				Enabled = enabled
			});
        }

	    public void AddSpacer()
	    {
		    AddChild(new GuiStackMenuSpacer());
	    }
	}
}
