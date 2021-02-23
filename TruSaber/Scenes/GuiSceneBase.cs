using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;

namespace TruSaber.Scenes
{
    public abstract class GuiSceneBase : Scene
    {
        protected readonly GuiScreenEntity GuiScreen;
        private            Skybox          _skybox;
        private            GuiManager      _guiManager;

        protected GuiSceneBase()
        {
            GuiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game, 800, 600);
            
            GuiScreen.Transform.Position = new Vector3(-2f, 3f,-3f);
            GuiScreen.Transform.Scale = new Vector3(5f / 800f, 3f / 600f, 1f);
            GuiScreen.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0f, 180f.ToRadians(), 0f);

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
            Components.Add(GuiScreen);
        }

        protected void AddChild(IGuiElement element) => GuiScreen.AddChild(element);
        
        protected override void OnShow()
        {
            _guiManager.AddScreen(GuiScreen);
            base.OnShow();            
        }

        protected override void OnHide()
        {
            base.OnHide();
            _guiManager.RemoveScreen(GuiScreen);
        }

    }
}