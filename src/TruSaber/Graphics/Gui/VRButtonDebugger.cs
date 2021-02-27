using Microsoft.Xna.Framework;
using RocketUI;
using SharpVR;
using Valve.VR;

namespace TruSaber.Graphics.Gui
{
    public class VRButtonDebugger : StackContainer
    {
        private readonly IVRController _controller;
        private readonly EVRButtonId   _buttonId;

        private GuiStatusDot   _eDown;
        private GuiStatusDot   _eTouch;
        private Vector2Bars _eAxis;
        
        private bool    _isDown;
        private bool    _isTouch;
        private Vector2 _axisValue;
        
        public VRButtonDebugger(IVRController controller, EVRButtonId buttonId)
        {
            Orientation = Orientation.Horizontal;
            _controller = controller;
            _buttonId = buttonId;
            
            AddChild(_eDown = new GuiStatusDot());
            AddChild(_eTouch = new GuiStatusDot());
            AddChild(_eAxis = new Vector2Bars());
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