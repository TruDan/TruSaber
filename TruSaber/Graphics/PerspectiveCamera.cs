using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using TruSaber.Abstractions;

namespace TruSaber.Graphics
{
    public class PerspectiveCamera : ICamera
    {
        private readonly IGame _game;
        private Vector3 _position = Vector3.Zero;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool Enabled { get; }
        public int UpdateOrder { get; }

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value; 
                UpdateView();
            }
        }

        public Quaternion Rotation { get; set; }
        public Vector3 Forward { get; private set; } = Vector3.Backward;
        public Vector3 Up { get; private set; } = Vector3.Up;
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        public PerspectiveCamera(IGame game)
        {
            _game = game;
            UpdateView();
            UpdateProjection();

            _game.GraphicsDeviceManager.DeviceCreated += (sender, args) => UpdateProjection();
            _game.GraphicsDeviceManager.DeviceReset += (sender, args) => UpdateProjection();
        }
        
        private void UpdateView()
        {
            View = Matrix.CreateLookAt(Position, Position - Forward, Up);
        }

        private void UpdateProjection()
        {
            var w = _game.Window.ClientBounds.Width;
            var h = _game.Window.ClientBounds.Height;

            var aspect = (float) w / (float) h;
            Projection = Matrix.CreatePerspectiveFieldOfView(60.0f.ToRadians(), aspect, 0.1f, 100f);
        }
        
        public void Update(GameTime gameTime)
        {
            var keyboard = KeyboardExtended.GetState();
            if (keyboard.IsKeyDown(Keys.Right))
            {
            }
        }
        
        public void Draw(Action doDraw)
        {
            var g = _game.Game.GraphicsDevice;
            
            g.SetRenderTarget(null);
            g.Clear(Color.Black);
            doDraw();
        }
    }
}