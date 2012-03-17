using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Bgr565 : IPackedVector<ushort>, IPackedVector, IEquatable<Bgr565>
    {
        private ushort packedValue;
        public Bgr565(float x, float y, float z)
        {
            this.packedValue = PackHelper(x, y, z);
        }

        public Bgr565(Vector3 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z);
        }

        private static ushort PackHelper(float vectorX, float vectorY, float vectorZ)
        {
            uint num3 = PackUtils.PackUNorm(31f, vectorX) << 11;
            uint num2 = PackUtils.PackUNorm(63f, vectorY) << 5;
            uint num = PackUtils.PackUNorm(31f, vectorZ);
            return (ushort) ((num3 | num2) | num);
        }

        public Vector3 ToVector3()
        {
            Vector3 vector;
            vector.X = PackUtils.UnpackUNorm(0x1f, (uint) (this.packedValue >> 11));
            vector.Y = PackUtils.UnpackUNorm(0x3f, (uint) (this.packedValue >> 5));
            vector.Z = PackUtils.UnpackUNorm(0x1f, this.packedValue);
            return vector;
        }

        Vector4 IPackedVector.ToVector4()
        {
            Vector3 vector = this.ToVector3();
            return new Vector4(vector.X, vector.Y, vector.Z, 1f);
        }

        [CLSCompliant(false)]
        public ushort PackedValue
        {
            get
            {
                return this.packedValue;
            }
            set
            {
                this.packedValue = value;
            }
        }
        public override string ToString()
        {
            return this.packedValue.ToString("X4", CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return this.packedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is Bgr565) && this.Equals((Bgr565) obj));
        }

        public bool Equals(Bgr565 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Bgr565 a, Bgr565 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Bgr565 a, Bgr565 b)
        {
            return !a.Equals(b);
        }
    }
}

