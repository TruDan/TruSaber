using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SharpVR;
using TruSaber.Abstractions;
using TruSaber.Debugging;
using TruSaber.Graphics;
using Valve.VR;
using ILogger = NLog.ILogger;

namespace TruSaber.Services
{
    public class VRService : GameComponent, IVRService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VRService> _logger;
        private VrContext _context;

        private VREyeCamera[] Cameras;
        
        private Matrix _hmdPose;

        public VRService(IServiceProvider serviceProvider, IGame game, ILogger<VRService> logger) : base(game.Game)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            UpdateOrder = -100;

            Cameras = new VREyeCamera[2];
            Cameras[0] = new VREyeCamera(((IGame) Game), Eye.Left);
            Cameras[1] = new VREyeCamera(((IGame) Game), Eye.Right);
            Game.Components.Add(Cameras[0]);
            Game.Components.Add(Cameras[1]);
            ((IGame)Game).Cameras.Add(Cameras[0]);
            ((IGame)Game).Cameras.Add(Cameras[1]);
        }

        public override void Initialize()
        {
            Game.IsFixedTimeStep = false;
            ((IGame)Game).GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            ((IGame)Game).GraphicsDeviceManager.GraphicsDevice.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
            ((IGame)Game).GraphicsDeviceManager.ApplyChanges();
            _context = CreateVrContext();
            
            base.Initialize();
        }

        private VrContext CreateVrContext()
        {
            if (!VrContext.CanCallNativeDll(out var error))
            {
                _logger.Error(error);
                return null;
            }

            var runtime = VrContext.RuntimeInstalled();
            var hmdConnected = VrContext.HmdConnected();
            _logger.Debug($"VR Runtime: {(runtime ? "yes" : "no")}");
            _logger.Debug($"VR HMD: {(hmdConnected ? "yes" : "no")}");

            if (!runtime)
            {
                _logger.Error("VR Runtime not installed, failed to create VR service...");
                return null;
            }

            if (!hmdConnected)
            {
                _logger.Error("No HMD connected, failed to create VR service...");
                return null;
            }

            var vrContext = VrContext.Get();

            _logger.Info("Initializing VR Runtime");
            try
            {
                vrContext.Initialize();
            }
            catch (SharpVRException ex)
            {
                if (ex.ErrorCode == 108)
                    _logger.Error("No HMD is connected and SteamVR failed to report it...");
                else
                    _logger.Error($"Initializing the runtime failed with error {ex.Message}");
                return null;
            }

            return vrContext;
        }
        

        public override void Update(GameTime gameTime)
        {
            _context.Update();

            var hmdPosition = _context.Hmd.LocalPosition;
            var hmdRotation = _context.Hmd.LocalRotation;
            
            //((IGame) Game).Player.Position = new Vector3(hmdPosition.X, 0f,  hmdPosition.Z);
            //((IGame) Game).Player.Rotation = hmdRotation;

            for (int i = 0; i < Cameras.Length; i++)
            {
                //Cameras[i].Position = ((IGame) Game).Player.Position;
                //Cameras[i].Rotation = ((IGame) Game).Player.Rotation;
            }

            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        public VrContext Context
        {
            get => _context;
        }
    }
}