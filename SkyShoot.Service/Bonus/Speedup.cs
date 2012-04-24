using SkyShoot.Contracts;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Speedup : AGameBonus
	{
		public Speedup(Vector2 coordinates)
			: base(coordinates)
		{
			Milliseconds = Constants.SPEEDUP_MILLISECONDS;
			ObjectType = EnumObjectType.Speedup;
			DamageFactor = 1.5f; // not damage, but speedup here
		}
	}
}
