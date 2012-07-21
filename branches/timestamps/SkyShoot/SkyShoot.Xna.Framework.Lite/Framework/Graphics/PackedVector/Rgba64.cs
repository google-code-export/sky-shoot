using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Rgba64 : IPackedVector<ulong>, IPackedVector, IEquatable<Rgba64>
    {
        private ulong packedValue;
        public Rgba64(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public Rgba64(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static ulong PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            ulong num4 = PackUtils.PackUNorm(65535f, vectorX);
            ulong num3 = PackUtils.PackUNorm(65535f, vectorY) << 0x10;
            ulong num2 = PackUtils.PackUNorm(65535f, vectorZ) << 0x20;
            ulong num = PackUtils.PackUNorm(65535f, vectorW) << 0x30;
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackUNorm(0xffff, (uint) this.packedValue);
            vector.Y = PackUtils.UnpackUNorm(0xffff, (uint) (this.packedValue >> 0x10));
            vector.Z = PackUtils.UnpackUNorm(0xffff, (uint) (this.packedValue >> 0x20));
            vector.W = PackUtils.UnpackUNorm(0xffff, (uint) (this.packedValue >> 0x30));
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
            return ((obj is Rgba64) && this.Equals((Rgba64) obj));
        }

        public bool Equals(Rgba64 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Rgba64 a, Rgba64 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rgba64 a, Rgba64 b)
        {
            return !a.Equals(b);
        }
    }
}

