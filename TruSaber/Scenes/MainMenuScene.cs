using System;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Serialization.Xaml;
using TruSaber.Graphics.Gui;

namespace TruSaber.Scenes
{
    public partial class MainMenuScene : GuiSceneBase
    {
        public MainMenuScene()
        {
            RocketXamlLoader.Load(GuiScreen, $"{GetType().FullName}.xaml");
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            //
            // var stack = new GuiStackMenu()
            // {
            //     Anchor = Alignment.FillLeft,
            //     ChildAnchor = Alignment.MiddleFill,
            //     Orientation = Orientation.Vertical,
            //     MinWidth = 200,
            //     MinHeight = 300,
            //     Width = 300,
            //     Height = 400
            // };
            // stack.ChildAdded += (sender, args) =>
            // {
            //     args.Child.Margin = Thickness.One * 50;
            //     args.Child.Padding = Thickness.One * 50;
            //     args.Child.Height = 100;
            // };
            // AddChild(stack);
            //
            // stack.AddMenuItem("PLAY", () => TruSaberGame.Instance.SceneManager.PushScene<SelectLevelScene>());
            // stack.AddMenuItem("OPTIONS", () => TruSaberGame.Instance.SceneManager.PushScene<OptionsScene>());
            // stack.AddMenuItem("EXIT", () => TruSaberGame.Instance.Exit());

            
            // _guiScreen.AddChild(new GuiVRControlDebug()
            // {
            //     Anchor = Alignment.FillRight
            // });
        }

        public override RichPresence GetPresence() =>
            new RichPresence()
            {
                State = "In Menus",
                Timestamps = Timestamps.Now
            };
    }
}