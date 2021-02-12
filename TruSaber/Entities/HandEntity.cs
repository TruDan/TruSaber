using System;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Character;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.DataStructures;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Materials;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.PositionUpdating;
using BEPUutilities;
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

        public Cylinder PhysicsEntity { get; private set; }

        public HandEntity(IGame game, Player player, Hand hand) : base(game)
        {
            Player = player;
            Hand = hand;
            VrContext = game.ServiceProvider.GetRequiredService<IVrContext>();
            ControllerOffset = Quaternion.CreateFromYawPitchRoll(0f, -90f.ToRadians(), 0f);
            // PhysicsEntity = new TransformableEntity(Position.ToBEPU(),
            //     new BoxShape(0.04f, 0.75f, 0.04f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));
            Position = hand == Hand.Left ? Microsoft.Xna.Framework.Vector3.Left : Microsoft.Xna.Framework.Vector3.Right;
            Rotation = Quaternion.Identity;
            InitPhysics();
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
                    PhysicsEntity.AngularVelocity = VrContext.LeftController.GetAngularVelocity().ToBEPU();
                    PhysicsEntity.LinearVelocity = VrContext.LeftController.GetVelocity().ToBEPU();
                    //PhysicsEntity.AngularVelocity = VrContext.LeftController.GetAngularVelocity().ToBEPU();
                }
            }
            else if (Hand == Hand.Right)
            {
                if (VrContext.RightController != null)
                {
                    Position = Player.Position + VrContext.RightController.LocalPosition;
                    Rotation = Quaternion.Multiply(VrContext.RightController.LocalRotation, ControllerOffset);
                   // PhysicsEntity.AngularVelocity = VrContext.RightController.GetAngularVelocity().ToBEPU();
                   PhysicsEntity.AngularVelocity = VrContext.RightController.GetAngularVelocity().ToBEPU();
                   PhysicsEntity.LinearVelocity = VrContext.RightController.GetVelocity().ToBEPU();
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
            PhysicsEntity = new Cylinder(Hand == Hand.Left ? Vector3.Left : Vector3.Right, 0.8f, 0.15f, 1000.0f);
            PhysicsEntity.Orientation = ControllerOffset.ToBEPU();
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
            space.Add(PhysicsEntity);

            space.BoundingBoxUpdater.Finishing += ExpandBoundingBox;
            
            PhysicsEntity.AngularVelocity = new Vector3();
            PhysicsEntity.LinearVelocity = new Vector3();
            PhysicsEntity.ActivityInformation.Activate();
        }

        public void RemoveFromSpace(Space space)
        {
            space.Remove(PhysicsEntity);
            space.BoundingBoxUpdater.Finishing -= ExpandBoundingBox;
            PhysicsEntity.AngularVelocity = new Vector3();
            PhysicsEntity.LinearVelocity = new Vector3();
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

                var down = PhysicsEntity.OrientationMatrix.Down;
                var boundingBox = PhysicsEntity.CollisionInformation.BoundingBox;
                //Expand the bounding box up and down using the step height.
                Vector3 expansion;
                Vector3.Multiply(ref down, 1f, out expansion);
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
                Vector3 squaredDown;
                squaredDown.X = down.X * down.X;
                squaredDown.Y = down.Y * down.Y;
                squaredDown.Z = down.Z * down.Z;
                expansion.X += horizontalExpansionAmount * (float)Math.Sqrt(squaredDown.Y + squaredDown.Z);
                expansion.Y += horizontalExpansionAmount * (float)Math.Sqrt(squaredDown.X + squaredDown.Z);
                expansion.Z += horizontalExpansionAmount * (float)Math.Sqrt(squaredDown.X + squaredDown.Y);

                Vector3.Add(ref expansion, ref boundingBox.Max, out boundingBox.Max);
                Vector3.Subtract(ref boundingBox.Min, ref expansion, out boundingBox.Min);

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

            if(PhysicsEntity.LinearVelocity != Velocity.ToBEPU())
                PhysicsEntity.LinearVelocity = Velocity.ToBEPU();
            
            if(PhysicsEntity.AngularVelocity != AngularVelocity.ToBEPU())
                PhysicsEntity.AngularVelocity = AngularVelocity.ToBEPU();
        }

        public BoundingBox GetBounds()
        {
            return new BoundingBox(new Vector3(-0.02f, 0, -0.02f), new Vector3(0.02f, 0.75f, 0.02f));
        }
    }
}