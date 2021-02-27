using System;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using RocketUI;
using TruSaber.Graphics.Gui;
using TruSaber.Models;
using TruSaber.Scenes.Screens;
using TruSaber.Services;
using Button = RocketUI.Button;

namespace TruSaber.Scenes
{
    public class SelectLevelScene : GuiSceneBase<SelectLevelScreen>
    {
        
        public SelectLevelScene()
        {
        }

        protected override void OnHide()
        {
            base.OnHide();
            MediaPlayer.Stop();
        }

        public override RichPresence GetPresence()
            => new RichPresence()
            {
                State = "In Menus",
                Details = "Selecting Song"
            };

    }
}