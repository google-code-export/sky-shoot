using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.ServProgram.Mobs
{
	class ShootingMob: Mob
	{
		private long _lastShoot = 0;
		private int _shootingDelay = 10000;

		public ShootingMob(float health, AWeapon weapon,int shootingDelay)
			: base(health)
		{
			Weapon = weapon;
			_shootingDelay = shootingDelay;
			Radius = 10;
			Speed = 0.03f;
		}

		public override IEnumerable<Contracts.GameEvents.AGameEvent> Think(List<AGameObject> gameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, time));
			ShootVector = RunVector;
			if (time - _lastShoot > _shootingDelay && Weapon != null && Weapon.Reload(time))
			{
				_lastShoot = time;
				var bullets = Weapon.CreateBullets(this, this.ShootVector);
				foreach(AGameObject bullet in bullets){
					res.Add(new NewObjectEvent(bullet,time));
				}
				gameObjects.AddRange(bullets);
			}
			return res;
		}
	}
}
