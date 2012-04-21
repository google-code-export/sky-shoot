using System.Collections.Generic;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.ServProgram.Mobs
{
	class ShootingMob: Mob
	{
		private long _lastShoot;
		private readonly int _shootingDelay = 10000;

		public ShootingMob(float health, AWeapon weapon,int shootingDelay)
			: base(health)
		{
			Weapon = weapon;
			_shootingDelay = shootingDelay;
			Radius = 10;
			Speed = 0.03f;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - _lastShoot > _shootingDelay && Weapon != null && Weapon.Reload(time))
			{
				_lastShoot = time;
				var bullets = Weapon.CreateBullets(this, ShootVector);
				foreach(AGameObject bullet in bullets){
					res.Add(new NewObjectEvent(bullet,time));
				}
				newGameObjects.AddRange(bullets);
			}
			return res;
		}
	}
}
