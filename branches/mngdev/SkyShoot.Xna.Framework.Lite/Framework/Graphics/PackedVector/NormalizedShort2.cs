using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct NormalizedShort2 : IPackedVector<uint>, IPackedVector, IEquatable<NormalizedShort2>
    {
        private uint packedValue;
        public NormalizedShort2(float x, float y)
        {
            this.packedValue = PackHelper(x, y);
        }

        public NormalizedShort2(Vector2 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        private static uint PackHelper(float vectorX, float vectorY)
        {
            uint num2 = PackUtils.PackSNorm(0xffff, vectorX);
            uint num = PackUtils.PackSNorm(0xffff, vectorY) << 0x10;
            return (num2 | num);
        }

        public Vector2 ToVector2()
        {
            Vector2 vector;
            vector.X = PackUtils.UnpackSNorm(0xffff, this.packedValue);
            vector.Y = PackUtils.UnpackSNorm(0xffff, this.packedValue >> 0x10);
            return vector;
        }

        Vector4 IPackedVector.ToVector4()
        {
            Vector2 vector = this.ToVector2();
            return new Vector4(vector.X, vector.Y, 0f, 1f);
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
            return ((obj is NormalizedShort2) && this.Equals((NormalizedShort2) obj));
        }

        public bool Equals(NormalizedShort2 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(NormalizedShort2 a, NormalizedShort2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NormalizedShort2 a, NormalizedShort2 b)
        {
            return !a.Equals(b);
        }
    }
}

