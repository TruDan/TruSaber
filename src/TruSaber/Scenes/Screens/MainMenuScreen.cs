using RocketUI;
using RocketUI.Serialization.Xaml;

namespace TruSaber.Scenes.Screens
{
    public partial class MainMenuScreen : Screen
    {
        public MainMenuScreen() : base()
        {
            RocketXamlLoader.Load(this);

            FindControl<StackMenuItem>("PlayButton").Action =
                () => TruSaberGame.Instance.SceneManager.PushScene<SelectLevelScene>();
            FindControl<StackMenuItem>("OptionsButton").Action =
                () => TruSaberGame.Instance.SceneManager.PushScene<OptionsScene>();
            FindControl<StackMenuItem>("ExitButton").Action = () => TruSaberGame.Instance.Exit();
        }
    }
}