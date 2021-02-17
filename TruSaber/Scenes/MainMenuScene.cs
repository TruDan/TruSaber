using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Primitive;

namespace TruSaber.Scenes
{
    public class MainMenuScene : Scene
    {
        private GuiScreenEntity _guiScreen;

        private GuiManager _guiManager;
        private GuiButton  _playButton;
        public MainMenuScene()
        {
            _guiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game, 800, 600);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _guiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();

            _guiScreen.Transform.Position = (Vector3.Forward * 7.5f) + (Vector3.Up * 2f);
            _guiScreen.Transform.Scale = new Vector3(3f);
            _guiScreen.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 180f.ToRadians());
            
            Components.Add(_guiScreen);

            var _stack = new GuiStackMenu();
            _stack.AddMenuItem("PLAY", () => TruSaberGame.Instance.SceneManager.SetScene<PlayLevelScene>());
            _stack.AddMenuItem("CHOOSE LEVEL", () => TruSaberGame.Instance.SceneManager.SetScene<SelectLevelScene>());
            _guiScreen.AddChild(_stack);
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