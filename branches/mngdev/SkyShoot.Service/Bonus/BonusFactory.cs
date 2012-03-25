using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	class BonusFactory
	{
		private Random _random = new Random();

		public AGameBonus CreateBonus(Vector2 coordinates)
		{
			if (_random.Next(2) == 0)
			{
				return new Shield(coordinates);
			}
			else
			{
				return new DoubleDamage(coordinates);
			}
		}
	}
}
