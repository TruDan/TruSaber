using BeatMapInfo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class WallEntity : BaseTrackEntity
    {
        public           Obstacle Obstacle { get; }
        
        public WallEntity(IGame game, Obstacle obstacle, float bpm, float speed, float offset) : base(game, obstacle, bpm, speed, offset)
        {
            if (obstacle.Type == ObstacleType.CrouchWall)
            {
                Scale = new Vector3(obstacle.Width, 1f, (float) (obstacle.Duration * speed * (60f/bpm)));
                Position = new Vector3(LineIndex - 1.5f, 1f, (-speed * (((float) obstacle.Time)) * (60f / bpm)));
            }
            else 
            {
                // Full height wall
                Scale = new Vector3(obstacle.Width, 2f, (float) (obstacle.Duration * speed * (60f/bpm)));
                Position = new Vector3(LineIndex - 1.5f, 0f, (-speed * (((float) obstacle.Time)) * (60f / bpm)));
            }
        }


        protected override void LoadContent()
        {
            Model = TruSaberGame.Instance.Game.Content.Load<Model>("Models/Wall");
            base.LoadContent();
        }
    }
}