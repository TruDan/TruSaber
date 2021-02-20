using System;
using Microsoft.Extensions.DependencyInjection;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Graphics;
using RocketUI.Input.Listeners;
using RocketUI.Layout;
using RocketUI.Primitive;
using SharpVR;
using Valve.VR;

namespace TruSaber.Graphics.Gui
{
    public class GuiVRControlDebug : GuiMultiStackContainer
    {

        public GuiVRControlDebug()
        {
            Background = GuiTextures.PanelGeneric;
            ClipToBounds = false;
            Padding = new Thickness(50);

            Orientation = Orientation.Vertical;
            ChildAnchor = Alignment.TopFill;
            MinWidth = 200;
            MinHeight = 300;
            
            var cxt = VrContext.Get();
            foreach (var btn in Enum.GetValues<EVRButtonId>())
            {
                AddRow(new GuiTextElement($"{btn.ToString()}"), new GuiVRButtonDebugger(cxt.LeftController, btn),
                    new GuiVRButtonDebugger(cxt.RightController, btn));
            }
            
        }
        
    }
}