using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.ServProgram.Weapon;

namespace SkyShoot.ServProgram.Mobs
{
	class ParentMob : Mob
	{
		private readonly int _shootingDelay = 3000;
		private long _lastShoot;

		// todo //!! move to constants
		const int RADIUS_MIN = 30;
		const int RADIUS_MAX = 40;

		public ParentMob(float healthAmount) : base(Constants.PARENT_MOB_HEALTH)
		{
			Radius = new Random().Next(RADIUS_MIN, RADIUS_MAX);
			Speed = Constants.PARENT_MOB_SPEED;
			Weapon = new MobGenerator(Guid.NewGuid(), this);
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - _lastShoot > _shootingDelay && Weapon != null && Weapon.IsReload(time))
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
