using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Statistics;

namespace SkyShoot.Service.Statistics
{
	class LinearExpTracker : ExpTracker
	{
		public override void AddExpPlayer(AGameObject owner, AGameObject wounded, int damage)
		{

			if (wounded.Is(AGameObject.EnumObjectType.Player) && wounded.HealthAmount < 0.1) if (owner != null) Value.Frag += 1;
			// Опыт всем игрокам
			if (wounded.HealthAmount < 0.1) if (owner != null)
				{
					Value.Creeps += 1;
					Value.Exp += (int)wounded.MaxHealthAmount/2;
				}

			if (owner != null) Value.Exp += damage/2;
			// Получение уровня
			if (Value.Exp >= 100 * Value.Lvl)
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}
		public override void AddExpTeam(AGameObject player, AGameObject wounded, int damage)
		{
			if (player != null) Value.Exp += damage/2;

			if (wounded.HealthAmount < 0.1) if (player != null)
			{
				Value.Exp += (int)wounded.MaxHealthAmount/2;
			}

			if (Value.Exp >= 100 * Value.Lvl)
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}
	}
}
