using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using SkyShoot.XNA.Framework.Design;
using SkyShoot.XNA.Framework.Design;
using SkyShoot.XNA.Framework.Graphics.PackedVector;
using SkyShoot.XNA.Framework.Graphics.PackedVector;

namespace SkyShoot.XNA.Framework
{
	[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(ColorConverter))]
    public struct Color : IPackedVector<uint>, IPackedVector, IEquatable<Color>
    {
        private uint packedValue;
        private Color(uint packedValue)
        {
            this.packedValue = packedValue;
        }

        public Color(int r, int g, int b)
        {
            if ((((r | g) | b) & -256) != 0)
            {
                r = ClampToByte64((long) r);
                g = ClampToByte64((long) g);
                b = ClampToByte64((long) b);
            }
            g = g << 8;
            b = b << 0x10;
            this.packedValue = (uint) (((r | g) | b) | -16777216);
        }

        public Color(int r, int g, int b, int a)
        {
            if (((((r | g) | b) | a) & -256) != 0)
            {
                r = ClampToByte32(r);
                g = ClampToByte32(g);
                b = ClampToByte32(b);
                a = ClampToByte32(a);
            }
            g = g << 8;
            b = b << 0x10;
            a = a << 0x18;
            this.packedValue = (uint) (((r | g) | b) | a);
        }

        public Color(float r, float g, float b)
        {
            this.packedValue = PackHelper(r, g, b, 1f);
        }

        public Color(float r, float g, float b, float a)
        {
            this.packedValue = PackHelper(r, g, b, a);
        }

        public Color(Vector3 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, 1f);
        }

        public Color(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            this.packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static Color FromNonPremultiplied(Vector4 vector)
        {
            Color color;
            color.packedValue = PackHelper(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W, vector.W);
            return color;
        }

        public static Color FromNonPremultiplied(int r, int g, int b, int a)
        {
            Color color;
            r = ClampToByte64((r * a) / 0xffL);
            g = ClampToByte64((g * a) / 0xffL);
            b = ClampToByte64((b * a) / 0xffL);
            a = ClampToByte32(a);
            g = g << 8;
            b = b << 0x10;
            a = a << 0x18;
            color.packedValue = (uint) (((r | g) | b) | a);
            return color;
        }

        private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackUNorm(255f, vectorX);
            uint num3 = PackUtils.PackUNorm(255f, vectorY) << 8;
            uint num2 = PackUtils.PackUNorm(255f, vectorZ) << 0x10;
            uint num = PackUtils.PackUNorm(255f, vectorW) << 0x18;
            return (((num4 | num3) | num2) | num);
        }

        private static int ClampToByte32(int value)
        {
            if (value < 0)
            {
                return 0;
            }
            if (value > 0xff)
            {
                return 0xff;
            }
            return value;
        }

        private static int ClampToByte64(long value)
        {
            if (value < 0L)
            {
                return 0;
            }
            if (value > 0xffL)
            {
                return 0xff;
            }
            return (int) value;
        }

        public Vector3 ToVector3()
        {
            Vector3 vector;
            vector.X = PackUtils.UnpackUNorm(0xff, this.packedValue);
            vector.Y = PackUtils.UnpackUNorm(0xff, this.packedValue >> 8);
            vector.Z = PackUtils.UnpackUNorm(0xff, this.packedValue >> 0x10);
            return vector;
        }

        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackUNorm(0xff, this.packedValue);
            vector.Y = PackUtils.UnpackUNorm(0xff, this.packedValue >> 8);
            vector.Z = PackUtils.UnpackUNorm(0xff, this.packedValue >> 0x10);
            vector.W = PackUtils.UnpackUNorm(0xff, this.packedValue >> 0x18);
            return vector;
        }

        public byte R
        {
            get
            {
                return (byte) this.packedValue;
            }
            set
            {
                this.packedValue = (this.packedValue & 0xffffff00) | value;
            }
        }
        public byte G
        {
            get
            {
                return (byte) (this.packedValue >> 8);
            }
            set
            {
                this.packedValue = (this.packedValue & 0xffff00ff) | ((uint) (value << 8));
            }
        }
        public byte B
        {
            get
            {
                return (byte) (this.packedValue >> 0x10);
            }
            set
            {
                this.packedValue = (this.packedValue & 0xff00ffff) | ((uint) (value << 0x10));
            }
        }
        public byte A
        {
            get
            {
                return (byte) (this.packedValue >> 0x18);
            }
            set
            {
                this.packedValue = (this.packedValue & 0xffffff) | ((uint) (value << 0x18));
            }
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
        public static Color Lerp(Color value1, Color value2, float amount)
        {
            Color color;
            uint packedValue = value1.packedValue;
            uint num2 = value2.packedValue;
            int num7 = (byte) packedValue;
            int num6 = (byte) (packedValue >> 8);
            int num5 = (byte) (packedValue >> 0x10);
            int num4 = (byte) (packedValue >> 0x18);
            int num15 = (byte) num2;
            int num14 = (byte) (num2 >> 8);
            int num13 = (byte) (num2 >> 0x10);
            int num12 = (byte) (num2 >> 0x18);
            int num = (int) PackUtils.PackUNorm(65536f, amount);
            int num11 = num7 + (((num15 - num7) * num) >> 0x10);
            int num10 = num6 + (((num14 - num6) * num) >> 0x10);
            int num9 = num5 + (((num13 - num5) * num) >> 0x10);
            int num8 = num4 + (((num12 - num4) * num) >> 0x10);
            color.packedValue = (uint) (((num11 | (num10 << 8)) | (num9 << 0x10)) | (num8 << 0x18));
            return color;
        }

        public static Color Multiply(Color value, float scale)
        {
            uint num;
            Color color;
            uint packedValue = value.packedValue;
            uint num5 = (byte) packedValue;
            uint num4 = (byte) (packedValue >> 8);
            uint num3 = (byte) (packedValue >> 0x10);
            uint num2 = (byte) (packedValue >> 0x18);
            scale *= 65536f;
            if (scale < 0f)
            {
                num = 0;
            }
            else if (scale > 1.677722E+07f)
            {
                num = 0xffffff;
            }
            else
            {
                num = (uint) scale;
            }
            num5 = (num5 * num) >> 0x10;
            num4 = (num4 * num) >> 0x10;
            num3 = (num3 * num) >> 0x10;
            num2 = (num2 * num) >> 0x10;
            if (num5 > 0xff)
            {
                num5 = 0xff;
            }
            if (num4 > 0xff)
            {
                num4 = 0xff;
            }
            if (num3 > 0xff)
            {
                num3 = 0xff;
            }
            if (num2 > 0xff)
            {
                num2 = 0xff;
            }
            color.packedValue = ((num5 | (num4 << 8)) | (num3 << 0x10)) | (num2 << 0x18);
            return color;
        }

        public static Color operator *(Color value, float scale)
        {
            uint num;
            Color color;
            uint packedValue = value.packedValue;
            uint num5 = (byte) packedValue;
            uint num4 = (byte) (packedValue >> 8);
            uint num3 = (byte) (packedValue >> 0x10);
            uint num2 = (byte) (packedValue >> 0x18);
            scale *= 65536f;
            if (scale < 0f)
            {
                num = 0;
            }
            else if (scale > 1.677722E+07f)
            {
                num = 0xffffff;
            }
            else
            {
                num = (uint) scale;
            }
            num5 = (num5 * num) >> 0x10;
            num4 = (num4 * num) >> 0x10;
            num3 = (num3 * num) >> 0x10;
            num2 = (num2 * num) >> 0x10;
            if (num5 > 0xff)
            {
                num5 = 0xff;
            }
            if (num4 > 0xff)
            {
                num4 = 0xff;
            }
            if (num3 > 0xff)
            {
                num3 = 0xff;
            }
            if (num2 > 0xff)
            {
                num2 = 0xff;
            }
            color.packedValue = ((num5 | (num4 << 8)) | (num3 << 0x10)) | (num2 << 0x18);
            return color;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", new object[] { this.R, this.G, this.B, this.A });
        }

        public override int GetHashCode()
        {
            return this.packedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is Color) && this.Equals((Color) obj));
        }

        public bool Equals(Color other)
        {
            return this.packedValue.Equals(other.packedValue);
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Color a, Color b)
        {
            return !a.Equals(b);
        }

        public static Color Transparent
        {
            get
            {
                return new Color(0);
            }
        }
        public static Color AliceBlue
        {
            get
            {
                return new Color(0xfffff8f0);
            }
        }
        public static Color AntiqueWhite
        {
            get
            {
                return new Color(0xffd7ebfa);
            }
        }
        public static Color Aqua
        {
            get
            {
                return new Color(0xffffff00);
            }
        }
        public static Color Aquamarine
        {
            get
            {
                return new Color(0xffd4ff7f);
            }
        }
        public static Color Azure
        {
            get
            {
                return new Color(0xfffffff0);
            }
        }
        public static Color Beige
        {
            get
            {
                return new Color(0xffdcf5f5);
            }
        }
        public static Color Bisque
        {
            get
            {
                return new Color(0xffc4e4ff);
            }
        }
        public static Color Black
        {
            get
            {
                return new Color(0xff000000);
            }
        }
        public static Color BlanchedAlmond
        {
            get
            {
                return new Color(0xffcdebff);
            }
        }
        public static Color Blue
        {
            get
            {
                return new Color(0xffff0000);
            }
        }
        public static Color BlueViolet
        {
            get
            {
                return new Color(0xffe22b8a);
            }
        }
        public static Color Brown
        {
            get
            {
                return new Color(0xff2a2aa5);
            }
        }
        public static Color BurlyWood
        {
            get
            {
                return new Color(0xff87b8de);
            }
        }
        public static Color CadetBlue
        {
            get
            {
                return new Color(0xffa09e5f);
            }
        }
        public static Color Chartreuse
        {
            get
            {
                return new Color(0xff00ff7f);
            }
        }
        public static Color Chocolate
        {
            get
            {
                return new Color(0xff1e69d2);
            }
        }
        public static Color Coral
        {
            get
            {
                return new Color(0xff507fff);
            }
        }
        public static Color CornflowerBlue
        {
            get
            {
                return new Color(0xffed9564);
            }
        }
        public static Color Cornsilk
        {
            get
            {
                return new Color(0xffdcf8ff);
            }
        }
        public static Color Crimson
        {
            get
            {
                return new Color(0xff3c14dc);
            }
        }
        public static Color Cyan
        {
            get
            {
                return new Color(0xffffff00);
            }
        }
        public static Color DarkBlue
        {
            get
            {
                return new Color(0xff8b0000);
            }
        }
        public static Color DarkCyan
        {
            get
            {
                return new Color(0xff8b8b00);
            }
        }
        public static Color DarkGoldenrod
        {
            get
            {
                return new Color(0xff0b86b8);
            }
        }
        public static Color DarkGray
        {
            get
            {
                return new Color(0xffa9a9a9);
            }
        }
        public static Color DarkGreen
        {
            get
            {
                return new Color(0xff006400);
            }
        }
        public static Color DarkKhaki
        {
            get
            {
                return new Color(0xff6bb7bd);
            }
        }
        public static Color DarkMagenta
        {
            get
            {
                return new Color(0xff8b008b);
            }
        }
        public static Color DarkOliveGreen
        {
            get
            {
                return new Color(0xff2f6b55);
            }
        }
        public static Color DarkOrange
        {
            get
            {
                return new Color(0xff008cff);
            }
        }
        public static Color DarkOrchid
        {
            get
            {
                return new Color(0xffcc3299);
            }
        }
        public static Color DarkRed
        {
            get
            {
                return new Color(0xff00008b);
            }
        }
        public static Color DarkSalmon
        {
            get
            {
                return new Color(0xff7a96e9);
            }
        }
        public static Color DarkSeaGreen
        {
            get
            {
                return new Color(0xff8bbc8f);
            }
        }
        public static Color DarkSlateBlue
        {
            get
            {
                return new Color(0xff8b3d48);
            }
        }
        public static Color DarkSlateGray
        {
            get
            {
                return new Color(0xff4f4f2f);
            }
        }
        public static Color DarkTurquoise
        {
            get
            {
                return new Color(0xffd1ce00);
            }
        }
        public static Color DarkViolet
        {
            get
            {
                return new Color(0xffd30094);
            }
        }
        public static Color DeepPink
        {
            get
            {
                return new Color(0xff9314ff);
            }
        }
        public static Color DeepSkyBlue
        {
            get
            {
                return new Color(0xffffbf00);
            }
        }
        public static Color DimGray
        {
            get
            {
                return new Color(0xff696969);
            }
        }
        public static Color DodgerBlue
        {
            get
            {
                return new Color(0xffff901e);
            }
        }
        public static Color Firebrick
        {
            get
            {
                return new Color(0xff2222b2);
            }
        }
        public static Color FloralWhite
        {
            get
            {
                return new Color(0xfff0faff);
            }
        }
        public static Color ForestGreen
        {
            get
            {
                return new Color(0xff228b22);
            }
        }
        public static Color Fuchsia
        {
            get
            {
                return new Color(0xffff00ff);
            }
        }
        public static Color Gainsboro
        {
            get
            {
                return new Color(0xffdcdcdc);
            }
        }
        public static Color GhostWhite
        {
            get
            {
                return new Color(0xfffff8f8);
            }
        }
        public static Color Gold
        {
            get
            {
                return new Color(0xff00d7ff);
            }
        }
        public static Color Goldenrod
        {
            get
            {
                return new Color(0xff20a5da);
            }
        }
        public static Color Gray
        {
            get
            {
                return new Color(0xff808080);
            }
        }
        public static Color Green
        {
            get
            {
                return new Color(0xff008000);
            }
        }
        public static Color GreenYellow
        {
            get
            {
                return new Color(0xff2fffad);
            }
        }
        public static Color Honeydew
        {
            get
            {
                return new Color(0xfff0fff0);
            }
        }
        public static Color HotPink
        {
            get
            {
                return new Color(0xffb469ff);
            }
        }
        public static Color IndianRed
        {
            get
            {
                return new Color(0xff5c5ccd);
            }
        }
        public static Color Indigo
        {
            get
            {
                return new Color(0xff82004b);
            }
        }
        public static Color Ivory
        {
            get
            {
                return new Color(0xfff0ffff);
            }
        }
        public static Color Khaki
        {
            get
            {
                return new Color(0xff8ce6f0);
            }
        }
        public static Color Lavender
        {
            get
            {
                return new Color(0xfffae6e6);
            }
        }
        public static Color LavenderBlush
        {
            get
            {
                return new Color(0xfff5f0ff);
            }
        }
        public static Color LawnGreen
        {
            get
            {
                return new Color(0xff00fc7c);
            }
        }
        public static Color LemonChiffon
        {
            get
            {
                return new Color(0xffcdfaff);
            }
        }
        public static Color LightBlue
        {
            get
            {
                return new Color(0xffe6d8ad);
            }
        }
        public static Color LightCoral
        {
            get
            {
                return new Color(0xff8080f0);
            }
        }
        public static Color LightCyan
        {
            get
            {
                return new Color(0xffffffe0);
            }
        }
        public static Color LightGoldenrodYellow
        {
            get
            {
                return new Color(0xffd2fafa);
            }
        }
        public static Color LightGreen
        {
            get
            {
                return new Color(0xff90ee90);
            }
        }
        public static Color LightGray
        {
            get
            {
                return new Color(0xffd3d3d3);
            }
        }
        public static Color LightPink
        {
            get
            {
                return new Color(0xffc1b6ff);
            }
        }
        public static Color LightSalmon
        {
            get
            {
                return new Color(0xff7aa0ff);
            }
        }
        public static Color LightSeaGreen
        {
            get
            {
                return new Color(0xffaab220);
            }
        }
        public static Color LightSkyBlue
        {
            get
            {
                return new Color(0xffface87);
            }
        }
        public static Color LightSlateGray
        {
            get
            {
                return new Color(0xff998877);
            }
        }
        public static Color LightSteelBlue
        {
            get
            {
                return new Color(0xffdec4b0);
            }
        }
        public static Color LightYellow
        {
            get
            {
                return new Color(0xffe0ffff);
            }
        }
        public static Color Lime
        {
            get
            {
                return new Color(0xff00ff00);
            }
        }
        public static Color LimeGreen
        {
            get
            {
                return new Color(0xff32cd32);
            }
        }
        public static Color Linen
        {
            get
            {
                return new Color(0xffe6f0fa);
            }
        }
        public static Color Magenta
        {
            get
            {
                return new Color(0xffff00ff);
            }
        }
        public static Color Maroon
        {
            get
            {
                return new Color(0xff000080);
            }
        }
        public static Color MediumAquamarine
        {
            get
            {
                return new Color(0xffaacd66);
            }
        }
        public static Color MediumBlue
        {
            get
            {
                return new Color(0xffcd0000);
            }
        }
        public static Color MediumOrchid
        {
            get
            {
                return new Color(0xffd355ba);
            }
        }
        public static Color MediumPurple
        {
            get
            {
                return new Color(0xffdb7093);
            }
        }
        public static Color MediumSeaGreen
        {
            get
            {
                return new Color(0xff71b33c);
            }
        }
        public static Color MediumSlateBlue
        {
            get
            {
                return new Color(0xffee687b);
            }
        }
        public static Color MediumSpringGreen
        {
            get
            {
                return new Color(0xff9afa00);
            }
        }
        public static Color MediumTurquoise
        {
            get
            {
                return new Color(0xffccd148);
            }
        }
        public static Color MediumVioletRed
        {
            get
            {
                return new Color(0xff8515c7);
            }
        }
        public static Color MidnightBlue
        {
            get
            {
                return new Color(0xff701919);
            }
        }
        public static Color MintCream
        {
            get
            {
                return new Color(0xfffafff5);
            }
        }
        public static Color MistyRose
        {
            get
            {
                return new Color(0xffe1e4ff);
            }
        }
        public static Color Moccasin
        {
            get
            {
                return new Color(0xffb5e4ff);
            }
        }
        public static Color NavajoWhite
        {
            get
            {
                return new Color(0xffaddeff);
            }
        }
        public static Color Navy
        {
            get
            {
                return new Color(0xff800000);
            }
        }
        public static Color OldLace
        {
            get
            {
                return new Color(0xffe6f5fd);
            }
        }
        public static Color Olive
        {
            get
            {
                return new Color(0xff008080);
            }
        }
        public static Color OliveDrab
        {
            get
            {
                return new Color(0xff238e6b);
            }
        }
        public static Color Orange
        {
            get
            {
                return new Color(0xff00a5ff);
            }
        }
        public static Color OrangeRed
        {
            get
            {
                return new Color(0xff0045ff);
            }
        }
        public static Color Orchid
        {
            get
            {
                return new Color(0xffd670da);
            }
        }
        public static Color PaleGoldenrod
        {
            get
            {
                return new Color(0xffaae8ee);
            }
        }
        public static Color PaleGreen
        {
            get
            {
                return new Color(0xff98fb98);
            }
        }
        public static Color PaleTurquoise
        {
            get
            {
                return new Color(0xffeeeeaf);
            }
        }
        public static Color PaleVioletRed
        {
            get
            {
                return new Color(0xff9370db);
            }
        }
        public static Color PapayaWhip
        {
            get
            {
                return new Color(0xffd5efff);
            }
        }
        public static Color PeachPuff
        {
            get
            {
                return new Color(0xffb9daff);
            }
        }
        public static Color Peru
        {
            get
            {
                return new Color(0xff3f85cd);
            }
        }
        public static Color Pink
        {
            get
            {
                return new Color(0xffcbc0ff);
            }
        }
        public static Color Plum
        {
            get
            {
                return new Color(0xffdda0dd);
            }
        }
        public static Color PowderBlue
        {
            get
            {
                return new Color(0xffe6e0b0);
            }
        }
        public static Color Purple
        {
            get
            {
                return new Color(0xff800080);
            }
        }
        public static Color Red
        {
            get
            {
                return new Color(0xff0000ff);
            }
        }
        public static Color RosyBrown
        {
            get
            {
                return new Color(0xff8f8fbc);
            }
        }
        public static Color RoyalBlue
        {
            get
            {
                return new Color(0xffe16941);
            }
        }
        public static Color SaddleBrown
        {
            get
            {
                return new Color(0xff13458b);
            }
        }
        public static Color Salmon
        {
            get
            {
                return new Color(0xff7280fa);
            }
        }
        public static Color SandyBrown
        {
            get
            {
                return new Color(0xff60a4f4);
            }
        }
        public static Color SeaGreen
        {
            get
            {
                return new Color(0xff578b2e);
            }
        }
        public static Color SeaShell
        {
            get
            {
                return new Color(0xffeef5ff);
            }
        }
        public static Color Sienna
        {
            get
            {
                return new Color(0xff2d52a0);
            }
        }
        public static Color Silver
        {
            get
            {
                return new Color(0xffc0c0c0);
            }
        }
        public static Color SkyBlue
        {
            get
            {
                return new Color(0xffebce87);
            }
        }
        public static Color SlateBlue
        {
            get
            {
                return new Color(0xffcd5a6a);
            }
        }
        public static Color SlateGray
        {
            get
            {
                return new Color(0xff908070);
            }
        }
        public static Color Snow
        {
            get
            {
                return new Color(0xfffafaff);
            }
        }
        public static Color SpringGreen
        {
            get
            {
                return new Color(0xff7fff00);
            }
        }
        public static Color SteelBlue
        {
            get
            {
                return new Color(0xffb48246);
            }
        }
        public static Color Tan
        {
            get
            {
                return new Color(0xff8cb4d2);
            }
        }
        public static Color Teal
        {
            get
            {
                return new Color(0xff808000);
            }
        }
        public static Color Thistle
        {
            get
            {
                return new Color(0xffd8bfd8);
            }
        }
        public static Color Tomato
        {
            get
            {
                return new Color(0xff4763ff);
            }
        }
        public static Color Turquoise
        {
            get
            {
                return new Color(0xffd0e040);
            }
        }
        public static Color Violet
        {
            get
            {
                return new Color(0xffee82ee);
            }
        }
        public static Color Wheat
        {
            get
            {
                return new Color(0xffb3def5);
            }
        }
        public static Color White
        {
            get
            {
                return new Color(uint.MaxValue);
            }
        }
        public static Color WhiteSmoke
        {
            get
            {
                return new Color(0xfff5f5f5);
            }
        }
        public static Color Yellow
        {
            get
            {
                return new Color(0xff00ffff);
            }
        }
        public static Color YellowGreen
        {
            get
            {
                return new Color(0xff32cd9a);
            }
        }
    }
}

