using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RocketUI;
using TruSaber.Abstractions;

namespace TruSaber.Graphics
{
    public enum ProjectionType
    {
        Perspective,
        Orthographic
    }
    
    public class Camera : GameComponent, ICamera, ITransformable
    {
        private readonly IGame       _game;
        private          Rectangle   _bounds       = Rectangle.Empty;
        private          float       _nearDistance = 0.1f;
        private          float       _farDistance         = 1000.0f;
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

        public float NearDistance
        {
            get => _nearDistance;
            set
            {
                _nearDistance = value;
                UpdateProjection();
            }
        }

        public float FarDistance
        {
            get => _farDistance;
            set
            {
                _farDistance = value;
                UpdateProjection();
            }
        }

        public Rectangle Bounds
        {
            get => _bounds;
            set
            {
                _bounds = value;
                UpdateProjection();
            }
        }

        public ProjectionType ProjectionType { get; set; } = ProjectionType.Perspective;
        
        public RenderTarget2D RenderTarget { get; set; }

        public Camera(IGame game) : base(game.Game)
        {
            _game = game;
            UpdateView();
            UpdateProjection();

            _game.GraphicsDeviceManager.DeviceCreated += (sender, args) => UpdateProjection();
            _game.GraphicsDeviceManager.DeviceReset += (sender,   args) => UpdateProjection();
            _game.Window.ClientSizeChanged += (sender,            args) => UpdateProjection();
            Transform.Changed += (sender,                 args) => UpdateView();
        }
        
        private void UpdateView()
        {
            var forward = Vector3.Transform(Vector3.Backward, Rotation);
            var up      = Vector3.Transform(Vector3.Up, Rotation);
            View = Matrix.CreateLookAt(Position, Position - forward, up);
        }

        private void UpdateProjection()
        {
            if (Bounds == Rectangle.Empty)
            {
                Bounds = _game.Window.ClientBounds;
            }
            var w = Bounds.Width;
            var h = Bounds.Height;

            if (ProjectionType == ProjectionType.Perspective)
            {
                var aspect = (float) w / (float) h;
                Projection = Matrix.CreatePerspectiveFieldOfView(60.0f.ToRadians(), aspect, NearDistance, FarDistance);
            }
            else
            {
                Projection = Matrix.CreateOrthographic(w, h, NearDistance, FarDistance);
            }
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Right))
            {
            }
        }

        public void Draw(Action doDraw)
        {
            var g = _game.Game.GraphicsDevice;

            g.SetRenderTarget(RenderTarget);
            g.Clear(Color.Black);

            doDraw();
        }
    }
}