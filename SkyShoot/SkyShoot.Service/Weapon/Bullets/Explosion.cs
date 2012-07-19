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
		private int _number;
		private int _maxCircles;

		public Explosion(AGameObject owner, Guid id, Vector2 coordinates,int maxCircles, int number)
			: base(owner, id, Vector2.Zero)
		{
			_number = number;
			_maxCircles = maxCircles;
			_explodedTime = -1;
			Speed = Constants.EXPLOSION_SPEED;			
			HealthAmount = Constants.EXPLOSION_LIFE_DISTANCE;
			ObjectType = EnumObjectType.Explosion;
			Coordinates = coordinates;
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
			// надо удалять сообщение objectdeleted 
			// потому что на самом деле взрыв этот не должен удаляться
			IsActive = true;
			res.RemoveAll(o => o.GetType() == typeof(ObjectDeleted));
			return res;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, List<AGameObject> newGameObjects, long time)
		{
			List<AGameEvent> res = new List<AGameEvent>();
			if (time > _explodedTime)
			{
				if (_number < _maxCircles)
				{
					var explos = new Explosion(Owner, Guid.NewGuid(), Coordinates, _maxCircles, _number + 1)
					{
						Radius = this.Radius * (_number + 1) / _number,
						Damage = this.Damage,
					};
					newGameObjects.Add(explos);
					res.Add(new NewObjectEvent(explos, time));
				}
				IsActive = false;
				res.Add(new ObjectDeleted(Id, time));
			}
			return res;
		}

		public override IEnumerable<AGameEvent> OnDead(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return base.OnDead(obj, newObjects, time);
		}
	}
}
