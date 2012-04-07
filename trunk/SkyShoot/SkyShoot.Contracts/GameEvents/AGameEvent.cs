using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using SkyShoot.Contracts.Mobs;

using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.GameEvents
{
	[DataContract]
	[KnownType(typeof (NewObjectEvent))] //возможно должно быть не здесь
	[KnownType(typeof (ObjectDirectionChanged))]
	[KnownType(typeof (ObjectDeleted))]
	[KnownType(typeof(ObjectHealthChanged))]
	[KnownType(typeof(BonusesChanged))]
	public abstract class AGameEvent
	{
		// now = DateTime.Now.Ticks/10000; //время в миллисекунда с начала игры
		[DataMember]
		public long TimeStamp { get; protected set; }

		[DataMember]
		public Guid GameObjectId { get; protected set; }

		protected AGameEvent(Guid id, long timeStamp)
		{
			GameObjectId = id;
			TimeStamp = timeStamp;
		}

		/// <summary>
		/// этот метод будет вызываться на клиенте при нахождении соответствующего объекта
		/// </summary>
		/// <param name="mob"></param>
		public abstract void UpdateMob(AGameObject mob);
	}

	[DataContract]
	public class NewObjectEvent : AGameEvent
	{
		[DataMember] private AGameObject _newMob;

		public NewObjectEvent(AGameObject mob, long timeStamp)
			: base(mob.Id, timeStamp)
		{
			_newMob = GameObjectConverter.OneObject(mob);
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.Copy(_newMob);
		}
	}

	[DataContract]
	public class ObjectDirectionChanged : AGameEvent
	{
		[DataMember] private Vector2 _newRunDirection;

		public ObjectDirectionChanged(Vector2 direction, Guid id, long timeStamp)
			: base(id, timeStamp)
		{
			_newRunDirection = direction;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.RunVector = _newRunDirection;
		}
	}

	[DataContract]
	public class ObjectDeleted : AGameEvent
	{
		public ObjectDeleted(Guid id, long timeStamp)
			: base(id, timeStamp)
		{
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.IsActive = false;
		}
	}

	[DataContract]
	public class ObjectHealthChanged : AGameEvent
	{
		[DataMember] private float _health;

		public ObjectHealthChanged(float newHp, Guid id, long timeStamp)
			: base(id, timeStamp)
		{
			_health = newHp;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.HealthAmount = _health;
		}
	}

	[DataContract]
	public class BonusesChanged : AGameEvent
	{
		[DataMember]
		public AGameObject.EnumObjectType Bonuses;

		public BonusesChanged(Guid id, long timeStamp, AGameObject.EnumObjectType bonuses) : 
			base(id, timeStamp)
		{
			Bonuses = bonuses;
		}

		public override void UpdateMob(AGameObject mob)
		{
			//todo //!! придумать что-то, ибо переносить в Mob бонусы нехорошо
		}
	}
}