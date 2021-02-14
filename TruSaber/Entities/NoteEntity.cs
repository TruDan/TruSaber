using System;
using BeatMapInfo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
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

        public Color Color { get; }

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

            Color = Type == NoteType.Bomb ? Color.DarkGray : (Type == NoteType.LeftNote ? Color.Red : Color.Blue);

//            PhysicsEntity = new TransformableEntity(Position.ToBEPU(), new BoxShape(1f, 1f, 1f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));

            _initialPosition = new Vector3(LineIndex - 1.5f, LineLayer + 0.5f, (-_speed * (((float) _note.Time)) * (60f / _bpm)));

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
            if(Spawned) return;
            
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
            if(!Spawned) return;
            
            Spawned = false;
         //   Visible = false;
            Enabled = false;

            space.Remove(this);
        }

        /// <summary>
        /// Cylinder shape used to compute the expanded bounding box of the character.
        /// </summary>
        void ExpandBoundingBox()
        {
           
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
                
            Visible = MathF.Abs(Position.Z) < 30f;
            
            var half3 = (Vector3.One * 1.0f) * 0.5f;
            BoundingBox = new BoundingBox(Position - half3, Position + half3);
           // if (Position)
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            //PhysicsEntity.CollisionInformation.UpdateBoundingBox((float) gameTime.ElapsedGameTime.TotalSeconds);
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
                        basicEffect.AmbientLightColor = Color.White.ToVector3() * 0.2f;

                        basicEffect.DirectionalLight0.Direction = new Vector3(-1f, 1f, -1f);
                        basicEffect.DirectionalLight0.DiffuseColor = Color.White.ToVector3() * 0.8f;
                        basicEffect.DirectionalLight0.SpecularColor = Color.Red.ToVector3();
                        basicEffect.DirectionalLight0.Enabled = true;
                        
                        basicEffect.DirectionalLight1.Direction = new Vector3(1f, 1f, -1f);
                        basicEffect.DirectionalLight1.DiffuseColor = Color.White.ToVector3() * 0.8f;
                        basicEffect.DirectionalLight1.SpecularColor = Color.Blue.ToVector3();
                        basicEffect.DirectionalLight1.Enabled = true;
                        
                    /*    basicEffect.DirectionalLight1.Direction = new Vector3(0f, 0f, 1f);
                        basicEffect.DirectionalLight1.DiffuseColor = Color.Red.ToVector3();
                        basicEffect.DirectionalLight1.SpecularColor = Color.Blue.ToVector3();
                        basicEffect.DirectionalLight1.Enabled = true;*/
                        
                        //basicEffect.SpecularColor = Color.ToVector3();
                        //basicEffect.SpecularPower = 0.1f;
                        //basicEffect.DiffuseColor = Color.ToVector3();
                        basicEffect.Alpha = 1.0f;
                       // basicEffect.VertexColorEnabled = true;
                        basicEffect.DiffuseColor = Color.ToVector3();
                    }
                }
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            BoundingBox.Draw(GraphicsDevice, Color);
        }
    }
}