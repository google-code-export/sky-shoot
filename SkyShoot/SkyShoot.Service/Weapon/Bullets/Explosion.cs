using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	class Explosion : AProjectile
	{
		private const int TIME_TO_LEAVE = (int)Constants.EXPLOSION_LIFE_DISTANCE;
		private const int TIME_TO_DAMAGE = (int)(Constants.EXPLOSION_LIFE_DISTANCE / 45f);

		private long _explodedTime;

		private bool _isExploded;

		public Explosion(AGameObject owner, Guid id, Vector2 coordinates)
			: base(owner, id, Vector2.Zero)
		{
			_explodedTime = -1;
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
			if (_explodedTime == -1)
			{
				_explodedTime = time;
			}
			
			if (time > _explodedTime)
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
			if (time > _explodedTime)
			{
				IsActive = false;
				return new AGameEvent[] { new ObjectDeleted(Id, time) };
			}
			return new AGameEvent[] { };
		}
	}
}
