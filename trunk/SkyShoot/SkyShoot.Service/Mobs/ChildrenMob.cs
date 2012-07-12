using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.ServProgram.Mobs
{
	class ChildrenMob : Mob
	{
		// todo //!! move to constants
		const int RADIUS_MIN = 10;
		const int RADIUS_MAX = 13;

		public ChildrenMob(float healthAmount) 
			: base(Constants.CHILDREN_MOB_HEALTH)
		{
			Radius = new Random().Next(RADIUS_MIN, RADIUS_MAX);
			Speed = Constants.SPIDER_SPEED;
		}
	}
}
