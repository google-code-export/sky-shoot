using SkyShoot.Contracts;
using SkyShoot.Contracts.Service;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Shield : AGameBonus
	{
		//public Shield(Guid id, DateTime startTime)
		//    : base(id, 0, 1, 30000, startTime, AObtainableDamageModifier.AObtainableDamageModifiers.Shield) {	}
		public Shield(Vector2 coordinates) : base(coordinates)
		{
			ObjectType = EnumObjectType.Shield;
			DamageFactor = 0.5f;
			Milliseconds = Constants.SHIELD_MILLISECONDS;
		}
	}
}