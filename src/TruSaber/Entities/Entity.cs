using Microsoft.Xna.Framework;
using RocketUI;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class Entity : GameComponent, ITransformable
    {
        public BoundingBox BoundingBox       { get; protected set; }
        public Vector3     BoundingBoxSize   { get; protected set; } = Vector3.One;
        public Vector3     BoundingBoxOrigin { get; protected set; } = Vector3.One / 2f;
        
        private Vector3    _velocity = Vector3.Zero;
        public virtual Vector3 Velocity
        {
            get => _velocity;
            set
            {
                if (_velocity == value)
                    return;
                _velocity = value;
                OnPositionChanged();
            }
        }
        public Transform3D Transform { get; } = new Transform3D();
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

        public bool Initialized => _initialized;


        public Entity(IGame game) : base(game.Game)
        {
            Position = Vector3.Zero;
            // Scale = Vector3.One / 2f;
            Transform.Changed += (sender, args) => OnPositionChanged();
        }

        public override void Initialize()
        {
            base.Initialize();
            _initialized = true;
        }

        protected virtual void OnPositionChanged()
        {
            var min = Vector3.Transform(BoundingBoxOrigin, World);
            var max = Vector3.Transform(BoundingBoxOrigin + BoundingBoxSize, World);
            BoundingBox = new BoundingBox(min, max);
        }

        private bool _initialized;

        public override void Update(GameTime gameTime)
        {
            if(!Enabled) return;

            base.Update(gameTime);
        }
    }
}