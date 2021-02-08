using Microsoft.Xna.Framework;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class Entity : GameComponent
    {
        private Vector3    _scale    = Vector3.One;
        private Vector3    _position = Vector3.Zero;
        private Vector3    _forward  = Vector3.Forward;
        private Vector3    _up       = Vector3.Up;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3    _velocity = Vector3.Zero;

        public virtual Matrix World { get; protected set; }
        
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

        public virtual Vector3 Forward => _forward;
        public virtual Vector3 Up => _up;

        public Entity(IGame game) : base(game.Game)
        {
            Position = Vector3.Zero;
            // Scale = Vector3.One / 2f;
            _dirty = true;
        }

        protected virtual void OnPositionChanged()
        {
            _forward = Vector3.Transform(Vector3.Forward, _rotation);
            _up = Vector3.Transform(Vector3.Up, _rotation);

            World = Matrix.Identity
                    * Matrix.CreateScale(_scale)
                    * Matrix.CreateFromQuaternion(_rotation)
                    * Matrix.CreateTranslation(_position);

        }

        private bool _dirty;
        public override void Update(GameTime gameTime)
        {
            if(!Enabled) return;

            if (_dirty)
            {
                _dirty = false;
                OnPositionChanged();
            }

            base.Update(gameTime);
            
            if (Velocity != Vector3.Zero)
            {
                Position += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}