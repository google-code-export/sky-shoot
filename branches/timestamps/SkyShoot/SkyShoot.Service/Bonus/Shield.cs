using SkyShoot.Contracts.Service;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Shield : AGameBonus
	{
		public Shield(Vector2 coordinates)
			: base(coordinates)
		{
			ObjectType = EnumObjectType.Shield;
			DamageFactor = 0.5f;
			Milliseconds = Constants.SHIELD_MILLISECONDS;
		}
	}
}