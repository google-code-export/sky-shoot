using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Short2 : IPackedVector<uint>, IPackedVector, IEquatable<Short2>
    {
        private uint packedValue;
        public Short2(float x, float y)
        {
            this.packedValue = PackHelper(x, y);
        }

        public Short2(Vector2 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        private static uint PackHelper(float vectorX, float vectorY)
        {
            uint num2 = PackUtils.PackSigned(0xffff, vectorX);
            uint num = PackUtils.PackSigned(0xffff, vectorY) << 0x10;
            return (num2 | num);
        }

        public Vector2 ToVector2()
        {
            Vector2 vector;
            vector.X = (short) this.packedValue;
            vector.Y = (short) (this.packedValue >> 0x10);
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
            return ((obj is Short2) && this.Equals((Short2) obj));
        }

        public bool Equals(Short2 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Short2 a, Short2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Short2 a, Short2 b)
        {
            return !a.Equals(b);
        }
    }
}

