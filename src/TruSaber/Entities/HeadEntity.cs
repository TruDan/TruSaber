using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using SharpVR;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class HeadEntity : Entity
    {
        private Vector3 _angularVelocity;
        
        public  Player  Player { get; }
        
        public Vector3 AngularVelocity
        {
            get => _angularVelocity;
            set
            {
                if(_angularVelocity == value)
                    return;
                
                _angularVelocity = value;
                OnPositionChanged();
            }
        }
        
        private IVrContext VrContext { get; }
        
        public HeadEntity(IGame game, Player player) : base(game)
        {
            Player = player;
            Transform.ParentTransform = player.Transform;
            VrContext = game.ServiceProvider.GetRequiredService<IVrContext>();
            Scale = Vector3.One;
        }

        public override void Initialize()
        {
            Position = Vector3.Up;
            Rotation = Quaternion.Identity;
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(!(Initialized && Enabled)) return;

            Transform.RelativePosition = VrContext.Hmd.LocalPosition;
            Transform.RelativeRotation = VrContext.Hmd.LocalRotation;
            
            base.Update(gameTime);
        }
    }
}