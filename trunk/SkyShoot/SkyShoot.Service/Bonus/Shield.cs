using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts;
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
			this.milliseconds = Constants.SHIELD_MILLISECONDS;
		}
	}
}