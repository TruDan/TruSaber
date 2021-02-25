using System;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Serialization.Xaml;
using TruSaber.Graphics.Gui;

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