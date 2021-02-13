using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using TruSaber.Abstractions;

namespace TruSaber.Graphics
{
    public class PerspectiveCamera : GameComponent, ICamera, ITransformable
    {
        private readonly IGame       _game;
        public           Transform3D Transform { get; } = new Transform3D();

        public Vector3 Scale
        {
            get => Transform.Scale;
            set => Transform.Scale = value;
        }

        public Vector3 Position
        {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        public Quaternion Rotation
        {
            get => Transform.Rotation;
            set => Transform.Rotation = value;
        }

        public Matrix World
        {
            get => Transform.World;
        }

        public Matrix View       { get; private set; }
        public Matrix Projection { get; private set; }

        public PerspectiveCamera(IGame game) : base(game.Game)
        {
            _game = game;
            UpdateView();
            UpdateProjection();

            _game.GraphicsDeviceManager.DeviceCreated += (sender, args) => UpdateProjection();
            _game.GraphicsDeviceManager.DeviceReset += (sender,   args) => UpdateProjection();
            _game.Window.ClientSizeChanged += (sender,            args) => UpdateProjection();
            Transform.PositionChanged += (sender,                 args) => UpdateView();
        }
        
        private void UpdateView()
        {
            var forward = Vector3.Transform(Vector3.Backward, Rotation);
            var up      = Vector3.Transform(Vector3.Up, Rotation);
            View = Matrix.CreateLookAt(Position, Position - forward, up);
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