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
	class Explosion : AProjectile
	{
		private bool _isExploded;

		public Explosion(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction) 
		{
			Speed = Constants.EXPLOSION_SPEED;
			Damage = Constants.EXPLOSION_DAMAGE;
			HealthAmount = Constants.EXPLOSION_LIFE_DISTANCE;
			ObjectType = EnumObjectType.Explosion;
			_isExploded = false;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, long time)
		{
			var res = base.Do(obj, time);
			IsActive = true;
			_isExploded = true;
			//!! todo: удалять сообщение objectdeleted
			return res;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, long time)
		{
			if(_isExploded)
			{
				IsActive=false;
				return new AGameEvent[] { new ObjectDeleted(this.Id, time) };
			}
			return new AGameEvent[]{};
		}
	}
}
