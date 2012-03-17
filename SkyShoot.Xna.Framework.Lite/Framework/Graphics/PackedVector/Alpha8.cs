using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Alpha8 : IPackedVector<byte>, IPackedVector, IEquatable<Alpha8>
    {
        private byte packedValue;
        public Alpha8(float alpha)
        {
            this.packedValue = PackHelper(alpha);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.W);
        }

        private static byte PackHelper(float alpha)
        {
            return (byte) PackUtils.PackUNorm(255f, alpha);
        }

        public float ToAlpha()
        {
            return PackUtils.UnpackUNorm(0xff, this.packedValue);
        }

        Vector4 IPackedVector.ToVector4()
        {
            return new Vector4(0f, 0f, 0f, this.ToAlpha());
        }

        public byte PackedValue
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
            return this.packedValue.ToString("X2", CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return this.packedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is Alpha8) && this.Equals((Alpha8) obj));
        }

        public bool Equals(Alpha8 other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Alpha8 a, Alpha8 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Alpha8 a, Alpha8 b)
        {
            return !a.Equals(b);
        }
    }
}

