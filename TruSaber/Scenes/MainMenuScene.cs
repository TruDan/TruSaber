using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Primitive;

namespace TruSaber.Scenes
{
    public class MainMenuScene : Scene
    {
        private GuiScreenEntity _guiScreen;

        private GuiManager _guiManager;
        public MainMenuScene()
        {
            _guiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game, 800, 600);

            
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _guiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();

            _guiScreen.Initialize();
            _guiScreen.Transform.Position = (Vector3.Forward * 7.5f) + Vector3.Up;
            _guiScreen.Transform.Scale = new Vector3(1.5f);
            _guiScreen.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 180f.ToRadians());
            
            Components.Add(_guiScreen);
            
            var el = new GuiElement();
            _guiScreen.AddChild(el);
            _guiScreen.Background = Color.PowderBlue;
            
            el.Background = Color.MediumVioletRed;
            el.Width = 300;
            el.Height = 50;
            el.Anchor = Alignment.MiddleCenter;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);
            
        }

        protected override void OnShow()
        {
           // _guiManager.AddScreen(_guiScreen);
            base.OnShow();            
        }

        protected override void OnHide()
        {
            base.OnHide();
            //_guiManager.RemoveScreen(_guiScreen);
        }
    }
}