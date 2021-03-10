using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class GuiScreenEntity : DrawableEntity, IGuiManaged, IDrawable
    {
        private readonly Game _game;

        private Crosshair _crosshair;
        private Screen    _screen;

        public Screen Screen
        {
            get => _screen;
            set { SetupScreen(value); }
        }

        private GuiManager GuiManager => ((IGame) Game).GuiManager;

        public GuiScreenEntity(Game game) : base((IGame) game)
        {
            _game = game;
            _crosshair = new Crosshair();
        }

        private void SetupScreen(Screen screen)
        {
            if (_screen != null)
            {
                GuiManager.RemoveScreen(_screen);
            }
            _screen = screen;

            if (screen == null) return;

            screen.Tag = this;
            screen.IsSelfManaged = true;
            screen.Background = (Color.Black * 0.2f);
            screen.ClipToBounds = true;
            //screen.UpdateSize(screen.Width, screen.Height);

            screen.AddChild(_crosshair);
            GuiManager.AddScreen(_screen);
        }

        public override void Update(GameTime gameTime)
        {
            _screen?.Update(gameTime);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            var game     = (IGame) _game;
            var cam      = game.Camera;
            var graphics = game.GuiManager.GuiSpriteBatch;

            using (var cxt = graphics.BranchContext(BlendState.AlphaBlend, DepthStencilState.None,
                RasterizerState.CullNone, SamplerState.LinearClamp))
            using (graphics.BeginWorld(Transform.World))
            using (graphics.BeginViewProjection(cam.View, cam.Projection))
            {
                graphics.Begin(true);

                _screen.Draw(graphics, gameTime);

                game.GuiManager.InvokeDrawScreen(_screen, gameTime);
                graphics.End();
            }
        }
    }
}