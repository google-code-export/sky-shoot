using SkyShoot.Contracts;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	class Remedy : AGameBonus
	{
		public Remedy(Vector2 coordinates)
			: base(coordinates)
		{
			ObjectType = EnumObjectType.Remedy;
			DamageFactor = 0.25f;
			_milliseconds = Constants.REMEDY_MILLISECONDS; // redundant
		}
	}
}
