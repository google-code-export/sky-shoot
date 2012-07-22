using SkyShoot.Contracts.GameObject;

namespace SkyShoot.Service.Statistics
{
	class ExponentialExpTracker : ExpTracker
	{
		public override void AddExpPlayer(AGameObject owner, AGameObject wounded, int damage)
		{
			if (wounded.Is(AGameObject.EnumObjectType.Player) && wounded.HealthAmount < 0.1) if (owner != null) Value.Frag += 1;
			if (wounded.HealthAmount < 0.1) if (owner != null) Value.Experience += (int)wounded.MaxHealthAmount * 2;
			if (owner != null) Value.Experience += damage * 2;
			// Получение уровня
			if (Value.Experience >= (100 * Value.Level + 2 ^ Value.Level))
			{
				Value.Experience = Value.Experience - 100 * Value.Level;
				Value.Level++;
			}
		}

		public override void AddExpTeam(AGameObject player, AGameObject wounded, int damage, int teamMembers)
		{
			if (player != null) Value.Experience += damage / teamMembers;

			if (wounded.HealthAmount < 0.1) if (player != null)
				{
					Value.Experience += (int)wounded.MaxHealthAmount / teamMembers;
				}

			if (Value.Experience >= (100 * Value.Level + 2 ^ Value.Level))
			{
				Value.Experience = Value.Experience - 100 * Value.Level;
				Value.Level++;
			}
		}
	}
}
