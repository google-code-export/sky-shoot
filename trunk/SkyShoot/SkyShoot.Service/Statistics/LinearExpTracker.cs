using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Statistics;

namespace SkyShoot.Service.Statistics
{
	class LinearExpTracker : ExpTracker
	{
		public override void AddExp(AGameObject owner, AGameObject wounded, int damage)
		{

			if (wounded.Is(AGameObject.EnumObjectType.Player) && wounded.HealthAmount < 0.1) if (owner != null) Value.Frag += 1;
			if (wounded.HealthAmount < 0.1) if (owner != null) Value.Exp += (int)wounded.MaxHealthAmount;
			if (owner != null) Value.Exp += damage;
			// Получение уровня
			if (Value.Exp >= 100 * Value.Lvl)
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}
	}
}
