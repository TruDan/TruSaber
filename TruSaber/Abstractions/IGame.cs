using System;
using Microsoft.Xna.Framework;
using RocketUI;
using TruSaber.Scenes;

namespace TruSaber.Abstractions
{
    public interface IGame : IDisposable
    {
        Game Game { get; }
        
        GraphicsDeviceManager GraphicsDeviceManager { get; }
        
        // include all the Properties/Methods that you'd want to use on your Game class below.
        GameWindow Window { get; }
        ICamera Camera { get; }
        NotifyingCollection<ICamera> Cameras { get; }
        SceneManager SceneManager { get; }
        
        Player Player { get; }
        
        IServiceProvider ServiceProvider { get; }
        GuiManager           GuiManager      { get; }

        event EventHandler<EventArgs> Exiting;

        void Run();
        void Exit();
    }
}