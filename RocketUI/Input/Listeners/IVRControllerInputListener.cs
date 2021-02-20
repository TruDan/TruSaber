using System;
using Microsoft.Xna.Framework;
using SharpVR;
using Valve.VR;

namespace RocketUI.Input.Listeners
{
    public interface IVRControllerInputListener
    {
    }

    public struct VRControllerPairState
    {
        public VRControllerState Left  { get; }
        public VRControllerState Right { get; }

        public VRControllerPairState(VRControllerState left, VRControllerState right)
        {
            Left = left;
            Right = right;
        }
    }

    public struct VRControllerState
    {
        public static readonly VRControllerState Default;

        public ulong ButtonPressed;
        public ulong ButtonTouched;

        public Vector2[] Axis;

        public static VRControllerState FromSharpVR(Valve.VR.VRControllerState state)
        {
            return new VRControllerState()
            {
                Axis = new[]
                {
                    ToXna(state.rAxis0),
                    ToXna(state.rAxis1),
                    ToXna(state.rAxis2),
                    ToXna(state.rAxis3),
                    ToXna(state.rAxis4),
                },
                ButtonPressed = state.ulButtonPressed,
                ButtonTouched = state.ulButtonTouched,
            };
        }

        private static Vector2 ToXna(VRControllerAxis_t axis)
        {
            return new Vector2(axis.x, axis.y);
        }
    }

    [Flags]
    public enum VRButtons : ulong
    {
        LeftSystem          = Left | EVRButtonId.System,
        LeftApplicationMenu = Left | EVRButtonId.ApplicationMenu,
        LeftGrip            = Left | EVRButtonId.Grip,
        LeftDPadLeft        = Left | EVRButtonId.DPadLeft,
        LeftDPadUp          = Left | EVRButtonId.DPadUp,
        LeftDPadRight       = Left | EVRButtonId.DPadRight,
        LeftDPadDown        = Left | EVRButtonId.DPadDown,
        LeftA               = Left | EVRButtonId.A,
        LeftProximitySensor = Left | EVRButtonId.ProximitySensor,
        LeftAxis0           = Left | EVRButtonId.Axis0,
        LeftAxis1           = Left | EVRButtonId.Axis1,
        LeftAxis2           = Left | EVRButtonId.Axis2,
        LeftAxis3           = Left | EVRButtonId.Axis3,
        LeftAxis4           = Left | EVRButtonId.Axis4,
        LeftSteamVRTouchpad = Left | EVRButtonId.SteamVRTouchpad,
        LeftSteamVRTrigger  = Left | EVRButtonId.SteamVRTrigger,
        LeftDashboardBack   = Left | EVRButtonId.DashboardBack,
        LeftMax             = Left | EVRButtonId.Max,

        RightSystem          = Right | EVRButtonId.System,
        RightApplicationMenu = Right | EVRButtonId.ApplicationMenu,
        RightGrip            = Right | EVRButtonId.Grip,
        RightDPadLeft        = Right | EVRButtonId.DPadLeft,
        RightDPadUp          = Right | EVRButtonId.DPadUp,
        RightDPadRight       = Right | EVRButtonId.DPadRight,
        RightDPadDown        = Right | EVRButtonId.DPadDown,
        RightA               = Right | EVRButtonId.A,
        RightProximitySensor = Right | EVRButtonId.ProximitySensor,
        RightAxis0           = Right | EVRButtonId.Axis0,
        RightAxis1           = Right | EVRButtonId.Axis1,
        RightAxis2           = Right | EVRButtonId.Axis2,
        RightAxis3           = Right | EVRButtonId.Axis3,
        RightAxis4           = Right | EVRButtonId.Axis4,
        RightSteamVRTouchpad = Right | EVRButtonId.SteamVRTouchpad,
        RightSteamVRTrigger  = Right | EVRButtonId.SteamVRTrigger,
        RightDashboardBack   = Right | EVRButtonId.DashboardBack,
        RightMax             = Right | EVRButtonId.Max,

        Left       = 0b00000000,
        Right      = 0b10000000,
        ButtonMask = 0x01111111
    }


    public class VRControllerInputListener : InputListenerBase<VRControllerPairState, VRButtons>, ICursorInputListener
    {
        public Hand       ActiveHand       { get; private set; }
        public Quaternion ControllerOffset { get; set; }

        private VrContext _vrContext;

        public VRControllerInputListener(PlayerIndex playerIndex) : base(playerIndex)
        {
            RegisterMap(InputCommand.MoveForwards, VRButtons.LeftDPadUp);
            RegisterMap(InputCommand.MoveBackwards, VRButtons.LeftDPadDown);
            RegisterMap(InputCommand.MoveLeft, VRButtons.LeftDPadLeft);
            RegisterMap(InputCommand.MoveRight, VRButtons.LeftDPadRight);

            RegisterMap(InputCommand.LookUp, VRButtons.RightDPadUp);
            RegisterMap(InputCommand.LookDown, VRButtons.RightDPadDown);
            RegisterMap(InputCommand.LookLeft, VRButtons.RightDPadLeft);
            RegisterMap(InputCommand.LookRight, VRButtons.RightDPadRight);
            
            RegisterMap(InputCommand.A, VRButtons.LeftA);
            RegisterMap(InputCommand.A, VRButtons.RightA);
            RegisterMap(InputCommand.LeftClick, VRButtons.LeftSteamVRTrigger);
            RegisterMap(InputCommand.LeftClick, VRButtons.RightSteamVRTrigger);

            _vrContext = VrContext.Get();
            ActiveHand = Hand.Right;
            ControllerOffset = Quaternion.CreateFromYawPitchRoll(0f, -(MathF.PI/2f), 0f);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (CurrentState.Left.ButtonPressed > 0 && CurrentState.Right.ButtonPressed == 0)
            {
                ActiveHand = Hand.Left;
            }
            else if (CurrentState.Left.ButtonPressed == 0 && CurrentState.Right.ButtonPressed > 0)
            {
                ActiveHand = Hand.Right;
            }
        }

        protected override VRControllerPairState GetCurrentState()
        {
            var leftHand  = _vrContext.LeftController.State;
            var rightHand = _vrContext.RightController.State;

            return new VRControllerPairState(VRControllerState.FromSharpVR(leftHand),
                VRControllerState.FromSharpVR(rightHand));
        }

        protected override bool IsButtonDown(VRControllerPairState state, VRButtons buttons)
        {
            if (buttons.HasFlag(VRButtons.Right))
            {
                var vrButtons = (ulong) (buttons & VRButtons.ButtonMask);
                return (state.Right.ButtonPressed & vrButtons) == vrButtons;
            }
            else
            {
                var vrButtons = (ulong) (buttons & VRButtons.ButtonMask);
                return (state.Left.ButtonPressed & vrButtons) == vrButtons;
            }
        }

        protected override bool IsButtonUp(VRControllerPairState state, VRButtons buttons)
        {
            if (buttons.HasFlag(VRButtons.Right))
            {
                var vrButtons = (ulong) (buttons & VRButtons.ButtonMask);
                return (state.Right.ButtonPressed & vrButtons) == 0;
            }
            else
            {
                var vrButtons = (ulong) (buttons & VRButtons.ButtonMask);
                return (state.Left.ButtonPressed & vrButtons) == 0;
            }
        }

        public Ray GetCursorRay()
        {
            if (ActiveHand == Hand.Left)
            {
                return _vrContext.LeftController.GetPointer();
            }
            else if (ActiveHand == Hand.Right)
            {
                return _vrContext.RightController.GetPointer();
            }
            
            return new Ray(Vector3.Zero, Vector3.Forward);
        }
    }
}