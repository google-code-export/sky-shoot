using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct NormalizedShort4 : IPackedVector<ulong>, IPackedVector, IEquatable<NormalizedShort4>
    {
        private ulong packedValue;
        public NormalizedShort4(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public NormalizedShort4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static ulong PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            ulong num4 = PackUtils.PackSNorm(0xffff, vectorX);
            ulong num3 = PackUtils.PackSNorm(0xffff, vectorY) << 0x10;
            ulong num2 = PackUtils.PackSNorm(0xffff, vectorZ) << 0x20;
            ulong num = PackUtils.PackSNorm(0xffff, vectorW) << 0x30;
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackSNorm(0xffff, (uint) this.packedValue);
            vector.Y = PackUtils.UnpackSNorm(0xffff, (uint) (this.packedValue >> 0x10));
            vector.Z = PackUtils.UnpackSNorm(0xffff, (uint) (this.packedValue >> 0x20));
            vector.W = PackUtils.UnpackSNorm(0xffff, (uint) (this.packedValue >> 0x30));
            return vector;
        }

        [CLSCompliant(false)]
        public ulong PackedValue
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
            return this.packedValue.ToString("X16", CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return this.packedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is NormalizedShort4) && this.Equals((NormalizedShort4) obj));
        }

        public bool Equals(NormalizedShort4 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(NormalizedShort4 a, NormalizedShort4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NormalizedShort4 a, NormalizedShort4 b)
        {
            return !a.Equals(b);
        }
    }
}

