using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Mobs
{
	class Caterpillar : SpiderWithSimpleMind
	{
		private Caterpillar _parent, _child;
		private long _childBornTime;
		public Caterpillar(float healthAmount, Caterpillar parent = null)
			: base(healthAmount)
		{
			_parent = parent;
			Speed = Constants.CATERPILLAR_SPEED;
			DefaultSpeed = Speed;
			Radius = Constants.CATERPILLAR_RADIUS;
			Damage = Constants.CATERPILLAR_DAMAGE;
			_childBornTime = -1;
			ObjectType = EnumObjectType.Caterpillar;
		}

		public override IEnumerable<AGameEvent> Think(List<Contracts.GameObject.AGameObject> gameObjects, System.Collections.Generic.List<Contracts.GameObject.AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>();
			if (_parent != null && !_parent.IsActive)
			{
				_parent = null;
			}
			if (_childBornTime == -1)
			{
				_childBornTime = time;
			}
			if ((_child == null || !_child.IsActive) && time - _childBornTime > Constants.CATERPILLAR_CHILD_BORN_INTERVAL)
			{
				_child = new Caterpillar(MaxHealthAmount, this);
				MaxHealthAmount *= 1.05f;
				HealthAmount *= 1.05f;
				Radius *= 1.05f;
				_child.Coordinates = Coordinates + (-RunVector) * (_child.Radius + Radius);
				newGameObjects.Add(_child);
				res.Add(new NewObjectEvent(_child, time));
			}
			if (_parent == null)
			{
				res.AddRange(base.Think(gameObjects, newGameObjects, time));
				if (Target != null)
				{
					//if(Vector2.DistanceSquared())
					{
						RunVector = (Target.Coordinates + (new Vector2(RunVector.Y, -RunVector.X)) * (Target.Radius + Radius)) - Coordinates;
					}
					//else
					//{
					//  RunVector = (Target.Coordinates + new Vector2(RunVector.Y, -RunVector.X)) - Coordinates;
					//}
					RunVector.Normalize();
				}
			}
			else
			{
				RunVector = _parent.Coordinates - Coordinates;
				RunVector.Normalize();
			}
			ShootVector = RunVector;
			return res;
		}

		public override IEnumerable<AGameEvent> Do(Contracts.GameObject.AGameObject obj, List<Contracts.GameObject.AGameObject> newObjects, long time)
		{
			var res = base.Do(obj, newObjects, time);
			Wait = 0;
			return res;
		}

		public override IEnumerable<AGameEvent> OnDead(Contracts.GameObject.AGameObject obj, List<Contracts.GameObject.AGameObject> newObjects, long time)
		{
			if (_parent != null && _parent.IsActive)
			{
				_parent._child = null;
			}
			return base.OnDead(obj, newObjects, time);
		}
	}
}
