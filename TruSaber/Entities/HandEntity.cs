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
using Vector3 = BEPUutilities.Vector3;

namespace TruSaber
{
    public class HandEntity : DrawableEntity
    {
        private Microsoft.Xna.Framework.Vector3 _angularVelocity;
        public  Player                          Player { get; }
        public  Hand                            Hand   { get; }

        public  Quaternion ControllerOffset { get; set; }
        private IVrContext  VrContext        { get; }

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
            VrContext = game.ServiceProvider.GetRequiredService<IVrContext>();
            ControllerOffset = Quaternion.CreateFromYawPitchRoll(0f, -90f.ToRadians(), 0f);
            // PhysicsEntity = new TransformableEntity(Position.ToBEPU(),
            //     new BoxShape(0.04f, 0.75f, 0.04f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));
            
            InitPhysics();
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            Position = Hand == Hand.Left ? Microsoft.Xna.Framework.Vector3.Left : Microsoft.Xna.Framework.Vector3.Right;
            Rotation = Quaternion.Identity;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            if(!Enabled) return;

            if (Hand == Hand.Left)
            {
                if (VrContext.LeftController != null)
                {
                    Position = Player.Position + VrContext.LeftController.LocalPosition;
                    Rotation = Quaternion.Multiply(VrContext.LeftController.LocalRotation, ControllerOffset);
                    //PhysicsEntity.AngularVelocity = VrContext.LeftController.GetAngularVelocity().ToBEPU();
                }
            }
            else if (Hand == Hand.Right)
            {
                if (VrContext.RightController != null)
                {
                    Position = Player.Position + VrContext.RightController.LocalPosition;
                    Rotation = Quaternion.Multiply(VrContext.RightController.LocalRotation, ControllerOffset);
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
        }

        public BoundingBox GetBounds()
        {
            return new BoundingBox(new Vector3(-0.02f, 0, -0.02f), new Vector3(0.02f, 0.75f, 0.02f));
        }
    }
}