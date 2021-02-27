using System;
using RocketUI;
using SharpVR;
using Valve.VR;

namespace TruSaber.Graphics.Gui
{
    public class VRControlDebug : MultiStackContainer
    {

        public VRControlDebug()
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
                AddRow(new TextElement($"{btn.ToString()}"), new VRButtonDebugger(cxt.LeftController, btn),
                    new VRButtonDebugger(cxt.RightController, btn));
            }
            
        }
        
    }
}