using System;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct HalfVector4 : IPackedVector<ulong>, IPackedVector, IEquatable<HalfVector4>
    {
        private ulong packedValue;
        public HalfVector4(float x, float y, float z, float w)
        {
            this.packedValue = PackHelper(x, y, z, w);
        }

        public HalfVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        private static ulong PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            ulong num4 = HalfUtils.Pack(vectorX);
            ulong num3 = (ulong) (HalfUtils.Pack(vectorY) << 0x10);
            ulong num2 = (ulong) (HalfUtils.Pack(vectorZ) << 0x20);
            ulong num = (ulong) (HalfUtils.Pack(vectorW) << 0x30);
            return (((num4 | num3) | num2) | num);
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = HalfUtils.Unpack((ushort) this.packedValue);
            vector.Y = HalfUtils.Unpack((ushort) (this.packedValue >> 0x10));
            vector.Z = HalfUtils.Unpack((ushort) (this.packedValue >> 0x20));
            vector.W = HalfUtils.Unpack((ushort) (this.packedValue >> 0x30));
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
            return this.ToVector4().ToString();
        }

        public override int GetHashCode()
        {
            return this.packedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is HalfVector4) && this.Equals((HalfVector4) obj));
        }

        public bool Equals(HalfVector4 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(HalfVector4 a, HalfVector4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(HalfVector4 a, HalfVector4 b)
        {
            return !a.Equals(b);
        }
    }
}

