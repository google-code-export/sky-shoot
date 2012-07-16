using System;
using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Mobs
{
	class Hydra : Mob
	{
		public Hydra()
			: base(Constants.HYDRA_HEALTH)
		{
			Radius = new Random().Next(Constants.HYDRA_RADIUS_MIN, Constants.HYDRA_RADIUS_MAX);
			Speed = Constants.HYDRA_SPEED;
		}

		public override IEnumerable<AGameEvent> OnDead(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			List<AGameEvent> events = base.OnDead(obj, newObjects, time).ToList();

			for (int i = 0; i < 2; i++)
			{
				var childrenMob = new ChildrenMob { Coordinates = new Vector2(Coordinates.X, Coordinates.Y) };
				newObjects.Add(childrenMob);
				events.Add(new NewObjectEvent(childrenMob, time));
			}
			return events;
		}
	}
}
