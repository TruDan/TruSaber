using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Valve.VR;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace SharpVR.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMatrix34Conversion()
        {
            var m = Matrix.CreateScale(0.995f) * Matrix.CreateRotationY((float) Math.PI / 2f) *
                    Matrix.CreateTranslation(Vector3.Left);

            var steamVRMatrix       = (HmdMatrix34_t) m;
            var convertedBackMatrix = (Matrix) steamVRMatrix;
            
            Assert.AreEqual(m, convertedBackMatrix);
        }
        [Test]
        public void TestMatrix44Conversion()
        {
            var m = Matrix.CreateScale(0.995f) * Matrix.CreateRotationY((float) Math.PI / 2f) *
                    Matrix.CreateTranslation(Vector3.Left);

            var steamVRMatrix       = (HmdMatrix44_t) m;
            var convertedBackMatrix = (Matrix) steamVRMatrix;
            
            Assert.AreEqual(m, convertedBackMatrix);
        }
    }
}