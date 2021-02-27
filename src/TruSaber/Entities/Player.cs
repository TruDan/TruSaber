using System.Collections.Generic;
using AnimationAux;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI.Input;
using SharpVR;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class Player : DrawableEntity
    {
        
        private Bone _head;
        private Bone _leftHand;
        private Bone _rightHand;
        private float _modelHeight;

        public float Height { get; set; } = 1.8f;

        public HandEntity LeftHand { get; }
        public HandEntity RightHand { get; }

        private PlayerInputManager _playerInputManager;

        public Player(IGame game) : base(game)
        {
            LeftHand = new HandEntity(game, this, Hand.Left);
            RightHand = new HandEntity(game, this, Hand.Right);

            _playerInputManager = game.InputManager.GetOrAddPlayerManager(PlayerIndex.One);
        }

        public override void Initialize()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            
            LeftHand.Initialize();
            RightHand.Initialize();
            
            base.Initialize();
            //ResizeToHeight();
        }

        public override void Update(GameTime gameTime)
        {
            LeftHand.Update(gameTime);
            RightHand.Update(gameTime);
            // if (_vrService.Enabled)
            // {
            //     //ResizeToHeight();
            //     //var pos = _vrService.Context.Hmd.GetPose().Translation;
            //     //Position = new Vector3(pos.X, 0, pos.Z);
            //     _vrService.Context.Hmd.GetRelativePosition(ref _headPosition);
            //     _vrService.Context.Hmd.GetRelativeRotation(ref _headRotation);
            //     Position = new Vector3(_headPosition.X, 0, _headPosition.Z);
            //     
            //     if (_head != null)
            //     {
            //         _head.SetCompleteTransform(
            //           Matrix.CreateFromQuaternion(_headRotation)
            //                                    * Matrix.CreateTranslation(_headPosition));
            //     }
            //
            //     if (_leftHand != null && _vrService.Context.LeftController != null)
            //     {
            //         _vrService.Context.LeftController.GetRelativePosition(ref _leftHandPosition);
            //         _vrService.Context.LeftController.GetRelativeRotation(ref _leftHandRotation);
            //         _leftHand.SetCompleteTransform(Matrix.CreateFromQuaternion(_leftHandRotation)
            //                                        * Matrix.CreateTranslation(_leftHandPosition));
            //     }
            //
            //     if (_rightHand != null && _vrService.Context.RightController != null)
            //     {
            //         _vrService.Context.RightController.GetRelativePosition(ref _rightHandPosition);
            //         _vrService.Context.RightController.GetRelativeRotation(ref _rightHandRotation);
            //         _rightHand.SetCompleteTransform(Matrix.CreateFromQuaternion(_rightHandRotation)
            //                                        * Matrix.CreateTranslation(_rightHandPosition));
            //     }
            // }

            base.Update(gameTime);
        }

        private List<Bone> bones = new List<Bone>();

        #region Bones Management

        /// <summary>
        /// Get the bones from the model and create a bone class object for
        /// each bone. We use our bone class to do the real animated bone work.
        /// </summary>
        private void ObtainBones()
        {
            bones.Clear();
            foreach (ModelBone bone in Model.Bones)
            {
                // Create the bone object and add to the heirarchy
                Bone newBone = new Bone(bone.Name, bone.Transform,
                    bone.Parent != null ? bones[bone.Parent.Index] : null);

                // Add to the bones for this model
                bones.Add(newBone);
            }
        }

        /// <summary>
        /// Find a bone in this model by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Bone FindBone(string name)
        {
            foreach (Bone bone in bones)
            {
                if (bone.Name == name)
                    return bone;
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Extra data associated with the XNA model
        /// </summary>
        private ModelExtra modelExtra = null;

        private Vector3 _leftHandPosition, _rightHandPosition, _headPosition;
        private Quaternion _leftHandRotation, _rightHandRotation, _headRotation;

        public override void Draw(GameTime gameTime)
        {
            LeftHand.Draw(gameTime);
            RightHand.Draw(gameTime);
            //base.Draw(gameTime);
        }
        
        protected override void LoadContent()
        {
            
            
            Model = Game.Content.Load<Model>("Models/Characters/Player");

            modelExtra = Model.Tag as ModelExtra;

            ObtainBones();

            _leftHand = FindBone("LeftHand") ?? FindBone("Left wrist");
            _rightHand = FindBone("RightHand") ?? FindBone("Right wrist");
            _head = FindBone("Head");
            
            for (int i = 0; i < bones.Count; i++)
            {
                Bone bone = bones[i];
                bone.ComputeAbsoluteTransform();
                if (bone.Name.Equals("Head"))
                {
                    _modelHeight = bone.AbsoluteTransform.Translation.Y;
                }
            }

            base.LoadContent();
        }
    }
}