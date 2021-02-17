﻿using System;
using System.Linq;
using RocketUI.Layout;

namespace RocketUI.Controls
{
    public class GuiStackMenu : GuiScrollableStackContainer
    {
	    private bool _modern = false;

	    public bool ModernStyle
	    {
		    get => _modern;
		    set
		    {
			    _modern = value;
			    foreach (var child in AllChildren.OfType<GuiStackMenuItem>())
			    {
				    child.Modern = value;
			    }
		    }
	    }

        public GuiStackMenu()
        {
        }

        public void AddMenuItem(string label, Action action, bool enabled = true, bool isTranslationKey = false)
        {
            AddChild(new GuiStackMenuItem(label, action, isTranslationKey)
            {
				Enabled = enabled,
				Modern = ModernStyle
			});
        }

	    public void AddSpacer()
	    {
		    AddChild(new GuiStackMenuSpacer());
	    }
	}
}
