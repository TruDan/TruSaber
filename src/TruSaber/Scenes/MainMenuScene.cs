﻿using System;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Serialization.Xaml;
using TruSaber.Graphics.Gui;
using TruSaber.Scenes.Screens;

namespace TruSaber.Scenes
{
    public partial class MainMenuScene : GuiSceneBase<MainMenuScreen>
    {
        public MainMenuScene()
        {
            
        }

        public override RichPresence GetPresence() =>
            new RichPresence()
            {
                State = "In Menus",
                Timestamps = Timestamps.Now
            };
    }
}