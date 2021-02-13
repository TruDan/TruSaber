using Microsoft.Xna.Framework;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class Entity : GameComponent, ITransformable
    {
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
        
        
        public Entity(IGame game) : base(game.Game)
        {
            // Scale = Vector3.One / 2f;
            _dirty = true;
        }

        protected virtual void OnPositionChanged()
        {
            _dirty = true;
        }

        private bool _dirty;
        public override void Update(GameTime gameTime)
        {
            if(!Enabled) return;

            base.Update(gameTime);
            
            if (Velocity != Vector3.Zero)
            {
                Transform.Position += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}