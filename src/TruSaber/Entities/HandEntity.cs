using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpVR;
using TruSaber.Abstractions;
using Quaternion = Microsoft.Xna.Framework.Quaternion;

namespace TruSaber
{
    public class HandEntity : DrawableEntity
    {
        private Vector3 _angularVelocity;
        public  Player  Player { get; }
        public  Hand    Hand   { get; }

        public Color Color { get; set; }

        public  Quaternion ControllerOffset { get; set; }
        private IVrContext VrContext        { get; }

        public Ray Ray { get; private set; }

        public Vector3 AngularVelocity
        {
            get => _angularVelocity;
            set
            {
                if (_angularVelocity == value)
                    return;

                _angularVelocity = value;
                OnPositionChanged();
            }
        }

        public HandEntity(IGame game, Player player, Hand hand) : base(game)
        {
            Player = player;
            Hand = hand;
            Transform.ParentTransform = player.Transform;

            Color = hand == Hand.Left ? Color.Red : Color.Blue;
            VrContext = game.ServiceProvider.GetRequiredService<IVrContext>();
            ControllerOffset = Quaternion.Identity; //Quaternion.CreateFromYawPitchRoll(0f, -90f.ToRadians(), 0f);
            // PhysicsEntity = new TransformableEntity(Position.ToBEPU(),
            //     new BoxShape(0.04f, 0.75f, 0.04f), Matrix3x3.CreateFromMatrix(World.ToBEPU()));

            InitPhysics();
            Ray = new Ray();
            Scale = Vector3.One;
        }

        private VertexPositionColor[] _rayVerticies;
        private VertexBuffer          _vertexBuffer;
        private BasicEffect           _effect;

        /// <inheritdoc />
        public override void Initialize()
        {
            Transform.LocalPosition = Hand == Hand.Left ? Vector3.Left : Vector3.Right;
            _rayVerticies = new VertexPositionColor[2];
            _vertexBuffer = new VertexBuffer(Game.GraphicsDevice, VertexPositionColor.VertexDeclaration, 2, BufferUsage.WriteOnly);
            _effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!(Initialized && Enabled)) return;

            if (Hand == Hand.Left)
            {
                if (VrContext.LeftController != null)
                {
                    Transform.LocalPosition = VrContext.LeftController.LocalPosition;
                    //Transform.LocalQuaternion = Quaternion.Multiply(VrContext.LeftController.LocalRotation, ControllerOffset);
                    Transform.LocalRotation = VrContext.LeftController.LocalRotation;
                    //PhysicsEntity.AngularVelocity = VrContext.LeftController.GetAngularVelocity().ToBEPU();
                }
            }
            else if (Hand == Hand.Right)
            {
                if (VrContext.RightController != null)
                {
                    Transform.LocalPosition = VrContext.RightController.LocalPosition;
//                    Transform.LocalQuaternion = Quaternion.Multiply(VrContext.RightController.LocalRotation, ControllerOffset);
                    Transform.LocalRotation = VrContext.RightController.LocalRotation;
                }
            }

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Model = Game.Content.Load<Model>("Models/saberMesh");

            foreach (var mesh in Model.Meshes)
            {
                foreach (var effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = Color.ToVector3();
                    //_effect.SpecularColor = Color.ToVector3();

                    _effect.Alpha = 1.0f;
                }
            }
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
                var farPoint  = Vector3.Transform(Vector3.Forward, World);
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
            if (!(Initialized && Enabled)) return;

            base.Draw(gameTime);
            var cam = (Game as IGame).Camera;

            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            _effect.VertexColorEnabled = true;

            _effect.World = Matrix.Identity;
            _effect.View = cam.View;
            _effect.Projection = cam.Projection;

            // _effect.EnableDefaultLighting();

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 1);
            }
        }

        public void Vibrate()
        {
            var ctrl = Hand == Hand.Left ? VrContext.LeftController : VrContext.RightController;
            if (ctrl != null)
            {
                (ctrl as VRController)?.Vibrate();
            }
        }
    }
}