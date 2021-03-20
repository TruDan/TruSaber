using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Serialization.Xaml;

namespace TruSaber.Scenes
{
    public abstract class GuiSceneBase<TScreen> : GuiSceneBase where TScreen : Screen, new()
    {
        private TScreen _screen;

        protected GuiSceneBase() : this(new TScreen()) { }
        
        protected GuiSceneBase(TScreen screen) : base()
        {
            _screen = screen;
            _screen.Anchor = Alignment.Fill;
            _screen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GuiScreen.Screen = _screen;
        }
    }
    
    public abstract class GuiSceneBase : Scene
    {
        protected readonly GuiScreenEntity GuiScreen;
        protected readonly GuiManager      GuiManager;
        private            Skybox          _skybox;
        private            Vector2         _screenSize = new Vector2(5f, 3f);

        public Vector2 ScreenSize
        {
            get => _screenSize;
            set
            {
                _screenSize = value;
                OnScreenSizeChanged();
            }
        }


        protected GuiSceneBase()
        {
            GuiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();
            GuiManager.ScaledResolution.ScaleChanged += ScaledResolutionOnScaleChanged;
            
            GuiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game);
            
            GuiScreen.Transform.RelativePosition = new Vector3(-(ScreenSize.X /2f), ScreenSize.Y,-3f);
            GuiScreen.Transform.RelativeScale = new Vector3(ScreenSize.X / GuiManager.ScaledResolution.ScaledWidth, ScreenSize.Y / GuiManager.ScaledResolution.ScaledHeight, 1f);
            GuiScreen.Transform.RelativeRotation = Quaternion.CreateFromYawPitchRoll(0f, 180f.ToRadians(), 0f);

        }

        protected override void OnInitialize()
        {
            
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
            OnScreenSizeChanged();
        }

        private void OnScreenSizeChanged()
        {
            GuiScreen.Transform.RelativePosition = new Vector3(-(ScreenSize.X /2f), ScreenSize.Y,-3f);
            GuiScreen.Transform.RelativeScale = new Vector3((float)ScreenSize.X / GuiManager.ScaledResolution.ScaledWidth, (float)ScreenSize.Y / GuiManager.ScaledResolution.ScaledHeight, 1f);
        }

    }
}