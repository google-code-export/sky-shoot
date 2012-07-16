using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram.Mobs
{
	class ChildrenMob : Mob
	{
		public ChildrenMob()
			: base(Constants.CHILDREN_MOB_HEALTH)
		{
			TeamIdentity = null;
			Radius = new Random().Next(Constants.CHILDREN_MOB_RADIUS_MIN, Constants.CHILDREN_MOB_RADIUS_MAX);
			Speed = Constants.SPIDER_SPEED;
		}
	}
}
