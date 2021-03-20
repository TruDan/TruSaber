using System;
using BeatMapInfo;
using Microsoft.Xna.Framework;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class BaseTrackEntity : DrawableEntity
    {
        private readonly TrackObjectBase _trackObject;
        private readonly float           _bpm;
        private readonly float           _speed;
        private readonly float           _offset;
        private          bool            _spawned;

        public byte LineIndex { get; }
        public byte LineLayer { get; }

        //public TimeSpan DueTime { get; }

        public bool Spawned
        {
            get => _spawned;
            protected set => _spawned = value;
        }

        public bool HasSpawnedAtLeastOnce { get; protected set; }

        public BaseTrackEntity(IGame game, TrackObjectBase trackObject, float bpm, float speed, float offset) : base(game)
        {
            _trackObject = trackObject;
            _bpm = bpm;
            _speed = speed;
            _offset = offset;
            LineIndex = trackObject.LineIndex;
            LineLayer = trackObject.LineLayer;

            _initialPosition = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f, -(_offset + (_speed * (((float) trackObject.Time)) * (60f / _bpm))));
            
            Position = _initialPosition;
        }

        private readonly Vector3 _initialPosition;

        public void AddToSpace(Space space)
        {
            if (Spawned) return;

            Spawned = true;

            //  space.BoundingBoxUpdater.Finishing += ExpandBoundingBox;
            Position = _initialPosition;

            HasSpawnedAtLeastOnce = true;
            //  Visible = true;
            Enabled = true;

            space.Add(this);
        }

        public void RemoveFromSpace(Space space)
        {
            if (!Spawned) return;

            Spawned = false;
            //   Visible = false;
            Enabled = false;

            space.Remove(this);
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();

            Visible = MathF.Abs(Position.Z) < 30f;
        }
    }
}