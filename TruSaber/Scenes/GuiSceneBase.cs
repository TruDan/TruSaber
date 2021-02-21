using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Abstractions;

namespace TruSaber.Scenes
{
    public abstract class GuiSceneBase : Scene
    {
        private GuiScreenEntity _guiScreen;
        private Skybox          _skybox;
        private GuiManager      _guiManager;

        protected GuiSceneBase()
        {
            _guiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game, 800, 600);
            
            _guiScreen.Transform.Position = new Vector3(-2f, 3f,-3f);
            _guiScreen.Transform.Scale = new Vector3(5f / 800f, 3f / 600f, 1f);
            _guiScreen.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0f, 180f.ToRadians(), 0f);

        }

        protected override void OnInitialize()
        {
            _guiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();
            
            base.OnInitialize();
            
            _skybox = new Skybox(TruSaberGame.Instance)
            {
                Scale = Vector3.One
            };
            
            Components.Add(_skybox);
            Components.Add(_guiScreen);
        }

        protected void AddChild(IGuiElement element) => _guiScreen.AddChild(element);
        
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