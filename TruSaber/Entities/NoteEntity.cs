using System;
using BeatMapInfo;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Materials;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.PositionUpdating;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;
using TruSaber.Utilities.Extensions;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TruSaber
{
    public class NoteEntity : DrawableEntity
    {
        private readonly Note         _note;
        private readonly float        _bpm;
        private readonly float        _speed;
        private readonly float        _offset;
        private          bool         _spawned;
        public           CutDirection CutDirection { get; set; }
        public           NoteType     Type         { get; set; }

        public byte LineIndex { get; }
        public byte LineLayer { get; }

        public TimeSpan DueTime { get; }

        public bool Spawned
        {
            get => _spawned;
            protected set => _spawned = value;
        }

        public bool HasSpawnedAtLeastOnce { get; protected set; }

        public Color Color { get; }

        public Box PhysicsEntity { get; private set; }

        public NoteEntity(IGame game, Note note, float bpm, float speed, float offset) : base(game)
        {
            _note = note;
            _bpm = bpm;
            _speed = speed;
            _offset = offset;
            CutDirection = note.CutDirection;
            Type = note.Type;
            LineIndex = note.LineIndex;
            LineLayer = note.LineLayer;
            DueTime = TimeSpan.FromSeconds((_offset + note.Time) * (60f / _bpm));

            Color = Type == NoteType.Bomb ? Color.DarkGray : (Type == NoteType.LeftNote ? Color.Red : Color.Blue);

//            PhysicsEntity = new TransformableEntity(Position.ToBEPU(), new BoxShape(1f, 1f, 1f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));

            _initialPosition = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f, -_speed - 10);
            Position = _initialPosition;
            Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, CutDirection.ToAngle());
            
            InitPhysics();
        }

        private Vector3 _initialPosition;

        private void InitPhysics()
        {
//            var pos = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f, -5f + (-_speed * ((_offset + (float) _note.Time)) * (60f / _bpm)));
            var scale = Scale;

            PhysicsEntity = new Box(_initialPosition.ToBEPU(), scale.X, scale.Y, scale.Z, 10000.0f);
            PhysicsEntity.Orientation = Rotation.ToBEPU();
            PhysicsEntity.PositionUpdateMode = PositionUpdateMode.Continuous;
            PhysicsEntity.LinearDamping = 0;
            PhysicsEntity.AngularDamping = 0;
            PhysicsEntity.Material = new Material(0f, 0f, 0f);
            PhysicsEntity.ActivityInformation.IsAlwaysActive = true;

            PhysicsEntity.IgnoreShapeChanges = true;
            PhysicsEntity.CollisionInformation.Shape.CollisionMargin = 0.1f;
            PhysicsEntity.Tag = this;
            PhysicsEntity.CollisionInformation.Events.DetectingInitialCollision += RemoveFriction;
            PhysicsEntity.BecomeKinematic();
        }


        public void AddToSpace(Space space)
        {
            if(Spawned) return;
            
            Spawned = true;
            space.Add(PhysicsEntity);
            space.BoundingBoxUpdater.Finishing += ExpandBoundingBox;
            Position = _initialPosition;
            PhysicsEntity.AngularVelocity = new BEPUutilities.Vector3();
            PhysicsEntity.LinearVelocity = new BEPUutilities.Vector3();
            HasSpawnedAtLeastOnce = true;
            Visible = true;
            Enabled = true;
            PhysicsEntity.ActivityInformation.Activate();
        }

        public void RemoveFromSpace(Space space)
        {
            if(!Spawned) return;
            
            Spawned = false;
            Visible = false;
            Enabled = false;
            space.Remove(PhysicsEntity);
            space.BoundingBoxUpdater.Finishing -= ExpandBoundingBox;
            PhysicsEntity.AngularVelocity = new BEPUutilities.Vector3();
            PhysicsEntity.LinearVelocity = new BEPUutilities.Vector3();
        }

        void RemoveFriction(EntityCollidable sender, BroadPhaseEntry other, NarrowPhasePair pair)
        {
            var collidablePair = pair as CollidablePairHandler;
            if (collidablePair != null)
            {
                //The default values for InteractionProperties is all zeroes- zero friction, zero bounciness.
                //That's exactly how we want the character to behave when hitting objects.
                collidablePair.UpdateMaterialProperties(new InteractionProperties());
            }
        }

        /// <summary>
        /// Cylinder shape used to compute the expanded bounding box of the character.
        /// </summary>
        void ExpandBoundingBox()
        {
            if (PhysicsEntity.ActivityInformation.IsActive)
            {
                //This runs after the bounding box updater is run, but before the broad phase.
                //Expanding the character's bounding box ensures that minor variations in velocity will not cause
                //any missed information.

                //TODO: seems a bit silly to do this work sequentially. Would be better if it could run in parallel in the proper location.

                var down        = PhysicsEntity.OrientationMatrix.Down;
                var boundingBox = PhysicsEntity.CollisionInformation.BoundingBox;
                //Expand the bounding box up and down using the step height.
                BEPUutilities.Vector3 expansion;
                BEPUutilities.Vector3.Multiply(ref down, 1f, out expansion);
                expansion.X = Math.Abs(expansion.X);
                expansion.Y = Math.Abs(expansion.Y);
                expansion.Z = Math.Abs(expansion.Z);

                //When the character climbs a step, it teleports horizontally a little to gain support. Expand the bounding box to accommodate the margin.
                //Compute the expansion caused by the extra radius along each axis.
                //There's a few ways to go about doing this.

                //The following is heavily cooked, but it is based on the angle between the vertical axis and a particular axis.
                //Given that, the amount of the radial expansion required along that axis can be computed.
                //The dot product would provide the cos(angle) between the vertical axis and a chosen axis.
                //Equivalently, it is how much expansion would be along that axis, if the vertical axis was the axis of expansion.
                //However, it's not. The dot product actually gives us the expansion along an axis perpendicular to the chosen axis, pointing away from the character's vertical axis.

                //What we need is actually given by the sin(angle), which is given by ||verticalAxis x testAxis||.
                //The sin(angle) is the projected length of the verticalAxis (not the expansion!) on the axis perpendicular to the testAxis pointing away from the character's vertical axis.
                //That projected length, however is equal to the expansion along the test axis, which is exactly what we want.
                //To show this, try setting up the triangles at the corner of a cylinder with the world axes and cylinder axes.

                //Since the test axes we're using are all standard directions ({0,0,1}, {0,1,0}, and {0,0,1}), most of the cross product logic simplifies out, and we are left with:
                var horizontalExpansionAmount = PhysicsEntity.CollisionInformation.Shape.CollisionMargin * 1.1f;
                BEPUutilities.Vector3 squaredDown;
                squaredDown.X = down.X * down.X;
                squaredDown.Y = down.Y * down.Y;
                squaredDown.Z = down.Z * down.Z;
                expansion.X += horizontalExpansionAmount * (float) Math.Sqrt(squaredDown.Y + squaredDown.Z);
                expansion.Y += horizontalExpansionAmount * (float) Math.Sqrt(squaredDown.X + squaredDown.Z);
                expansion.Z += horizontalExpansionAmount * (float) Math.Sqrt(squaredDown.X + squaredDown.Y);

                BEPUutilities.Vector3.Add(ref expansion, ref boundingBox.Max, out boundingBox.Max);
                BEPUutilities.Vector3.Subtract(ref boundingBox.Min, ref expansion, out boundingBox.Min);

                PhysicsEntity.CollisionInformation.BoundingBox = boundingBox;
            }
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
            if (PhysicsEntity == null)
                return;
            
            if (PhysicsEntity.Position != Position.ToBEPU())
                PhysicsEntity.Position = Position.ToBEPU();

            if (PhysicsEntity.Orientation != Rotation.ToBEPU())
                PhysicsEntity.Orientation = Rotation.ToBEPU();

            if (PhysicsEntity.LinearVelocity != Velocity.ToBEPU())
                PhysicsEntity.LinearVelocity = Velocity.ToBEPU();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            if (Type == NoteType.LeftNote)
            {
                if (CutDirection == CutDirection.Any)
                {
                    Model = Game.Content.Load<Model>("Models/Block_Red_Omni");
                }
                else
                {
                    Model = Game.Content.Load<Model>("Models/Block_Red");
                }
            }
            else if (Type == NoteType.RightNote)
            {
                if (CutDirection == CutDirection.Any)
                {
                    Model = Game.Content.Load<Model>("Models/Block_Blue_Omni");
                }
                else
                {
                    Model = Game.Content.Load<Model>("Models/Block_Blue");
                }
            }
            else if (Type == NoteType.Bomb)
            {
                Model = Game.Content.Load<Model>("Models/Bomb");
            }

            foreach (var mesh in Model.Meshes)
            {
                foreach (var effect in mesh.Effects)
                {
                    if (effect is BasicEffect basicEffect)
                    {
                        basicEffect.TextureEnabled = true;
                        basicEffect.EnableDefaultLighting();
                        basicEffect.SpecularColor = Color.ToVector3();
                        basicEffect.SpecularPower = 0.1f;
                        //basicEffect.DiffuseColor = Color.ToVector3();
                        basicEffect.Alpha = 1.0f;
                        //basicEffect.VertexColorEnabled = true;
                    }
                }
            }
        }

    }
}