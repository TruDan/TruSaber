using Microsoft.Xna.Framework;
using RocketUI.Layout;
using RocketUI.Primitive;
using SharpVR;
using Valve.VR;

namespace TruSaber.Graphics.Gui
{
    public class GuiVRButtonDebugger : GuiStackContainer
    {
        private readonly IVRController _controller;
        private readonly EVRButtonId   _buttonId;

        private GuiStatusDot   _eDown;
        private GuiStatusDot   _eTouch;
        private GuiVector2Bars _eAxis;
        
        private bool    _isDown;
        private bool    _isTouch;
        private Vector2 _axisValue;
        
        public GuiVRButtonDebugger(IVRController controller, EVRButtonId buttonId)
        {
            Orientation = Orientation.Horizontal;
            _controller = controller;
            _buttonId = buttonId;
            
            AddChild(_eDown = new GuiStatusDot());
            AddChild(_eTouch = new GuiStatusDot());
            AddChild(_eAxis = new GuiVector2Bars());
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            
            _isDown = _controller.GetPress(_buttonId);
            _eDown.Status = _isDown;
            
            _isTouch = _controller.GetTouch(_buttonId);
            _eTouch.Status = _isTouch;
            
            _controller.GetAxis(_buttonId, ref _axisValue);
            _eAxis.Value = _axisValue;
        }
    }
}