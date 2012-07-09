using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Bgra4444 : IPackedVector<ushort>, IPackedVector, IEquatable<Bgra4444>
    {
        private ushort packedValue;
        public Bgra4444(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public Bgra4444(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static ushort PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackUNorm(15f, vectorX) << 8;
            uint num3 = PackUtils.PackUNorm(15f, vectorY) << 4;
            uint num2 = PackUtils.PackUNorm(15f, vectorZ);
            uint num = PackUtils.PackUNorm(15f, vectorW) << 12;
            return (ushort) (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackUNorm(15, (uint) (this.packedValue >> 8));
            vector.Y = PackUtils.UnpackUNorm(15, (uint) (this.packedValue >> 4));
            vector.Z = PackUtils.UnpackUNorm(15, this.packedValue);
            vector.W = PackUtils.UnpackUNorm(15, (uint) (this.packedValue >> 12));
            return vector;
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
            return ((obj is Bgra4444) && this.Equals((Bgra4444) obj));
        }

        public bool Equals(Bgra4444 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Bgra4444 a, Bgra4444 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Bgra4444 a, Bgra4444 b)
        {
            return !a.Equals(b);
        }
    }
}

