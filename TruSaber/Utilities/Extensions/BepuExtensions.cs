using BEPUMatrix = BEPUutilities.Matrix;
using BEPUMatrix2x2 = BEPUutilities.Matrix2x2;
using BEPUMatrix2x3 = BEPUutilities.Matrix2x3;
using BEPUMatrix3x2 = BEPUutilities.Matrix3x2;
using BEPUMatrix3x3 = BEPUutilities.Matrix3x3;
using BEPUQuaternion = BEPUutilities.Quaternion;
using BEPUVector2 = BEPUutilities.Vector2;
using BEPUVector3 = BEPUutilities.Vector3;
using BEPUVector4 = BEPUutilities.Vector4;
using BEPUPlane = BEPUutilities.Plane;
using BEPUBoundingSphere = BEPUutilities.BoundingSphere;
using BEPUBoundingBox = BEPUutilities.BoundingBox;
using BEPURay = BEPUutilities.Ray;

using XnaMatrix = Microsoft.Xna.Framework.Matrix;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using XnaVector3 = Microsoft.Xna.Framework.Vector3;
using XnaVector4 = Microsoft.Xna.Framework.Vector4;
using XnaQuaternion = Microsoft.Xna.Framework.Quaternion;
using XnaPlane = Microsoft.Xna.Framework.Plane;
using XnaBoundingSphere = Microsoft.Xna.Framework.BoundingSphere;
using XnaBoundingBox = Microsoft.Xna.Framework.BoundingBox;
using XnaRay = Microsoft.Xna.Framework.Ray;

namespace TruSaber.Utilities.Extensions
{
    public static class BepuExtensions
    {

        public static BEPUMatrix ToBEPU(this XnaMatrix m)
        {
            return new(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41,
                m.M42, m.M43, m.M44);
        }
        public static XnaMatrix ToXna(this BEPUMatrix m)
        {
            return new(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41,
                m.M42, m.M43, m.M44);
        }

        public static BEPUVector3 ToBEPU(this XnaVector3 v)
        {
            return new(v.X, v.Y, v.Z);
        }
        public static XnaVector3 ToXna(this BEPUVector3 v)
        {
            return new(v.X, v.Y, v.Z);
        }
        
        public static BEPUQuaternion ToBEPU(this XnaQuaternion q)
        {
            return new(q.X, q.Y, q.Z, q.W);
        }
        public static XnaQuaternion ToXna(this BEPUQuaternion q)
        {
            return new(q.X, q.Y, q.Z, q.W);
        }
    }
    
    /// <summary>
    /// Helps convert between XNA math types and the BEPUphysics replacement math types.
    /// A version of this converter could be created for other platforms to ease the integration of the engine.
    /// </summary>
    public static class MathConverter
    {
        //Vector2
        public static XnaVector2 Convert(BEPUVector2 bepuVector)
        {
            XnaVector2 toReturn;
            toReturn.X = bepuVector.X;
            toReturn.Y = bepuVector.Y;
            return toReturn;
        }

        public static void Convert(ref BEPUVector2 bepuVector, out XnaVector2 xnaVector)
        {
            xnaVector.X = bepuVector.X;
            xnaVector.Y = bepuVector.Y;
        }

        public static BEPUVector2 Convert(XnaVector2 xnaVector)
        {
            BEPUVector2 toReturn;
            toReturn.X = xnaVector.X;
            toReturn.Y = xnaVector.Y;
            return toReturn;
        }

        public static void Convert(ref XnaVector2 xnaVector, out BEPUVector2 bepuVector)
        {
            bepuVector.X = xnaVector.X;
            bepuVector.Y = xnaVector.Y;
        }

        //Vector3
        public static XnaVector3 Convert(BEPUVector3 bepuVector)
        {
            XnaVector3 toReturn;
            toReturn.X = bepuVector.X;
            toReturn.Y = bepuVector.Y;
            toReturn.Z = bepuVector.Z;
            return toReturn;
        }

        public static void Convert(ref BEPUVector3 bepuVector, out XnaVector3 xnaVector)
        {
            xnaVector.X = bepuVector.X;
            xnaVector.Y = bepuVector.Y;
            xnaVector.Z = bepuVector.Z;
        }

        public static BEPUVector3 Convert(XnaVector3 xnaVector)
        {
            BEPUVector3 toReturn;
            toReturn.X = xnaVector.X;
            toReturn.Y = xnaVector.Y;
            toReturn.Z = xnaVector.Z;
            return toReturn;
        }

        public static void Convert(ref XnaVector3 xnaVector, out BEPUVector3 bepuVector)
        {
            bepuVector.X = xnaVector.X;
            bepuVector.Y = xnaVector.Y;
            bepuVector.Z = xnaVector.Z;
        }

