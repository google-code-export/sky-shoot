using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram.Mobs
{
	class ShootingMob : Mob
	{
		protected readonly int ShootingDelay = 10000;
		protected long LastShoot;

		public ShootingMob(float health, AWeapon weapon, int shootingDelay)
			: base(health)
		{
			Weapon = weapon;
			Weapon.Owner = this;
			ShootingDelay = shootingDelay;
			Radius = 10;
			Speed = Constants.SPIDER_SPEED;
			DefaultSpeed = Speed;
			ObjectType = EnumObjectType.ShootingSpider;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - LastShoot > ShootingDelay && Weapon != null && Weapon.IsReload(time) && Target != null)
			{
				LastShoot = time;
				var bullets = Weapon.CreateBullets(ShootVector);
				res.AddRange(bullets.Select(bullet => new NewObjectEvent(bullet, time)));
				newGameObjects.AddRange(bullets);
			}
			return res;
		}
	}
}
