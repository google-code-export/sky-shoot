using SkyShoot.Contracts;
using SkyShoot.Contracts.Service;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class DoubleDamage : AGameBonus
	{
		//public DoubleDamage(Guid id, DateTime startTime)
		//    : base(id, 1, 2, 30000, startTime,AObtainableDamageModifiers.DoubleDamage)  {  }
		public DoubleDamage(Vector2 coordinates) : base(coordinates)
		{
			ObjectType = EnumObjectType.DoubleDamage;
			DamageFactor = 2f;
			Milliseconds = Constants.DOUBLE_DAMAGE_MILLISECONDS;
		}
	}
}