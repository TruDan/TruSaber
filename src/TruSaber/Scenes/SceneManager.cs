using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using TruSaber.Abstractions;
using TruSaber.Services;

namespace TruSaber.Scenes
{
    public class SceneManager : DrawableGameComponent
    {
        private readonly IDiscordRichPresenseProvider _richPresenseProvider;
        public           IScene                       ActiveScene { get; private set; }

        private Stack<IScene> _stack;

        private bool _initialized = false;
        public SceneManager(IGame game, IDiscordRichPresenseProvider richPresenseProvider) : base(game.Game)
        {
            _richPresenseProvider = richPresenseProvider;
            DrawOrder = 0;
            _stack = new Stack<IScene>();
        }

        public override void Initialize()
        {
            _initialized = true;
            base.Initialize();
            
            ActiveScene?.Initialize();
        }

        public void SetScene<TScene>() where TScene : IScene, new()
        {
            var scene = new TScene();
            SetScene(scene);
        }

        public void SetScene<TScene>([NotNull] TScene scene) where TScene : IScene
        {
            if(_initialized)
                scene.Initialize();
            ActiveScene?.Hide();
            ActiveScene = scene;
            scene?.Show();
            _richPresenseProvider.SetPresence(scene?.GetPresence());
        }

        public void PushScene<TScene>() where TScene : IScene, new()
        {
            if(ActiveScene != null)
                _stack.Push(ActiveScene);
            SetScene<TScene>();
        }
        
        public void PushScene<TScene>([NotNull] TScene scene) where TScene : IScene
        {
            if(ActiveScene != null)
                _stack.Push(ActiveScene);
            SetScene<TScene>(scene);
        }

        public void Pop()
        {
            if (_stack.TryPop(out var previousScene))
            {
                SetScene(previousScene);
            }
        }

        public void Back() => Pop();
        
        public void ResetStack()
        {
            _stack.Clear();
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