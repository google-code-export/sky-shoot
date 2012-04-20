using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.GameEvents;

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
			ObjectType = EnumObjectType.RocketBullet;
		}

		public override IEnumerable<Contracts.GameEvents.AGameEvent> Do(AGameObject obj, long time)
		{
			var res = new List<Contracts.GameEvents.AGameEvent>(base.Do(obj, time));
			Explosion explosion = new Explosion(this.Owner, Guid.NewGuid(), this.Coordinates);
			res.Add(new NewObjectEvent(explosion, time));

			return res;
		}
	}
}
