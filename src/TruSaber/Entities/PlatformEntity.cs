using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class PlatformEntity : DrawableEntity
    {
        public PlatformEntity(IGame game) : base(game)
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Model = Game.Content.Load<Model>("Models/PlatformV2/PlatformV2");
            Position = Vector3.Zero;
            Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 180f.ToRadians());
        }
    }
}