using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public Guid guid { get; protected set; }

		public AGameEvent(Guid id,long timeStamp){
			guid = id;
			TimeStamp = timeStamp;
		}

		public abstract void updateMob(AGameObject mob);
		//этот метод будет вызываться на клиенте при нахождении соответствующего объекта
	}

	public class NewObjectEvent : AGameEvent{
		[DataMember]
		private AGameObject newMob;
		public NewObjectEvent(AGameObject mob,long timeStamp)
			:base(mob.Id,timeStamp)
		{
			newMob = mob;
		}

		public override void updateMob(AGameObject mob)
		{
			mob.Copy(newMob);
		}
	}

	public class ObjectDirectionChanged : AGameEvent
	{
		[DataMember]
		private Vector2 newRunDirection;
		public ObjectDirectionChanged(Vector2 direction,Guid id,long timeStamp)
			:base(id,timeStamp)
		{
			newRunDirection = direction;
		}

		public override void updateMob(AGameObject mob)
		{
			mob.RunVector = newRunDirection;
		}
	}

	public class ObjectDeleted : AGameEvent
	{
		public ObjectDeleted(Guid id,long timeStamp)
			:base(id,timeStamp)
		{
		}

		public override void updateMob(AGameObject mob)
		{
			mob.IsActive = false;
		}
	}

	public class ObjectHealthChanged : AGameEvent
	{
		[DataMember]
		private float health;

		public ObjectHealthChanged(float newHp, Guid id, long timeStamp)
			: base(id, timeStamp)
		{
			health = newHp;
		}

		public override void updateMob(AGameObject mob)
		{
			mob.HealthAmount = health;
		}
	}
}
