using System;
using BeatMapInfo;
using Microsoft.Xna.Framework;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class BaseTrackEntity : DrawableEntity
    {
        private readonly TrackObjectBase _trackObject;
        private readonly float           _positioningMultiplier;
        private          bool            _spawned;

        public long LineIndex { get; protected set; }
        public long LineLayer { get; protected set; }

        //public TimeSpan DueTime { get; }

        public bool Spawned
        {
            get => _spawned;
            protected set => _spawned = value;
        }

        public bool HasSpawnedAtLeastOnce { get; protected set; }

        public BaseTrackEntity(IGame game, TrackObjectBase trackObject, float positioningMultiplier) : base(game)
        {
            _trackObject = trackObject;
            _positioningMultiplier = positioningMultiplier;
            LineIndex = trackObject.LineIndex;
            LineLayer = trackObject.LineLayer;

            _initialPosition = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f, -((float) trackObject.Time * _positioningMultiplier));
            
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

//            Visible = BoundingBox.Min.Z < 5f || Position.Z < 5f;
            Visible = Math.Abs(Position.Z) < 30f;
        }
    }
}