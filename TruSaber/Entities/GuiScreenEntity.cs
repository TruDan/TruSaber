using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;
using RocketUI.Abstractions;
using RocketUI.Graphics;
using RocketUI.Primitive;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class GuiScreenEntity : GuiScreen
    {
        private readonly Game        _game;
        public           Transform3D Transform { get; } = new Transform3D();

        private float    _dotsPerMm;
        private Viewport _viewport;

        private RenderTarget2D _renderTarget;

        public float DotsPerInch
        {
            get => _dotsPerMm / 25.4f;
            set { _dotsPerMm = value * 25.4f; }
        }

        public GuiScreenEntity(Game game, int width, int height) : base()
        {
            _game = game;
            //AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClipToBounds = false;
            Background = Color.GreenYellow;
            _viewport = new Viewport(0, 0, Width, Height);
            UpdateSize(width, height);
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            base.OnInit(renderer);
            _renderTarget = new RenderTarget2D(_game.GraphicsDevice, Width, Height);
        }

        /// <summary>Draw this component.</summary>
        /// <param name="gameTime">The time elapsed since the last call to <see cref="M:Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime)" />.</param>
        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            //using (var context = graphics.BeginTransform(Transform.World))
           // using (var gfx = _game.GraphicsDevice.PushRenderTarget(_renderTarget))
            using (var cxt = graphics.BranchContext(BlendState.AlphaBlend, DepthStencilState.DepthRead,
                RasterizerState.CullNone, SamplerState.PointWrap))
            using (graphics.BeginWorld(Transform.World))
            {
                cxt.Viewport = _viewport;
                graphics.BeginTransform(Transform.World);
                graphics.Begin();

                base.OnDraw(graphics, gameTime);

                graphics.End();
            }

            graphics.Begin();
            graphics.DrawRectangle(new Rectangle(50, 50, 200, 200), Color.Aqua, 5);
            graphics.FillRectangle(Bounds, Color.LimeGreen);
            graphics.End();
        }
    }
}