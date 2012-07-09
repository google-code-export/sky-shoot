namespace SkyShoot.XNA.Framework.Graphics.PackedVector
{
	public interface IPackedVector<TPacked> : IPackedVector
    {
        TPacked PackedValue { get; set; }
    }
}

