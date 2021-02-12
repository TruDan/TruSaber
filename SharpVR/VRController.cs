﻿using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Valve.VR;

namespace SharpVR
{

    public class VRButtonMask
    {
        public const ulong System          = (1ul << (int) EVRButtonId.System); // reserved
        public const ulong ApplicationMenu = (1ul << (int) EVRButtonId.ApplicationMenu);
        public const ulong Grip            = (1ul << (int) EVRButtonId.Grip);
        public const ulong Axis0           = (1ul << (int) EVRButtonId.Axis0);
        public const ulong Axis1           = (1ul << (int) EVRButtonId.Axis1);
        public const ulong Axis2           = (1ul << (int) EVRButtonId.Axis2);
        public const ulong Axis3           = (1ul << (int) EVRButtonId.Axis3);
        public const ulong Axis4           = (1ul << (int) EVRButtonId.Axis4);
        public const ulong Touchpad        = (1ul << (int) EVRButtonId.SteamVRTouchpad);
        public const ulong Trigger         = (1ul << (int) EVRButtonId.SteamVRTrigger);
    }

    public interface IVRController : ITrackedDevice
    {
        Hand Hand { get; }
    }

    public class EmulatedController : TrackedDevice, IVRController
    {
        public Hand Hand { get; }
        
        public EmulatedController(VrContext context, int index, Hand hand) : base(context, index)
        {
            Hand = hand;
        }
    }
    
    public class VRController : TrackedDevice, IVRController
    {
        public Hand Hand { get; }

        private TrackedDevicePose_t _currentPose;
        
        private VRControllerState_t _lastState;
        private VRControllerState_t _currentState;

        public bool ButtonPressed(EVRButtonId button)
        {
            return (_lastState.ulButtonPressed & (1ul << (int) button)) == 0 &&
                   (_currentState.ulButtonPressed & (1ul << (int) button)) != 0;
        }

        public override void Update()
        {
            _lastState = _currentState;
            _currentPose = Context.ValidDevicePoses[Index];
            Context.System.GetControllerState((uint) Index, ref _currentState, (uint) Marshal.SizeOf<VRControllerState_t>());
            //Context.System.GetControllerStateWithPose(ETrackingUniverseOrigin.TrackingUniverseStanding, (uint) Index, ref _currentState, (uint) Marshal.SizeOf<VRControllerState_t>(), ref _currentPose);
                //throw new SharpVRException("Getting controller state failed");
            base.Update();
        }

        
        public void GetAxis(EVRButtonId buttonId, ref Vector2 result)
        {
            var axisId = (uint)buttonId - (uint)EVRButtonId.Axis0;
            switch (axisId)
            {
                case 0:
                    result.X = _currentState.rAxis0.x;
                    result.Y = _currentState.rAxis0.y;
                    break;

                case 1:
                    result.X = _currentState.rAxis1.x;
                    result.Y = _currentState.rAxis1.y;
                    break;
                case 2:
                    result.X = _currentState.rAxis2.x;
                    result.Y = _currentState.rAxis2.y;
                    break;
                case 3:
                    result.X = _currentState.rAxis3.x;
                    result.Y = _currentState.rAxis3.y;
                    break;

                case 4:
                    result.X = _currentState.rAxis4.x;
                    result.Y = _currentState.rAxis4.y;
                    break;

                default:
                    result.X = 0;
                    result.Y = 0;
                    break;
            }
        }
        
        public bool GetPress(ulong buttonMask) => (_currentState.ulButtonPressed & buttonMask) != 0;
        public bool GetPressDown(ulong buttonMask) => (_currentState.ulButtonPressed & buttonMask) != 0 && (_lastState.ulButtonPressed & buttonMask) == 0;
        public bool GetPressUp(ulong buttonMask) => (_currentState.ulButtonPressed & buttonMask) == 0 && (_lastState.ulButtonPressed & buttonMask) != 0;
        
        public bool GetPress(EVRButtonId buttonId) => GetPress(1ul << (int)buttonId);
        public bool GetPressDown(EVRButtonId buttonId) => GetPressDown(1ul << (int)buttonId);
        public bool GetPressUp(EVRButtonId buttonId) => GetPressUp(1ul << (int)buttonId);

        public bool GetTouch(ulong buttonMask) => (_currentState.ulButtonTouched & buttonMask) != 0;
        public bool GetTouchDown(ulong buttonMask) => (_currentState.ulButtonTouched & buttonMask) != 0 && (_lastState.ulButtonTouched & buttonMask) == 0;
        public bool GetTouchUp(ulong buttonMask) => (_currentState.ulButtonTouched & buttonMask) == 0 && (_lastState.ulButtonTouched & buttonMask) != 0;

        public bool GetTouch(EVRButtonId buttonId) => GetTouch(1ul << (int)buttonId);
        public bool GetTouchDown(EVRButtonId buttonId) => GetTouchDown(1ul << (int)buttonId);
        public bool GetTouchUp(EVRButtonId buttonId) => GetTouchUp(1ul << (int)buttonId);


        internal VRController(VrContext context, int index) : base(context, index)
        {
            var role = context.System.GetControllerRoleForTrackedDeviceIndex((uint) index);
            switch (role)
            {
                case ETrackedControllerRole.LeftHand:
                    Hand = Hand.Left;
                    break;
                case ETrackedControllerRole.RightHand:
                    Hand = Hand.Right;
                    break;
                default:
                    Hand = Hand.None;
                    break;
            }
        }
    }
}