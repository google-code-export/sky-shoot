using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Rg32 : IPackedVector<uint>, IPackedVector, IEquatable<Rg32>
    {
        private uint packedValue;
        public Rg32(float x, float y)
        {
            this.packedValue = PackHelper(x, y);
        }

        public Rg32(Vector2 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        private static uint PackHelper(float vectorX, float vectorY)
        {
            uint num2 = PackUtils.PackUNorm(65535f, vectorX);
            uint num = PackUtils.PackUNorm(65535f, vectorY) << 0x10;
            return (num2 | num);
        }

        public Vector2 ToVector2()
        {
            Vector2 vector;
            vector.X = PackUtils.UnpackUNorm(0xffff, this.packedValue);
            vector.Y = PackUtils.UnpackUNorm(0xffff, this.packedValue >> 0x10);
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
            return ((obj is Rg32) && this.Equals((Rg32) obj));
        }

        public bool Equals(Rg32 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Rg32 a, Rg32 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rg32 a, Rg32 b)
        {
            return !a.Equals(b);
        }
    }
}

