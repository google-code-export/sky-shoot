using System;
using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.ServProgram.Weapon;

namespace SkyShoot.ServProgram.Mobs
{
	class ParentMob : Mob
	{
		private const int SHOOTING_DELAY = 3000;
		private long _lastShoot;

		public ParentMob()
			: base(Constants.PARENT_MOB_HEALTH)
		{
			Radius = new Random().Next(Constants.PARENT_MOB_RADIUS_MIN, Constants.PARENT_MOB_RADIUS_MAX);
			Speed = Constants.PARENT_MOB_SPEED;
			Weapon = new MobGenerator(Guid.NewGuid(), this);
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - _lastShoot > SHOOTING_DELAY && Weapon != null && Weapon.IsReload(time))
			{
				_lastShoot = time;
				var bullets = Weapon.CreateBullets(ShootVector);
				res.AddRange(bullets.Select(bullet => new NewObjectEvent(bullet, time)));
				newGameObjects.AddRange(bullets);
			}
			return res;
		}
	}
}
