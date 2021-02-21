using DiscordRPC;

namespace TruSaber.Scenes
{
    public class OptionsScene : GuiSceneBase
    {
        public override RichPresence GetPresence()
            => new RichPresence()
            {
                State = "In Menus",
                Details = "Configuring Options"
            };
    }
}