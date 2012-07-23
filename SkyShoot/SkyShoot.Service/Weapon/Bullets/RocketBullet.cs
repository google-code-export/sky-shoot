using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
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
			if (obj.TeamIdentity!= Owner.TeamIdentity && obj.Is(EnumObjectType.Block) && !obj.Is(EnumObjectType.Poisoning))
			{
				int circles;
				if (Damage/Constants.ROCKET_BULLET_DAMAGE>1)
				{
					circles = Constants.ROCKET_EXPLOSION_CIRCLES + 2;
				}
				else
				{
					circles = Constants.ROCKET_EXPLOSION_CIRCLES;
				}
				var explosion = new Explosion(Owner, Guid.NewGuid(), Coordinates, circles, 1)
				                	{
				                		Radius = Constants.ROCKET_EXPLOSION_RADIUS,
										Damage = Constants.ROCKET_EXPLOSION_DAMAGE,
				                	};

				res.Add(new NewObjectEvent(explosion, time));
				newObjects.Add(explosion);
			}
			return res;
		}
	}
}
