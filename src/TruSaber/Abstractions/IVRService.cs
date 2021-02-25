using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpVR;
using Valve.VR;

namespace TruSaber.Abstractions
{
    public interface IVRService : IGameComponent
    {
        
        IVrContext Context { get; }
        
    }
}