using Microsoft.Xna.Framework;
using RocketUI;
using TruSaber.Abstractions;
using TruSaber.Scenes;

namespace TruSaber.Graphics.Gui
{
    public class GuiMinimap : RocketElement
    {

        public IScene Scene { get; set; }
        
        public float GridSpacing   { get; set; }
        public Color GridLineColor { get; set; }

        public Vector2 Center { get; set; }
        
        public Camera Camera { get; set; }

        private DebugGrid _debugGrid;
        public GuiMinimap()
        {
            
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            base.OnInit(renderer);

            Camera = new Camera(TruSaberGame.Instance);
            Camera.ProjectionType = ProjectionType.Orthographic;
            Camera.Rotation = Quaternion.CreateFromYawPitchRoll(0f, 90f.ToRadians(), 0f);
            Camera.Position = Vector3.Up * 100;
            Camera.Bounds = Bounds;

            _debugGrid = new DebugGrid();
            _debugGrid.Init(TruSaberGame.Instance.Game.GraphicsDevice);
        }

        protected override void OnUpdateLayout()
        {
            base.OnUpdateLayout();
            Camera.Bounds = Bounds;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            _debugGrid.Update();
        }


        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            base.OnDraw(graphics, gameTime);

            _debugGrid.Draw(graphics.Context.GraphicsDevice, Camera.View, Camera.Projection);
        }
    }
}