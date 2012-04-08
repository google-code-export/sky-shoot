using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Mobs
{
	class Spider : Mob
	{
		// todo //!! move to constants
		const int RADIUS_MIN = 15;
		const int RADIUS_MAX = 20;

		public Spider(float healthAmount)
			: base(healthAmount)
		{
			Random rand = new Random();
			Radius = rand.Next(RADIUS_MIN, RADIUS_MAX);
			Speed = Constants.SPIDER_SPEED;
		}
	}
}
