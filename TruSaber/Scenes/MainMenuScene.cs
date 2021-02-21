﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Primitive;
using TruSaber.Graphics.Gui;

namespace TruSaber.Scenes
{
    public class MainMenuScene : GuiSceneBase
    {
        public MainMenuScene()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            var stack = new GuiStackMenu()
            {
                Anchor = Alignment.FillLeft,
                ChildAnchor = Alignment.MiddleFill,
                Orientation = Orientation.Vertical,
                MinWidth = 200,
                MinHeight = 300,
                Width = 300,
                Height = 400
            };
            stack.ChildAdded += (sender, args) =>
            {
                args.Child.Margin = Thickness.One * 50;
                args.Child.Padding = Thickness.One * 50;
                args.Child.Height = 100;
            };
            AddChild(stack);
            
            stack.AddMenuItem("PLAY", () => TruSaberGame.Instance.SceneManager.SetScene<PlayLevelScene>());
            stack.AddMenuItem("CHOOSE LEVEL", () => TruSaberGame.Instance.SceneManager.SetScene<SelectLevelScene>());
            stack.AddMenuItem("OPTIONS", () => TruSaberGame.Instance.SceneManager.SetScene<OptionsScene>());
            stack.AddMenuItem("EXIT", () => TruSaberGame.Instance.Exit());

            
            // _guiScreen.AddChild(new GuiVRControlDebug()
            // {
            //     Anchor = Alignment.FillRight
            // });
        }

    }
}