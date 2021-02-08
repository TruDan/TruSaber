using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using TruSaber.Abstractions;

namespace TruSaber.Scenes
{
    public class SceneManager : DrawableGameComponent
    {
        public IScene ActiveScene { get; private set; }

        public SceneManager(IGame game) : base(game.Game)
        {
            
        }


        public void SetScene<TScene>() where TScene : IScene, new()
        {
            var scene = new TScene();
            scene.Initialize();
            SetScene(scene);
        }

        public void SetScene<TScene>([NotNull] TScene scene) where TScene : IScene
        {
            ActiveScene?.Hide();
            ActiveScene = scene;
            scene?.Show();
        }

        public override void Update(GameTime gameTime)
        {
            ActiveScene?.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            ActiveScene?.Draw(gameTime);
        }
        
    }
}