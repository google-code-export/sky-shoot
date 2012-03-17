using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Bgra5551 : IPackedVector<ushort>, IPackedVector, IEquatable<Bgra5551>
    {
        private ushort packedValue;
        public Bgra5551(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public Bgra5551(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static ushort PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackUNorm(31f, vectorX) << 10;
            uint num3 = PackUtils.PackUNorm(31f, vectorY) << 5;
            uint num2 = PackUtils.PackUNorm(31f, vectorZ);
            uint num = PackUtils.PackUNorm(1f, vectorW) << 15;
            return (ushort) (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackUNorm(0x1f, (uint) (this.packedValue >> 10));
            vector.Y = PackUtils.UnpackUNorm(0x1f, (uint) (this.packedValue >> 5));
            vector.Z = PackUtils.UnpackUNorm(0x1f, this.packedValue);
            vector.W = PackUtils.UnpackUNorm(1, (uint) (this.packedValue >> 15));
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
            return ((obj is Bgra5551) && this.Equals((Bgra5551) obj));
        }

        public bool Equals(Bgra5551 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Bgra5551 a, Bgra5551 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Bgra5551 a, Bgra5551 b)
        {
            return !a.Equals(b);
        }
    }
}

