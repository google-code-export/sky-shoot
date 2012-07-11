using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.ServProgram.Mobs
{
	class Hydra : Mob
	{
		// todo //!! move to constants
		const int RADIUS_MIN = 15;
		const int RADIUS_MAX = 20;

		public Hydra(float healthAmount) : base(Constants.HYDRA_HEALTH)
		{
			Radius = new Random().Next(RADIUS_MIN, RADIUS_MAX);
			Speed = Constants.HYDRA_SPEED;
		}
	}
}
