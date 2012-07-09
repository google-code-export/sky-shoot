using SkyShoot.Contracts;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Mirror : AGameBonus
	{
		public Mirror(Vector2 coordinates) : base(coordinates)
		{
			ObjectType = EnumObjectType.Mirror;
			DamageFactor = 0f; // we don't use it
			Milliseconds = Constants.MIRROR_MILLISECONDS;
		}
	}
}
