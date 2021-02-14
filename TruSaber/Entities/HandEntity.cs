using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpVR;
using TruSaber.Abstractions;
using TruSaber.Utilities.Extensions;
using BoundingBox = BEPUutilities.BoundingBox;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace TruSaber
{
    public class HandEntity : DrawableEntity
    {
        private Microsoft.Xna.Framework.Vector3 _angularVelocity;
        public  Player                          Player { get; }
        public  Hand                            Hand   { get; }

        public Color Color { get; set; }
        
        public  Quaternion ControllerOffset { get; set; }
        private IVrContext  VrContext        { get; }
        
        public Ray Ray { get; private set; }

        public Microsoft.Xna.Framework.Vector3 AngularVelocity
        {
            get => _angularVelocity;
            set
            {
                if(_angularVelocity == value)
                    return;
                
                _angularVelocity = value;
                OnPositionChanged();
            }
        }

        public HandEntity(IGame game, Player player, Hand hand) : base(game)
        {
            Player = player;
            Hand = hand;
            Color = hand == Hand.Left ? Color.Red : Color.Blue;
            VrContext = game.ServiceProvider.GetRequiredService<IVrContext>();
            ControllerOffset = Quaternion.CreateFromYawPitchRoll(0f, -90f.ToRadians(), 0f);
            // PhysicsEntity = new TransformableEntity(Position.ToBEPU(),
            //     new BoxShape(0.04f, 0.75f, 0.04f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));
            
            InitPhysics();
            Ray = new Ray();
        }

        private VertexPositionColor[] _rayVerticies;
        private VertexBuffer          _vertexBuffer;
        private BasicEffect           _effect;

        /// <inheritdoc />
        public override void Initialize()
        {
            Position = Hand == Hand.Left ? Microsoft.Xna.Framework.Vector3.Left : Microsoft.Xna.Framework.Vector3.Right;
            Rotation = Quaternion.Identity;
            _rayVerticies = new VertexPositionColor[2];
            _vertexBuffer = new VertexBuffer(Game.GraphicsDevice, VertexPositionColor.VertexDeclaration, 2,
                BufferUsage.WriteOnly);
            _effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            if(!Enabled) return;

            if (Hand == Hand.Left)
            {
                if (VrContext.LeftController != null)
                {
                    Transform.Position = Player.Position + VrContext.LeftController.LocalPosition;
                    Transform.Rotation = Quaternion.Multiply(VrContext.LeftController.LocalRotation, ControllerOffset);
                    //PhysicsEntity.AngularVelocity = VrContext.LeftController.GetAngularVelocity().ToBEPU();
                }
            }
            else if (Hand == Hand.Right)
            {
                if (VrContext.RightController != null)
                {
                    Transform.Position = Player.Position + VrContext.RightController.LocalPosition;
                    Transform.Rotation = Quaternion.Multiply(VrContext.RightController.LocalRotation, ControllerOffset);
                }
            }

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Model = Game.Content.Load<Model>("Models/Saber");
            
        }

        private void InitPhysics()
        {
            
        }

        
        public void AddToSpace(Space space)
        {
            space.Add(this);
        }

        public void RemoveFromSpace(Space space)
        {
            space.Remove(this);
        }

        /// <summary>
        /// Cylinder shape used to compute the expanded bounding box of the character.
        /// </summary>

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
            if (_rayVerticies != null)
            {

                var nearPoint = Vector3.Transform(Vector3.Zero, World);
                var farPoint  = Vector3.Transform(Vector3.Up, World);
                var direction = farPoint - nearPoint;

                //var direction = Vector3.Transform(Vector3.Up, World);
                direction.Normalize();
                
                Ray = new Ray(Position, direction);
                _rayVerticies[0] = new VertexPositionColor(Position, Color);
                _rayVerticies[1] = new VertexPositionColor(Position + (direction * 1000.0f), Color);
                _vertexBuffer.SetData(_rayVerticies);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var cam = (Game as IGame).Camera;
            
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            _effect.VertexColorEnabled = true;
            
            _effect.World = Matrix.Identity;
            _effect.View = cam.View;
            _effect.Projection = cam.Projection;
            
           // _effect.EnableDefaultLighting();
            
            foreach(EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 1);
            }
        }
    }
}