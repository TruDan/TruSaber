using DiscordRPC;
using Microsoft.Xna.Framework;

namespace TruSaber.Scenes
{
    public interface IScene
    {
        GameComponentCollection Components { get; }
        
        void Initialize();
        void Show();
        void Hide();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);

        RichPresence GetPresence();
    }
}