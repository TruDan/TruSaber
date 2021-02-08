using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Valve.VR;

namespace SharpVR
{
    // Don't trust the wiki, it's horribly outdated. The header files contain the best docs
    // https://github.com/ValveSoftware/openvr/blob/master/headers/openvr.h#L1208)
    public class VrContext : IDisposable
    {
        private static readonly uint EventSize = (uint) Marshal.SizeOf<VREvent_t>();

        internal CVRSystem System;
        internal CVRCompositor Compositor;
        
        private readonly TrackedDevice[] _devices;
        private readonly TrackedDevicePose_t[] _lastDevicePoses;
        private readonly TrackedDevicePose_t[] _lastNextDevicePoses;
        internal readonly TrackedDevicePose_t[] ValidDevicePoses;
        internal readonly TrackedDevicePose_t[] ValidNextDevicePoses;

        // hmd is always device with index 0
        public HeadMountedDisplay Hmd => (HeadMountedDisplay) _devices[0];
        public Controller LeftController => (Controller) _devices[1];
        public Controller RightController => (Controller) _devices[2];
        
        private VrContext()
        {
            _devices = new TrackedDevice[OpenVR.k_unMaxTrackedDeviceCount];
            _lastDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
            _lastNextDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
            ValidDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
            ValidNextDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        }

        #region Initialization and Cleanup

