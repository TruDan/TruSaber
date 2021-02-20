using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Primitive;
using TruSaber.Graphics.Gui;

namespace TruSaber.Scenes
{
    public class MainMenuScene : Scene
    {
        private GuiScreenEntity _guiScreen;

        private GuiManager _guiManager;
        private GuiButton  _playButton;
        private Skybox     _skybox;
        public MainMenuScene()
        {
            _guiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game, 800, 600);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _guiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();

            var pos    = (Vector3.Forward * 7.5f) + (Vector3.Up * 2f);
            var aspect = 800f / 600f;
            _guiScreen.Transform.Position = new Vector3(-2f, 3f,-3f);
            _guiScreen.Transform.Scale = new Vector3(5f / 800f, 3f / 600f, 1f);
            _guiScreen.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0f, 180f.ToRadians(), 0f);
            

            var _stack = new GuiStackMenu()
            {
                Anchor = Alignment.FillLeft,
                ChildAnchor = Alignment.TopFill,
                Orientation = Orientation.Vertical,
                Background = Color.Red,
                MinWidth = 200,
                MinHeight = 300,
                Width = 200,
                Height = 300
            };
            _stack.ChildAdded += (sender, args) =>
            {
                args.Child.Margin = Thickness.One * 50;
                args.Child.Padding = Thickness.One * 50;
                args.Child.Height = 100;
            };
            _guiScreen.AddChild(_stack);
            
            _stack.AddMenuItem("PLAY", () => TruSaberGame.Instance.SceneManager.SetScene<PlayLevelScene>());
            _stack.AddMenuItem("CHOOSE LEVEL", () => TruSaberGame.Instance.SceneManager.SetScene<SelectLevelScene>());

            _skybox = new Skybox(TruSaberGame.Instance)
            {
                Scale = Vector3.One
            };
            
            Components.Add(_skybox);
            Components.Add(_guiScreen);
            
            _guiScreen.AddChild(new GuiVRControlDebug()
            {
                Anchor = Alignment.FillRight
            });
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
            _guiManager.AddScreen(_guiScreen);
            base.OnShow();            
        }

        protected override void OnHide()
        {
            base.OnHide();
            _guiManager.RemoveScreen(_guiScreen);
        }
    }
}