using System;
using Microsoft.Xna.Framework;

namespace TruSaber.Abstractions
{
    public interface ICamera : IUpdateable
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Forward { get; }
        public Vector3 Up { get; }
        
        public Matrix View { get; }
        public Matrix Projection { get; }

        public void Draw(Action doDraw);

    }
}