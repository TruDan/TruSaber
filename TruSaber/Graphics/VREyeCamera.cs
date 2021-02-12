using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpVR;
using TruSaber.Abstractions;
using TruSaber.Utilities.Extensions;
using Valve.VR;

namespace TruSaber.Graphics
{
    public class VREyeCamera : GameComponent, ICamera
    {
        public Eye Eye { get; }
        
        
        public virtual Vector3 Scale
        {
            get => _scale;
            set
            {
                if(_scale == value) 
                    return;
                _scale = value;
                OnPositionChanged();
            }
        }
        public virtual Vector3 Position
        {
            get => _position;
            set
            {
                if(_position == value) 
                    return;
                _position = value;
                OnPositionChanged();
            }
        }

        public virtual Quaternion Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation == value)
                    return;
                _rotation = value;
                OnPositionChanged();
            }
        }
        
        public virtual Vector3 Forward    => _forward;
        public virtual Vector3 Up         => _up;
        public         Matrix  World      { get; private set; }
        public         Matrix  View       { get; private set; }
        public         Matrix  Projection { get; private set; }

        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 100.0f;

        
        private Vector3    _scale    = Vector3.One;
        private Vector3    _position = Vector3.Zero;
        private Vector3    _forward  = Vector3.Backward;
        private Vector3    _up       = Vector3.Up;
        private Quaternion _rotation = Quaternion.Identity;
        
        private GraphicsDevice _graphicsDevice;
        private IVrContext _vrContext;
        
        private Texture_t _vrTexture;
        private VRTextureBounds_t _vrTextureBounds;
        private RenderTarget2D _renderTarget;
        
        public VREyeCamera(IGame game, Eye eye) : base(game.Game)
        {
            Eye = eye;
        }

        protected virtual void OnPositionChanged()
        {
            _forward = Vector3.Transform(Vector3.Backward, _rotation);
            _up = Vector3.Transform(Vector3.Up, _rotation);

            World = Matrix.Identity
                    * Matrix.CreateScale(_scale)
                    * Matrix.CreateFromQuaternion(_rotation)
                    * Matrix.CreateTranslation(_position);
            UpdateViewMatrix();

        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            _vrContext = VrContext.Get();
            _graphicsDevice = Game.GraphicsDevice;
            _renderTarget = CreateRenderTarget();
            UpdateProjectionMatrix();
        }
        
        private RenderTarget2D CreateRenderTarget()
        {
            var eyeNo = (int) Eye;
            _vrContext.GetRenderTargetSize(out var width, out var height);
            
            var pp = _graphicsDevice.PresentationParameters;

            var renderTarget = new RenderTarget2D(_graphicsDevice, width, height, false, SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);

            _vrTexture = new Texture_t();

#if DIRECTX
            var info = typeof(RenderTarget2D).GetField("_msTexture", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var handle = info.GetValue(renderTarget) as SharpDX.Direct3D11.Texture2D;
            _vrTexture.handle = handle.NativePointer;
            _vrTexture.eType = ETextureType.DirectX;
            _vrTextureBounds.uMin = 0;
            _vrTextureBounds.uMax = 1;
            _vrTextureBounds.vMin = 0;
            _vrTextureBounds.vMax = 1;
#else
            var info = typeof(RenderTarget2D).GetField("glTexture", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var glTexture = (int)info.GetValue(renderTarget);
            _vrTexture.handle = new System.IntPtr(glTexture);
            _vrTexture.eType = ETextureType.OpenGL;
            _vrTextureBounds.uMin = 0;
            _vrTextureBounds.uMax = 1;
            _vrTextureBounds.vMin = 1;
            _vrTextureBounds.vMax = 0;
#endif
            _vrTexture.eColorSpace = EColorSpace.Gamma;

            return renderTarget;
        }

        private void UpdateViewMatrix()
        {
            View = Matrix.Invert(World) * (_vrContext.Hmd.GetPose() * _vrContext.GetEyeMatrix(Eye));
        }

        private void UpdateProjectionMatrix()
        {
            Projection = _vrContext.GetProjectionMatrix(Eye, NearPlane, FarPlane);
        }

        public override void Update(GameTime gameTime)
        {
            OnPositionChanged();
            base.Update(gameTime);
        }

        public void Draw(Action doDraw)
        {
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Black);
            doDraw();

            _vrContext.Submit(Eye, ref _vrTexture, ref _vrTextureBounds, EVRSubmitFlags.Submit_Default);
        }
    }
}