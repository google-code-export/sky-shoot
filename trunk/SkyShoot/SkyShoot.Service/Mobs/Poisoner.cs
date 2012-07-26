using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram.Mobs
{
	class Poisoner : Mob
	{
		private readonly int _shootingDelay = 1000;
		private long _lastShoot;

		public Poisoner(float health, AWeapon weapon, int shootingDelay)
			: base(health)
		{
			Weapon = weapon;
			Weapon.Owner = this;
			_shootingDelay = shootingDelay;
			Radius = 10;
			Speed = Constants.POISONER_MOB_SPEED;
			DefaultSpeed = Speed;
			this.ObjectType = EnumObjectType.Poisoner;
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
