using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Service.Statistics;

namespace SkyShoot.Service.Statistics
{
	class ExponentialExpTracker : ExpTracker
	{
		public override void AddExpPlayer(AGameObject owner, AGameObject wounded, int damage)
		{
			if (wounded.Is(AGameObject.EnumObjectType.Player) && wounded.HealthAmount < 0.1) if (owner != null) Value.Frag += 1;
			if (wounded.HealthAmount < 0.1) if (owner != null) Value.Exp += (int)wounded.MaxHealthAmount;
			if (owner != null) Value.Exp += damage;
			// Получение уровня
			if (Value.Exp >= (100 * Value.Lvl + 2 ^ Value.Lvl))
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}

		public override void AddExpTeam(AGameObject player, AGameObject wounded, int damage)
		{
			if (player != null) Value.Exp += damage;

			if (wounded.HealthAmount < 0.1) if (player != null)
				{
					Value.Exp += (int)wounded.MaxHealthAmount;
				}

			if (Value.Exp >= (100 * Value.Lvl + 2 ^ Value.Lvl))
			{
				Value.Exp = Value.Exp - 100 * Value.Lvl;
				Value.Lvl++;
			}
		}
	}
}
