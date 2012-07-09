using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;

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
			Radius = new Random().Next(RADIUS_MIN, RADIUS_MAX);
			Speed = Constants.SPIDER_SPEED;
		}
	}
}
