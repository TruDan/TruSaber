using System;
using BeatMapInfo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;
using RocketUI.Utilities.Helpers;
using TruSaber.Abstractions;
using TruSaber.Graphics;
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

        //public TimeSpan DueTime { get; }

        public bool Spawned
        {
            get => _spawned;
            protected set => _spawned = value;
        }

        public bool HasSpawnedAtLeastOnce { get; protected set; }

        public Model ArrowModel { get; private set; }
        public Color Color      { get; }

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
            //  DueTime = TimeSpan.FromSeconds((_offset + note.Time) * (60f / _bpm));

            Color = note.Type == NoteType.Bomb ? Color.DarkGray : (note.Type == NoteType.LeftNote ? Color.Red : Color.Blue);

//            PhysicsEntity = new TransformableEntity(Position.ToBEPU(), new BoxShape(1f, 1f, 1f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));

            _initialPosition = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f,
                (-_speed * (((float) _note.Time)) * (60f / _bpm)));

            BoundingBoxSize = Vector3.One;
            BoundingBoxOrigin = -Vector3.One / 2f;
            
            base.Position = _initialPosition;
            base.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, CutDirection.ToAngle());

            InitPhysics();
        }

        private Vector3 _initialPosition;

        private void InitPhysics()
        {
//            var pos = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f, -5f + (-_speed * ((_offset + (float) _note.Time)) * (60f / _bpm)));
            var scale = Scale;
        }


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

        private void LoadBlockModel()
        {
            Model  = Game.Content.Load<Model>("Models/Default Base");
            ArrowModel = Game.Content.Load<Model>("Models/Default Arrows");
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (Type == NoteType.LeftNote || Type == NoteType.RightNote)
            {
                LoadBlockModel();
            }
            else if (Type == NoteType.Bomb)
            {
                Model = Game.Content.Load<Model>("Models/Bomb");
            }

            if (ArrowModel != null)
            {
                var arrowToHideName = CutDirection == CutDirection.Any ? "Arrow" : "Dot";
                foreach (var mesh in ArrowModel.Meshes)
                foreach (var effect in mesh.Effects)
                {
                    if (effect is BasicEffect basicEffect)
                    {
                        if (arrowToHideName == mesh.Name)
                        {
                            basicEffect.Alpha = 0.0f;
                        }
                        else
                        {
                            basicEffect.Alpha = 1.0f;
                        }

                        basicEffect.LightingEnabled = true;
                        basicEffect.EnableDefaultLighting();
                    }
                }
            }


        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var effect in mesh.Effects)
                {
                    if (effect is BasicEffect basicEffect)
                    {
                        basicEffect.EnableDefaultLighting();
                        basicEffect.DiffuseColor = Color.ToVector3();
                    }
                }
            }
            base.Draw(gameTime);
            //
            // foreach (var mesh in Model.Meshes)
            // {
            //     foreach (var effect in mesh.Effects)
            //     {
            //         if (effect is BasicEffect basicEffect)
            //         {
            //             basicEffect.VertexColorEnabled = true;
            //             basicEffect.EnableDefaultLighting();
            //         }
            //     }
            // }
            using (GraphicsContext.CreateContext(
                GraphicsDevice, BlendState.AlphaBlend, DepthStencilState.Default, RasterizerState.CullCounterClockwise))
            {
                var cam = ((IGame) Game).Camera;
                //   var cam = (Game as IGame).Camera;
                ArrowModel?.DrawModelWithExclusions(World, cam.View, cam.Projection, CutDirection == CutDirection.Any ? "Arrow" : "Dot");
            }

            if(false)
                BoundingBox.Draw(GraphicsDevice, Color);
        }
    }
}