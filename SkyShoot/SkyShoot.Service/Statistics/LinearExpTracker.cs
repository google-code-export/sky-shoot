using SkyShoot.Contracts.GameObject;

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
					Value.Experience += (int)wounded.MaxHealthAmount;
				}

			if (owner != null) Value.Experience += damage;
			// Получение уровня
			if (Value.Experience >= 100 * Value.Level)
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

			if (Value.Experience >= 100 * Value.Level)
			{
				Value.Experience = Value.Experience - 100 * Value.Level;
				Value.Level++;
			}
		}
	}
}
