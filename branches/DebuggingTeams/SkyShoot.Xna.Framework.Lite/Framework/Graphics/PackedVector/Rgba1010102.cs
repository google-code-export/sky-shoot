using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Rgba1010102 : IPackedVector<uint>, IPackedVector, IEquatable<Rgba1010102>
    {
        private uint packedValue;
        public Rgba1010102(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public Rgba1010102(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackUNorm(1023f, vectorX);
            uint num3 = PackUtils.PackUNorm(1023f, vectorY) << 10;
            uint num2 = PackUtils.PackUNorm(1023f, vectorZ) << 20;
            uint num = PackUtils.PackUNorm(3f, vectorW) << 30;
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackUNorm(0x3ff, this.packedValue);
            vector.Y = PackUtils.UnpackUNorm(0x3ff, this.packedValue >> 10);
            vector.Z = PackUtils.UnpackUNorm(0x3ff, this.packedValue >> 20);
            vector.W = PackUtils.UnpackUNorm(3, this.packedValue >> 30);
            return vector;
        }

        [CLSCompliant(false)]
        public uint PackedValue
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
            return this.packedValue.ToString("X8", CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return this.packedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is Rgba1010102) && this.Equals((Rgba1010102) obj));
        }

        public bool Equals(Rgba1010102 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Rgba1010102 a, Rgba1010102 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rgba1010102 a, Rgba1010102 b)
        {
            return !a.Equals(b);
        }
    }
}

