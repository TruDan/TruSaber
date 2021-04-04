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
    public class NoteEntity : BaseTrackEntity
    {
        private readonly Note         _note;
        public           CutDirection CutDirection { get; set; }
        public           NoteType     Type         { get; set; }
        public Model ArrowModel { get; private set; }
        public Color Color      { get; }

        public NoteEntity(IGame game, Note note, float positioningMultiplier) : base(game, note, positioningMultiplier)
        {
            CutDirection = note.CutDirection;
            Type = note.Type;

            Color = note.Type == NoteType.Bomb ? Color.DarkGray : (note.Type == NoteType.LeftNote ? Color.Red : Color.Blue);
            BoundingBoxSize = Vector3.One;
            BoundingBoxOrigin = Vector3.One / 2f;
            
            base.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, CutDirection.ToAngle());
            
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

            //if(false)
                BoundingBox.Draw(GraphicsDevice, Color);
        }
    }
}