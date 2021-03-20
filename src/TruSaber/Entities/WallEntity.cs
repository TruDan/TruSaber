using BeatMapInfo;
using Microsoft.Xna.Framework;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class WallEntity : DrawableEntity
    {
        private readonly IGame    _game;
        private readonly float    _bpm;
        private readonly float    _speed;
        private readonly float    _offset;
        public           Obstacle Obstacle { get; }
        
        public byte LineIndex { get; }
        public byte LineLayer { get; }
        
        public WallEntity(IGame game, Obstacle obstacle, float bpm, float speed, float offset) : base(game)
        {
            _game = game;
            _bpm = bpm;
            _speed = speed;
            _offset = offset;
            Obstacle = obstacle;
            LineIndex = obstacle.LineIndex;
            
            if (obstacle.Type == 0)
            {
                // Full height wall
                Scale = new Vector3(obstacle.Width, 2f, (float) (obstacle.Duration * speed * (60f/bpm)));
                Position = new Vector3(LineIndex - 1.5f, 0f, (-speed * (((float) obstacle.Time)) * (60f / bpm)));
            }
            else if (obstacle.Type == 1)
            {
                Scale = new Vector3(obstacle.Width, 1f, (float) (obstacle.Duration * speed * (60f/bpm)));
                Position = new Vector3(LineIndex - 1.5f, 1f, (-speed * (((float) obstacle.Time)) * (60f / bpm)));
            }
        }
        
        
    }
}