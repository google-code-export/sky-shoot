namespace SkyShoot.Game
{
	static  class TypeConverter
	{
		static public SkyShoot.XNA.Framework.Vector2 Vector2_m2s(Microsoft.Xna.Framework.Vector2 v)
		{
			return new SkyShoot.XNA.Framework.Vector2(v.X, v.Y);
		}

		static public Microsoft.Xna.Framework.Vector2 Vector2_s2m(SkyShoot.XNA.Framework.Vector2 v)
		{
			return new Microsoft.Xna.Framework.Vector2(v.X, v.Y);
		}
	}
}
