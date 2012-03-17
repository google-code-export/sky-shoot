using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Short4 : IPackedVector<ulong>, IPackedVector, IEquatable<Short4>
    {
        private ulong packedValue;
        public Short4(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public Short4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static ulong PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            ulong num4 = PackUtils.PackSigned(0xffff, vectorX);
            ulong num3 = PackUtils.PackSigned(0xffff, vectorY) << 0x10;
            ulong num2 = PackUtils.PackSigned(0xffff, vectorZ) << 0x20;
            ulong num = PackUtils.PackSigned(0xffff, vectorW) << 0x30;
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = (short) this.packedValue;
            vector.Y = (short) (this.packedValue >> 0x10);
            vector.Z = (short) (this.packedValue >> 0x20);
            vector.W = (short) (this.packedValue >> 0x30);
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
            return ((obj is Short4) && this.Equals((Short4) obj));
        }

        public bool Equals(Short4 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Short4 a, Short4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Short4 a, Short4 b)
        {
            return !a.Equals(b);
        }
    }
}

