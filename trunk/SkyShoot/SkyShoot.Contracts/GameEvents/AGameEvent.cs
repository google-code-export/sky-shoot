using System;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.GameEvents
{
	[DataContract]
	[KnownType(typeof( NewObjectEvent))] //возможно должно быть не здесь
	[KnownType(typeof(ObjectDirectionChanged))]
	[KnownType(typeof(ObjectDeleted))]
	[KnownType(typeof(ObjectHealthChanged))]
	public abstract class AGameEvent
	{
		[DataMember]
		public long TimeStamp { get; protected set; }//номер update'a
		[DataMember]
		public Guid Id { get; protected set; }

		public AGameEvent(Guid id,long timeStamp){
			Id = id;
			TimeStamp = timeStamp;
		}

		public abstract void UpdateMob(AGameObject mob);
		//этот метод будет вызываться на клиенте при нахождении соответствующего объекта
	}

	[DataContract]
	public class NewObjectEvent : AGameEvent
	{
		[DataMember]
		private AGameObject _newMob;
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
		[DataMember]
		private Vector2 _newRunDirection;
		public ObjectDirectionChanged(Vector2 direction,Guid id,long timeStamp)
			:base(id,timeStamp)
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
		public ObjectDeleted(Guid id,long timeStamp)
			:base(id,timeStamp)
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
		[DataMember]
		private float _health;

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
}
