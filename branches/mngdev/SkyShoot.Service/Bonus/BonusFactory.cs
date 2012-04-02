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
			// TODO: special nice method for choosing bonus type 
			int r = _random.Next(3);
			if (r == 0)
			{
				return new Shield(coordinates);
			}
			if (r == 1)
			{
				return new DoubleDamage(coordinates);
			}
			else
			{
				return new Remedy(coordinates);
			}
		}
	}
}