        /// <summary>
        /// Indicates if OpenVR is initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Attempt to initialize OpenVR.
        /// </summary>
        /// <exception cref="SharpVRException">If initialization fails.</exception>
        public void Initialize()
        {
            if (Initialized)
                return;

            var error = EVRInitError.None;
            System = OpenVR.Init(ref error);
            Compositor = OpenVR.Compositor;

            if (error != EVRInitError.None)
                throw CreateException(error);

            Compositor.SetTrackingSpace(ETrackingUniverseOrigin.TrackingUniverseStanding);
            
            var strDriver = GetTrackedDeviceString(OpenVR.k_unTrackedDeviceIndex_Hmd, ETrackedDeviceProperty.Prop_TrackingSystemName_String);
            var strDisplay = GetTrackedDeviceString(OpenVR.k_unTrackedDeviceIndex_Hmd, ETrackedDeviceProperty.Prop_SerialNumber_String);

            Debug.WriteLine($"Driver: {strDriver} - Display {strDisplay}");
            
            for (var i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
                RegisterDevice(i);

            Initialized = true;
        }
        public string GetTrackedDeviceString(uint deviceIndex, ETrackedDeviceProperty prop)
        {
            var error      = ETrackedPropertyError.TrackedProp_Success;
            var bufferSize = System.GetStringTrackedDeviceProperty(deviceIndex, prop, null, 0, ref error);
            if (bufferSize == 0)
                return string.Empty;

            var buffer = new System.Text.StringBuilder((int)bufferSize);
            bufferSize = System.GetStringTrackedDeviceProperty(deviceIndex, prop, buffer, bufferSize, ref error);

            return buffer.ToString();
        }


        private TrackedDevice RegisterDevice(int index)
        {
            var device = TrackedDevice.Create(this, index);
            _devices[index] = device;
            if (device != null)
            {
                Console.WriteLine($"[VR][Device {index}]: Registered ({device.GetType().Name})");

                if (device is Controller controller)
                {
                    var role = System.GetControllerRoleForTrackedDeviceIndex((uint) index);
                    switch (role)
                    {
                        case ETrackedControllerRole.LeftHand:
                            //LeftController = controller;
                            break;
                        case ETrackedControllerRole.RightHand:
                            //RightController = controller;
                            break;
                        case ETrackedControllerRole.Invalid:
                        case ETrackedControllerRole.OptOut:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return device;
        }

        public void Shutdown()
        {
            OpenVR.Shutdown();
            Initialized = false;
        }

        #endregion

        public void Update()
        {
            if (!Initialized)
                return;
            
            ProcessEvents();
            WaitGetPoses();
            // if (LeftController == null)
            // {
            //     var index = System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
            //     RegisterDevice((int) index);
            // }
            // if (RightController == null)
            // {
            //     var index = System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
            //     RegisterDevice((int) index);
            // }

            for (var i = 0; i < _devices.Length; i++)
            {
                _devices[i]?.Update();
            }
            
           // Hmd?.Update();
           // LeftController?.Update();
           // RightController?.Update();
        }

        #region IVRSystem

        #region Display

        /// <summary>
        /// Provides the game with the minimum size that it should use for its
        /// offscreen render target to minimize pixel stretching. This size is
        /// matched with the projection matrix and distortion function and will
        /// change from display to display depending on resolution, distortion,
        /// and field of view.
        /// </summary>
        public void GetRenderTargetSize(out int width, out int height)
        {
            var w = 0u;
            var h = 0u;
            System.GetRecommendedRenderTargetSize(ref w, ref h);
            width = (int) w;
            height = (int) h;
        }

        /// <summary>
        /// Returns the projection matrix of the given eye.
        /// </summary>
        /// <param name="eye">Eye to get the projection matrix for.</param>
        /// <param name="zNear">Near plane distance.</param>
        /// <param name="zFar">Far plane distance.</param>
        /// <param name="matrix">Projection matrix; note that this is column major.</param>
        public void GetProjectionMatrix(Eye eye, float zNear, float zFar, out HmdMatrix44_t matrix)
        {
            matrix = System.GetProjectionMatrix((EVREye) eye, zNear, zFar);
        }
        
        /// <summary>
        /// Returns the projection matrix of the given eye.
        /// </summary>
        /// <param name="eye">Eye to get the projection matrix for.</param>
        /// <param name="zNear">Near plane distance.</param>
        /// <param name="zFar">Far plane distance.</param>
        /// <param name="matrix">Projection matrix; note that this is column major.</param>
        public Matrix GetProjectionMatrix(Eye eye, float zNear, float zFar)
        {
            return System.GetProjectionMatrix((EVREye) eye, zNear, zFar).ToMg();
        }

        // NOT EXPOSED: public void GetProjectionRaw

        // NOT EXPOSED: public bool ComputeDistortion

        /// <summary>
        /// Returns the view matrix transform between the view space and eye space. Eye space is the
        /// per-eye flavor of view space that provides stereo disparity. Instead of
        /// Model * View * Projection the model is Model * View * Eye * Projection. Normally
        /// View and Eye will be multiplied together and treated as View in your application.
        /// </summary>
        /// <param name="eye">Eye to get the matrix for.</param>
        /// <param name="matrix">Eye transform matrix.</param>
        /// <remarks>
        /// Matrix incorporates the user's interpupillary distance (IPD).
        /// </remarks>
        public void GetEyeMatrix(Eye eye, out HmdMatrix34_t matrix)
        {
            matrix = System.GetEyeToHeadTransform((EVREye) eye);
        }
        
        /// <summary>
        /// Returns the view matrix transform between the view space and eye space. Eye space is the
        /// per-eye flavor of view space that provides stereo disparity. Instead of
        /// Model * View * Projection the model is Model * View * Eye * Projection. Normally
        /// View and Eye will be multiplied together and treated as View in your application.
        /// </summary>
        /// <param name="eye">Eye to get the matrix for.</param>
        /// <param name="matrix">Eye transform matrix.</param>
        /// <remarks>
        /// Matrix incorporates the user's interpupillary distance (IPD).
        /// </remarks>
        public Matrix GetEyeMatrix(Eye eye)
        {
            return System.GetEyeToHeadTransform((EVREye) eye).ToMg();
        }

        // NOT EXPOSED: bool GetTimeSinceLastVsync

        // NOT EXPOSED: void GetDXGIOutputInfo

        // NOT EXPOSED: void GetD3D9AdapterIndex

        #endregion

        #region Tracking

        // TODO: GetDeviceToAbsoluteTrackingPose

        public void ResetSeatedZeroPose()
        {
            System.ResetSeatedZeroPose();
        }

        // TODO: HmdMatrix34_t GetSeatedZeroPoseToStandingAbsoluteTrackingPose()

        #endregion

        #region OpenVR Events

        public void ProcessEvents()
        {
            var ev = new VREvent_t();
            while (System.PollNextEvent(ref ev, EventSize))
                ProcessEvent(ev);
        }

        private void ProcessEvent(VREvent_t e)
        {
            Console.WriteLine($"[VR][Device {e.trackedDeviceIndex}]: {((EVREventType)e.eventType).ToString().Replace("VREvent_", "")}");
            
            switch ((EVREventType) e.eventType)
            {
                case EVREventType.VREvent_TrackedDeviceActivated:
                    HandleDeviceActivated((int) e.trackedDeviceIndex);
                    break;
                case EVREventType.VREvent_TrackedDeviceDeactivated:
                    HandleDeviceDeactivated((int) e.trackedDeviceIndex);
                    break;
                
                case EVREventType.VREvent_TrackedDeviceRoleChanged:
                    HandleDeviceRoleChanged((int) e.trackedDeviceIndex);
                    break;
                
                default:
                    Console.WriteLine($"Unhandled event '{(EVREventType) e.eventType}' ({e.eventType})");
                    break;
            }
        }

        private void HandleDeviceRoleChanged(int index)
        {
        }

        private void HandleDeviceActivated(int index)
        {
            var device = RegisterDevice(index);
            DeviceActivated?.Invoke(this, new TrackedDeviceActivateEventArgs(device));
        }

        private void HandleDeviceDeactivated(int index)
        {
            var device = _devices[index];
            _devices[index] = null;
            DeviceDeactivated?.Invoke(this, new TrackedDeviceActivateEventArgs(device));
        }

        #endregion

        #endregion

        #region Compositor

        public void WaitGetPoses()
        {
            var error = OpenVR.Compositor.WaitGetPoses(_lastDevicePoses, _lastNextDevicePoses);
            if (error != EVRCompositorError.None)
                throw CreateException(error);

            for (var i = 0; i < _lastDevicePoses.Length; i++)
            {
                var p = _lastDevicePoses[i];
                if (p.bPoseIsValid)
                    ValidDevicePoses[i] = p;
            }
            for (var i = 0; i < _lastNextDevicePoses.Length; i++)
            {
                var p = _lastNextDevicePoses[i];
                if (p.bPoseIsValid)
                    ValidNextDevicePoses[i] = p;
            }
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Check if the native openvr_api library can be called into.
        /// </summary>
        public static bool CanCallNativeDll(out string error)
        {
            // hackzzz
            error = null;
            try
            {
                RuntimeInstalled();
                return true;
            }
            catch (DllNotFoundException)
            {
                error = "Can't find openvr_api library, make sure it's next to the executable.";
                return false;
            }
            catch (BadImageFormatException)
            {
                error = "Got a BadImageFormatException, this most often indicates the native library is " +
                        "for the wrong bitness. Make sure the bitness of the native dll matches the target " +
                        "of your .NET Project.";
                return false;
            }
        }

        /// <summary>
        /// Check if the OpenVR runtime is installed.
        /// </summary>
        public static bool RuntimeInstalled()
        {
            return OpenVR.IsRuntimeInstalled();
        }

        /// <summary>
        /// Check if an HMD is present on the system.
        /// </summary>
        public static bool HmdConnected()
        {
            return OpenVR.IsHmdPresent();
        }

        #endregion

        #region Singleton

        protected static VrContext Instance;

        public static VrContext Get()
        {
            if (Instance == null)
            {
                Instance = new VrContext();
                Instance.Initialize();
            }

            return Instance;
        }

        #endregion

        #region Events

        public event EventHandler<TrackedDeviceActivateEventArgs> DeviceActivated;
        public event EventHandler<TrackedDeviceActivateEventArgs> DeviceDeactivated;

        #endregion

        private static Exception CreateException(EVRInitError error)
        {
            var str = OpenVR.GetStringForHmdError(error);
            return new SharpVRException("Initialization error: " + str, (int) error);
        }

        private Exception CreateException(EVRCompositorError error)
        {
            return new SharpVRException("Compositor error: " + error, (int) error);
        }

        ~VrContext()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Shutdown();
        }
    }
}