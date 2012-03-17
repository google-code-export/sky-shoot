namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	public interface IPackedVector
    {
        void PackFromVector4(Vector4 vector);
        Vector4 ToVector4();
    }
}

