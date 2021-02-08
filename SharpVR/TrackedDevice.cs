using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using TruSaber.Utilities.Extensions;
using Valve.VR;

namespace SharpVR
{
    public abstract class TrackedDevice
    {
        protected readonly VrContext Context;
        public int Index { get; }

        public bool IsConnected => Context.System.IsTrackedDeviceConnected((uint) Index);

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;

        public void GetRelativePosition(ref Vector3    position) => position = _position;
        public void GetRelativeRotation(ref Quaternion rotation) => rotation = _rotation;
        public void GetRelativeScale(ref    Vector3    scale) => scale = _scale;

        public Vector3    LocalPosition => _position;
        public Quaternion LocalRotation => _rotation;
        public Vector3    LocalScale => _scale;
        
        internal TrackedDevice(VrContext context, int index)
        {
            Context = context;
            Index = index;
        }

        public Matrix GetPose()
        {
            return Context.ValidDevicePoses[Index].mDeviceToAbsoluteTracking.ToMg();
        }

        public Matrix GetNextPose()
        {
            return Context.ValidNextDevicePoses[Index].mDeviceToAbsoluteTracking.ToMg();
        }

        public Vector3 GetVelocity()
        {
            return Context.ValidDevicePoses[Index].vVelocity.ToMg();
        }

        public Vector3 GetAngularVelocity()
        {
            return Context.ValidDevicePoses[Index].vAngularVelocity.ToMg();
        }

        public virtual void Update()
        {
            var transform = GetPose();
            transform = Matrix.Invert(transform);
            if (transform.Decompose(out var scale, out var rotation, out var position))
            {
                _scale = scale;
                _rotation = rotation;
                _position = position;

                var rotationEuler = _rotation.ToEuler();
                Debug.WriteLine($"[VR][Device {Index}] Position: X={_position.X:F2}, Y={_position.Y:F2}, Z={_position.Z:F2}\tRotation: Yaw={MathHelper.ToDegrees(rotationEuler.X):F2}, Pitch={MathHelper.ToDegrees(rotationEuler.Y):F2}, Roll={MathHelper.ToDegrees(rotationEuler.Z):F2}\t Scale: X={_scale.X:F2}, Y={_scale.Y:F2}, Z={_scale.Z:F2}");
            }
        }
        
        internal static TrackedDevice Create(VrContext context, int index)
        {
            var cls = context.System.GetTrackedDeviceClass((uint) index);
            switch (cls)
            {
                case ETrackedDeviceClass.Invalid:
                    return null;
                case ETrackedDeviceClass.HMD:
                    return new HeadMountedDisplay(context, index);
                case ETrackedDeviceClass.Controller:
                    return new Controller(context, index);
                case ETrackedDeviceClass.GenericTracker:
                    return new GenericTracker(context, index);
                case ETrackedDeviceClass.TrackingReference:
                    return new TrackingReference(context, index);
                case ETrackedDeviceClass.DisplayRedirect:
                    return new DisplayRedirect(context, index);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected Exception CreateException(ETrackedPropertyError error)
        {
            var str = Context.System.GetPropErrorNameFromEnum(error);
            return new SharpVRException("Get property error: " + str, (int) error);
        }
    }
}