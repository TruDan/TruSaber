using Microsoft.Xna.Framework;
using SharpVR;

namespace TruSaber.Abstractions
{
    public interface IVRService : IGameComponent
    {
        
        IVrContext Context { get; }
        
    }
}