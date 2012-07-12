using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Service.Weapon;
using SkyShoot.XNA.Framework;

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

		public override IEnumerable<AGameEvent> OnDead(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			List<AGameEvent> events = base.OnDead(obj, newObjects, time).ToList();
			
			for(int i = 0; i < 2; i++)
			{
				var childrenMob = new ChildrenMob(Constants.CHILDREN_MOB_HEALTH);
				childrenMob.Coordinates = new Vector2(Coordinates.X, Coordinates.Y);
				newObjects.Add(childrenMob);
				events.Add(new NewObjectEvent(childrenMob, time));
			}
			return events;
		}
	}
}
