using DiscordRPC;
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