        public static XnaVector3[] Convert(BEPUVector3[] bepuVectors)
        {
            XnaVector3[] xnaVectors = new XnaVector3[bepuVectors.Length];
            for (int i = 0; i < bepuVectors.Length; i++)
            {
                Convert(ref bepuVectors[i], out xnaVectors[i]);
            }
            return xnaVectors;

        }

        public static BEPUVector3[] Convert(XnaVector3[] xnaVectors)
        {
            var bepuVectors = new BEPUVector3[xnaVectors.Length];
            for (int i = 0; i < xnaVectors.Length; i++)
            {
                Convert(ref xnaVectors[i], out bepuVectors[i]);
            }
            return bepuVectors;

        }

        //Matrix
        public static XnaMatrix Convert(BEPUMatrix matrix)
        {
            XnaMatrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

        public static BEPUMatrix Convert(XnaMatrix matrix)
        {
            BEPUMatrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

        public static void Convert(ref BEPUMatrix matrix, out XnaMatrix xnaMatrix)
        {
            xnaMatrix.M11 = matrix.M11;
            xnaMatrix.M12 = matrix.M12;
            xnaMatrix.M13 = matrix.M13;
            xnaMatrix.M14 = matrix.M14;

            xnaMatrix.M21 = matrix.M21;
            xnaMatrix.M22 = matrix.M22;
            xnaMatrix.M23 = matrix.M23;
            xnaMatrix.M24 = matrix.M24;

            xnaMatrix.M31 = matrix.M31;
            xnaMatrix.M32 = matrix.M32;
            xnaMatrix.M33 = matrix.M33;
            xnaMatrix.M34 = matrix.M34;

            xnaMatrix.M41 = matrix.M41;
            xnaMatrix.M42 = matrix.M42;
            xnaMatrix.M43 = matrix.M43;
            xnaMatrix.M44 = matrix.M44;

        }

        public static void Convert(ref XnaMatrix matrix, out BEPUMatrix bepuMatrix)
        {
            bepuMatrix.M11 = matrix.M11;
            bepuMatrix.M12 = matrix.M12;
            bepuMatrix.M13 = matrix.M13;
            bepuMatrix.M14 = matrix.M14;

            bepuMatrix.M21 = matrix.M21;
            bepuMatrix.M22 = matrix.M22;
            bepuMatrix.M23 = matrix.M23;
            bepuMatrix.M24 = matrix.M24;

            bepuMatrix.M31 = matrix.M31;
            bepuMatrix.M32 = matrix.M32;
            bepuMatrix.M33 = matrix.M33;
            bepuMatrix.M34 = matrix.M34;

            bepuMatrix.M41 = matrix.M41;
            bepuMatrix.M42 = matrix.M42;
            bepuMatrix.M43 = matrix.M43;
            bepuMatrix.M44 = matrix.M44;

        }

        public static XnaMatrix Convert(BEPUMatrix3x3 matrix)
        {
            XnaMatrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

        public static void Convert(ref BEPUMatrix3x3 matrix, out XnaMatrix xnaMatrix)
        {
            xnaMatrix.M11 = matrix.M11;
            xnaMatrix.M12 = matrix.M12;
            xnaMatrix.M13 = matrix.M13;
            xnaMatrix.M14 = 0;

            xnaMatrix.M21 = matrix.M21;
            xnaMatrix.M22 = matrix.M22;
            xnaMatrix.M23 = matrix.M23;
            xnaMatrix.M24 = 0;

            xnaMatrix.M31 = matrix.M31;
            xnaMatrix.M32 = matrix.M32;
            xnaMatrix.M33 = matrix.M33;
            xnaMatrix.M34 = 0;

            xnaMatrix.M41 = 0;
            xnaMatrix.M42 = 0;
            xnaMatrix.M43 = 0;
            xnaMatrix.M44 = 1;
        }

        public static void Convert(ref XnaMatrix matrix, out BEPUMatrix3x3 bepuMatrix)
        {
            bepuMatrix.M11 = matrix.M11;
            bepuMatrix.M12 = matrix.M12;
            bepuMatrix.M13 = matrix.M13;

            bepuMatrix.M21 = matrix.M21;
            bepuMatrix.M22 = matrix.M22;
            bepuMatrix.M23 = matrix.M23;

            bepuMatrix.M31 = matrix.M31;
            bepuMatrix.M32 = matrix.M32;
            bepuMatrix.M33 = matrix.M33;

        }

        //Quaternion
        public static XnaQuaternion Convert(BEPUQuaternion quaternion)
        {
            XnaQuaternion toReturn;
            toReturn.X = quaternion.X;
            toReturn.Y = quaternion.Y;
            toReturn.Z = quaternion.Z;
            toReturn.W = quaternion.W;
            return toReturn;
        }

        public static BEPUQuaternion Convert(XnaQuaternion quaternion)
        {
            BEPUQuaternion toReturn;
            toReturn.X = quaternion.X;
            toReturn.Y = quaternion.Y;
            toReturn.Z = quaternion.Z;
            toReturn.W = quaternion.W;
            return toReturn;
        }

        public static void Convert(ref BEPUQuaternion bepuQuaternion, out XnaQuaternion quaternion)
        {
            quaternion.X = bepuQuaternion.X;
            quaternion.Y = bepuQuaternion.Y;
            quaternion.Z = bepuQuaternion.Z;
            quaternion.W = bepuQuaternion.W;
        }

        public static void Convert(ref XnaQuaternion quaternion, out  BEPUQuaternion bepuQuaternion)
        {
            bepuQuaternion.X = quaternion.X;
            bepuQuaternion.Y = quaternion.Y;
            bepuQuaternion.Z = quaternion.Z;
            bepuQuaternion.W = quaternion.W;
        }

        //Ray
        public static BEPURay Convert(XnaRay ray)
        {
            BEPURay toReturn;
            Convert(ref ray.Position, out toReturn.Position);
            Convert(ref ray.Direction, out toReturn.Direction);
            return toReturn;
        }

        public static void Convert(ref XnaRay ray, out BEPURay bepuRay)
        {
            Convert(ref ray.Position, out bepuRay.Position);
            Convert(ref ray.Direction, out bepuRay.Direction);
        }

        public static XnaRay Convert(BEPURay ray)
        {
            XnaRay toReturn;
            Convert(ref ray.Position, out toReturn.Position);
            Convert(ref ray.Direction, out toReturn.Direction);
            return toReturn;
        }

        public static void Convert(ref BEPURay ray, out XnaRay xnaRay)
        {
            Convert(ref ray.Position, out xnaRay.Position);
            Convert(ref ray.Direction, out xnaRay.Direction);
        }

        //BoundingBox
        public static XnaBoundingBox Convert(BEPUBoundingBox boundingBox)
        {
            XnaBoundingBox toReturn;
            Convert(ref boundingBox.Min, out toReturn.Min);
            Convert(ref boundingBox.Max, out toReturn.Max);
            return toReturn;
        }

        public static BEPUBoundingBox Convert(XnaBoundingBox boundingBox)
        {
            BEPUBoundingBox toReturn;
            Convert(ref boundingBox.Min, out toReturn.Min);
            Convert(ref boundingBox.Max, out toReturn.Max);
            return toReturn;
        }

        public static void Convert(ref BEPUBoundingBox boundingBox, out XnaBoundingBox xnaBoundingBox)
        {
            Convert(ref boundingBox.Min, out xnaBoundingBox.Min);
            Convert(ref boundingBox.Max, out xnaBoundingBox.Max);
        }

        public static void Convert(ref XnaBoundingBox boundingBox, out BEPUBoundingBox bepuBoundingBox)
        {
            Convert(ref boundingBox.Min, out bepuBoundingBox.Min);
            Convert(ref boundingBox.Max, out bepuBoundingBox.Max);
        }

        //BoundingSphere
        public static XnaBoundingSphere Convert(BEPUBoundingSphere boundingSphere)
        {
            XnaBoundingSphere toReturn;
            Convert(ref boundingSphere.Center, out toReturn.Center);
            toReturn.Radius = boundingSphere.Radius;
            return toReturn;
        }

        public static BEPUBoundingSphere Convert(XnaBoundingSphere boundingSphere)
        {
            BEPUBoundingSphere toReturn;
            Convert(ref boundingSphere.Center, out toReturn.Center);
            toReturn.Radius = boundingSphere.Radius;
            return toReturn;
        }

        public static void Convert(ref BEPUBoundingSphere boundingSphere, out XnaBoundingSphere xnaBoundingSphere)
        {
            Convert(ref boundingSphere.Center, out xnaBoundingSphere.Center);
            xnaBoundingSphere.Radius = boundingSphere.Radius;
        }

        public static void Convert(ref XnaBoundingSphere boundingSphere, out BEPUBoundingSphere bepuBoundingSphere)
        {
            Convert(ref boundingSphere.Center, out bepuBoundingSphere.Center);
            bepuBoundingSphere.Radius = boundingSphere.Radius;
        }

        //Plane
        public static XnaPlane Convert(BEPUPlane plane)
        {
            XnaPlane toReturn;
            Convert(ref plane.Normal, out toReturn.Normal);
            toReturn.D = plane.D;
            return toReturn;
        }

        public static BEPUPlane Convert(XnaPlane plane)
        {
            BEPUPlane toReturn;
            Convert(ref plane.Normal, out toReturn.Normal);
            toReturn.D = plane.D;
            return toReturn;
        }

        public static void Convert(ref BEPUPlane plane, out XnaPlane xnaPlane)
        {
            Convert(ref plane.Normal, out xnaPlane.Normal);
            xnaPlane.D = plane.D;
        }

        public static void Convert(ref XnaPlane plane, out BEPUPlane bepuPlane)
        {
            Convert(ref plane.Normal, out bepuPlane.Normal);
            bepuPlane.D = plane.D;
        }
    }
}