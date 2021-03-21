using BeatMapInfo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class WallEntity : BaseTrackEntity
    {
        public Obstacle Obstacle { get; }

        public WallEntity(IGame game, Obstacle obstacle, float bpm, float speed, float offset) : base(game, obstacle,
            bpm, speed, offset)
        {
            float height = 2f;
            float y      = 0f;
            float width  = obstacle.Width;
            if (obstacle.Type == ObstacleType.CrouchWall)
            {
                height = 1f;
                LineLayer = 2;
            }

            // Mapping Extensions Support
            int value                 = (int) obstacle.Type;
                float multiplier = 1f;
            if (obstacle.Width >= 1000 || (value >= 1000 && value <= 4000) || (value >= 4001 && value <= 4005000))
            {
                var preciseHeighStartMode = (value >= 4001 && value <= 4100000);

                if (preciseHeighStartMode)
                {
                    value -= 4001;
                    height = value / 1000;
                    y = value % 1000;
                }
                else
                {
                    height = value - 1000;
                }

                if (width >= 1000 || preciseHeighStartMode)
                {
                    width = (width - 1000) / 1000;
                }

                if (value >= 1000)
                {
                    multiplier = height / 1000f;
                }
            }

            Scale = new Vector3(width * 0.98f, height * multiplier, (float) (obstacle.Duration * speed * (60f / bpm)));
            Position = new Vector3(LineIndex - 1.5f, y, (-speed * (((float) obstacle.Time)) * (60f / bpm)));

            if (obstacle.CustomData.Scale.HasValue)
            {
                var s =obstacle.CustomData.Scale.Value;
                Scale = new Vector3(s.X, s.Y, s.Z);
            }
        }


        protected override void LoadContent()
        {
            Model = TruSaberGame.Instance.Game.Content.Load<Model>("Models/Wall");
            base.LoadContent();
        }
    }
}