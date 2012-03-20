using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct NormalizedByte4 : IPackedVector<uint>, IPackedVector, IEquatable<NormalizedByte4>
    {
        private uint packedValue;
        public NormalizedByte4(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public NormalizedByte4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackSNorm(0xff, vectorX);
            uint num3 = PackUtils.PackSNorm(0xff, vectorY) << 8;
            uint num2 = PackUtils.PackSNorm(0xff, vectorZ) << 0x10;
            uint num = PackUtils.PackSNorm(0xff, vectorW) << 0x18;
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackSNorm(0xff, this.packedValue);
            vector.Y = PackUtils.UnpackSNorm(0xff, this.packedValue >> 8);
            vector.Z = PackUtils.UnpackSNorm(0xff, this.packedValue >> 0x10);
            vector.W = PackUtils.UnpackSNorm(0xff, this.packedValue >> 0x18);
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
            return ((obj is NormalizedByte4) && this.Equals((NormalizedByte4) obj));
        }

        public bool Equals(NormalizedByte4 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(NormalizedByte4 a, NormalizedByte4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NormalizedByte4 a, NormalizedByte4 b)
        {
            return !a.Equals(b);
        }
    }
}

