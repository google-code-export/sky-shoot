using System;
using System.Collections.Generic;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	class RocketBullet : AProjectile
	{
		public RocketBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction)
		{
			Speed = Constants.ROCKET_BULLET_SPEED;
			Damage = Constants.ROCKET_BULLET_DAMAGE;
			HealthAmount = Constants.ROCKET_BULLET_LIFE_DISTANCE;
			Radius = Constants.ROCKET_BULLET_RADIUS;
			ObjectType = EnumObjectType.RocketBullet;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			var res = new List<AGameEvent>(base.Do(obj, newObjects, time));
			if (obj.Id != Owner.Id && obj.Is(EnumObjectType.Block))
			{
				var explosion = new Explosion(Owner, Guid.NewGuid(), Coordinates, time);
				res.Add(new NewObjectEvent(explosion, time));
				newObjects.Add(explosion);
			}

			return res;
		}
	}
}
