using SkyShoot.Contracts.GameObject;

namespace SkyShoot.Service.Statistics
{
	class ExponentialExpTracker : ExpTracker
	{
		public override void AddExpPlayer(AGameObject owner, AGameObject wounded, int damage)
		{
			if (wounded.Is(AGameObject.EnumObjectType.Player) && wounded.HealthAmount < 0.1) if (owner != null) Value.Frag += 1;
			if (wounded.HealthAmount < 0.1) if (owner != null) Value.Exp += (int)wounded.MaxHealthAmount * 2;
			if (owner != null) Value.Exp += damage * 2;
			// Получение уровня
			if (Value.Exp >= (100 * Value.Lvl + 2 ^ Value.Lvl))
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}

		public override void AddExpTeam(AGameObject player, AGameObject wounded, int damage, int teamMembers)
		{
			if (player != null) Value.Exp += damage / teamMembers;

			if (wounded.HealthAmount < 0.1) if (player != null)
				{
					Value.Exp += (int)wounded.MaxHealthAmount / teamMembers;
				}

			if (Value.Exp >= (100 * Value.Lvl + 2 ^ Value.Lvl))
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}
	}
}
