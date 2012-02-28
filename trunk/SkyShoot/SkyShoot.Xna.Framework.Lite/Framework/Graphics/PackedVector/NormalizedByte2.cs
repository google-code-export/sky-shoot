using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct NormalizedByte2 : IPackedVector<ushort>, IPackedVector, IEquatable<NormalizedByte2>
    {
        private ushort packedValue;
        public NormalizedByte2(float x, float y)
        {
            this.packedValue = PackHelper(x, y);
        }

        public NormalizedByte2(Vector2 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y);
        }

        private static ushort PackHelper(float vectorX, float vectorY)
        {
            uint num2 = PackUtils.PackSNorm(0xff, vectorX);
            uint num = PackUtils.PackSNorm(0xff, vectorY) << 8;
            return (ushort) (num2 | num);
        }

        public Vector2 ToVector2()
        {
            Vector2 vector;
            vector.X = PackUtils.UnpackSNorm(0xff, this.packedValue);
            vector.Y = PackUtils.UnpackSNorm(0xff, (uint) (this.packedValue >> 8));
            return vector;
        }

        Vector4 IPackedVector.ToVector4()
        {
            Vector2 vector = this.ToVector2();
            return new Vector4(vector.X, vector.Y, 0f, 1f);
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
            return ((obj is NormalizedByte2) && this.Equals((NormalizedByte2) obj));
        }

        public bool Equals(NormalizedByte2 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(NormalizedByte2 a, NormalizedByte2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NormalizedByte2 a, NormalizedByte2 b)
        {
            return !a.Equals(b);
        }
    }
}

