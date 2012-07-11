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
		public override void AddExp(AGameObject owner, AGameObject wounded, int damage)
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
	}
}
