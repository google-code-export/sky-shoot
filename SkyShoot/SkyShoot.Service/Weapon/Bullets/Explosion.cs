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
	class Explosion : AProjectile
	{
        private const int TimeToLeave = (int)Constants.EXPLOSION_LIFE_DISTANCE;
        private const int TimeToDamage = (int)(Constants.EXPLOSION_LIFE_DISTANCE / 45f);

        private readonly long _explodedTime;

		private bool _isExploded;
	    
		public Explosion(AGameObject owner, Guid id, Vector2 coordinates, long explodedTime)
			: base(owner, id, Vector2.Zero)
		{
			_explodedTime = explodedTime;
			Speed = Constants.EXPLOSION_SPEED;
			Damage = Constants.EXPLOSION_DAMAGE;
			HealthAmount = Constants.EXPLOSION_LIFE_DISTANCE;
			ObjectType = EnumObjectType.Explosion;
			Radius = Constants.EXPLOSION_RADIUS;
			Coordinates = coordinates;
			_isExploded = false;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
		    if (_isExploded && _explodedTime + TimeToDamage < time)
		    {
		        return new AGameEvent[] { };
		    }
		    var res = new List<AGameEvent>(base.Do(obj, newObjects, time));
		    _isExploded = true;
			// надо удалять сообщение objectdeleted 
			// потому что на самом деле взрыв этот не должен удаляться
			IsActive = true;
			res.RemoveAll(o => o.GetType() == typeof(ObjectDeleted));
			return res;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, List<AGameObject> newGameObjects, long time)
		{
			if (_explodedTime + TimeToLeave < time)
			{
				IsActive = false;
				return new AGameEvent[] { new ObjectDeleted(Id, time) };
			}
			return new AGameEvent[] { };
		}
	}
}
