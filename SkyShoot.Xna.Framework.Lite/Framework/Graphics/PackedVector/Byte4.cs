using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Byte4 : IPackedVector<uint>, IPackedVector, IEquatable<Byte4>
    {
        private uint packedValue;
        public Byte4(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public Byte4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackUnsigned(255f, vectorX);
            uint num3 = PackUtils.PackUnsigned(255f, vectorY) << 8;
            uint num2 = PackUtils.PackUnsigned(255f, vectorZ) << 0x10;
            uint num = PackUtils.PackUnsigned(255f, vectorW) << 0x18;
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = this.packedValue & 0xff;
            vector.Y = (this.packedValue >> 8) & 0xff;
            vector.Z = (this.packedValue >> 0x10) & 0xff;
            vector.W = (this.packedValue >> 0x18) & 0xff;
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
            return ((obj is Byte4) && this.Equals((Byte4) obj));
        }

        public bool Equals(Byte4 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Byte4 a, Byte4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Byte4 a, Byte4 b)
        {
            return !a.Equals(b);
        }
    }
}

