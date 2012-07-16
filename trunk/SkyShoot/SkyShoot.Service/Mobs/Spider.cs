using System;
using SkyShoot.Contracts.Service;
using SkyShoot.ServProgram.Mobs;

namespace SkyShoot.Service.Mobs
{
	class Spider : Mob
	{
		public Spider(float healthAmount)
			: base(healthAmount)
		{
			Radius = new Random().Next(Constants.SPIDER_RADIUS_MIN, Constants.SPIDER_RADIUS_MAX);
			Speed = Constants.SPIDER_SPEED;
		}
	}
}
