using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RocketUI;
using RocketUI.Input;
using RocketUI.Utilities.Helpers;
using SharpVR;
using TruSaber.Abstractions;
using TruSaber.Debugging;
using TruSaber.Graphics;
using TruSaber.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TruSaber
{
    public class TruSaberGame : Game, IGame
    {
        public        IServiceProvider ServiceProvider { get; }
        public        Game             Game     => this;
        public static IGame            Instance { get; private set; } 

        public GraphicsDeviceManager GraphicsDeviceManager => _graphics;

        public NotifyingCollection<ICamera> Cameras { get; } = new NotifyingCollection<ICamera>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public SceneManager   SceneManager   { get; private set; }
        public InputManager   InputManager   { get; private set; }
        
        public Player Player { get; private set; }
        
        
        
        public GuiManager GuiManager { get; private set; }

        public TruSaberGame(IServiceProvider services)
        {
            Instance = this;
            ServiceProvider = services;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var hostApplicationLifetime = services.GetRequiredService<IHostApplicationLifetime>();
            hostApplicationLifetime.ApplicationStopping.Register(OnHostApplicationStopping);

        }

        private void OnHostApplicationStopping()
        {
            GpuResourceManager.Dispose();
        }

        private DebugGui _debugGui;
        protected override void Initialize()
        {
            GraphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
//            GraphicsDeviceManager.PreferMultiSampling = true;
            IsFixedTimeStep = false;
            Window.AllowUserResizing = true;
            
            GraphicsDeviceManager.ApplyChanges();

            InputManager = ServiceProvider.GetRequiredService<InputManager>();
            Components.Add(InputManager);            
            
            Components.Add(SceneManager = ServiceProvider.GetRequiredService<SceneManager>());
            var vr = ServiceProvider.GetRequiredService<IVRService>();
            Components.Add(vr);
            
            Player = new Player(this);
            Components.Add(Player);

            base.Initialize();
        }

        private Camera _cam;
        protected override void LoadContent()
        {
            GpuResourceManager.Init(GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
            _cam = new Camera(Instance);
            _cam.Position = new Vector3(0f, 1.7f, 0f);
           // cam.Rotation = Quaternion.CreateFromYawPitchRoll(270f, (float)Math.PI / 2f, 0f);
            Cameras.Add(_cam);
            
            GuiManager = ServiceProvider.GetRequiredService<GuiManager>();
            GuiManager.AddScreen(_debugGui = new DebugGui());
            Components.Add(GuiManager);
            GuiManager.DrawOrder = 10;
            GuiManager.Init();

            Options = ServiceProvider.GetRequiredService<IOptions<GameOptions>>().Value;

            SceneManager.SetScene<MainMenuScene>();
//            SceneManager.SetScene<PlayLevelScene>();

            _graphics.GraphicsDevice.Viewport = new Viewport(Window.ClientBounds);
        }

        public GameOptions Options { get; set; }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (!Options.EmulateVr)
            {
                var hmd = ServiceProvider.GetRequiredService<IVrContext>().Hmd;
                _cam.Position = hmd.LocalPosition;
                _cam.Rotation = hmd.LocalRotation;
            }

            base.Update(gameTime);
        }
        
        public ICamera Camera { get; private set; }

        protected override void Draw(GameTime gameTime)
        {
            
            var cameras = Cameras.ToArray();
            foreach (var camera in cameras)
            {
                Camera = camera;
                //GuiManager.GuiSpriteBatch.Effect.View = camera.View;
                //GuiManager.GuiSpriteBatch.Effect.Projection = camera.Projection;
                camera.Draw(() => base.Draw(gameTime));
            }
            //
            // GraphicsDevice.SetRenderTarget(null);
            // GraphicsDevice.Clear(Color.Black);
            // base.Draw(gameTime);
            
        }
    }
}