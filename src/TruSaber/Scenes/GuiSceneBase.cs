using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Serialization.Xaml;

namespace TruSaber.Scenes
{
    public abstract class GuiSceneBase<TScreen> : GuiSceneBase where TScreen : Screen,new()
    {
        private TScreen screen;

        protected GuiSceneBase() : base()
        {
            //RocketXamlLoader.Load<Screen, TScreen>(GuiScreen);
            screen = new TScreen();
            screen.Anchor = Alignment.Fill;
            screen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GuiScreen.Screen = screen;
        }
    }
    
    public abstract class GuiSceneBase : Scene
    {
        protected readonly GuiScreenEntity GuiScreen;
        private            Skybox          _skybox;
        private            GuiManager      _guiManager;

        public Vector2 ScreenSize { get; set; } = new Vector2(5f, 3f);
        
        protected GuiSceneBase()
        {
            GuiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game);
            
            GuiScreen.Transform.Position = new Vector3(-2f, 3f,-3f);
            GuiScreen.Transform.Scale = new Vector3(ScreenSize.X / 1f, ScreenSize.Y / 1f, 1f);
            GuiScreen.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0f, 180f.ToRadians(), 0f);

        }

        protected override void OnInitialize()
        {
            _guiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();
            _guiManager.ScaledResolution.ScaleChanged += ScaledResolutionOnScaleChanged;
            
            base.OnInitialize();
            
            _skybox = new Skybox(TruSaberGame.Instance)
            {
                Scale = Vector3.One
            };
            
            Components.Add(_skybox);
            Components.Add(GuiScreen);
        }

        private void ScaledResolutionOnScaleChanged(object? sender, UiScaleEventArgs e)
        {
            GuiScreen.Transform.Scale = new Vector3(ScreenSize.X / e.ScaledWidth, ScreenSize.Y / e.ScaledHeight, 1f);
        }

        protected override void OnShow()
        {
            _guiManager.AddScreen(GuiScreen.Screen);
            base.OnShow();            
        }

        protected override void OnHide()
        {
            base.OnHide();
            _guiManager.RemoveScreen(GuiScreen.Screen);
        }

    }
}