using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TruSaber.Models;

namespace TruSaber
{
    public class Space
    {
        private List<Entity> _entities = new List<Entity>();
        private object       _lock     = new object();

        private float _frameAccumulator = 0f;
        private float TargetTime        = 1f / 60f;

        private BeatLevel _level;

        public Space(BeatLevel level)
        {
            _level = level;
        }

        public void Start(TimeSpan offset)
        {
            _frameAccumulator = -(float) offset.TotalSeconds;
        }

        public void Update(GameTime gameTime)
        {
            var frameTime = (float) gameTime.ElapsedGameTime.TotalSeconds; // / 50;
            _frameAccumulator += frameTime;

            Entity[] entities;
            lock (_lock)
            {
                entities = _entities.ToArray();
            }

            //var targetTime = (float)1f / ((float)_level.MapInfo.BeatsPerMinute / 60f);

            foreach (var entity in entities)
            {
                var velocity = entity.Velocity;
                if (MathF.Abs(velocity.LengthSquared()) > 0f)
                {
                    entity.Position += (velocity * frameTime);
                }
            }

            _frameAccumulator -= TargetTime;
        }

        public void Add(Entity entity)
        {
            lock (_lock)
            {
                _entities.Add(entity);
            }
        }

        public void Remove(Entity entity)
        {
            lock (_lock)
            {
                _entities.Remove(entity);
            }
        }
    }
}