namespace SkyShoot.Game.Utils
{
	static class TypeConverter
	{
		static public XNA.Framework.Vector2 Xna2XnaLite(Microsoft.Xna.Framework.Vector2 v)
		{
			return new XNA.Framework.Vector2(v.X, v.Y);
		}

		static public Microsoft.Xna.Framework.Vector2 XnaLite2Xna(XNA.Framework.Vector2 v)
		{
			return new Microsoft.Xna.Framework.Vector2(v.X, v.Y);
		}
	}
}
