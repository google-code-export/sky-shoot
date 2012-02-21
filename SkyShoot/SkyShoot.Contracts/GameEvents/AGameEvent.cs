using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.GameEvents
{
	[DataContract]
	[KnownType(typeof( NewObjectEvent))] //возможно должно быть не здесь
	[KnownType(typeof(ObjectDirectionChanged))]
	[KnownType(typeof(ObjectDeleted))]
	public abstract class AGameEvent
	{
		[DataMember]
		public long TimeStamp { get; protected set; }//номер update'a
		[DataMember]
		public Guid guid { get; protected set; }

		public abstract void updateMob(AMob mob);
		//этот метод будет вызываться на клиенте при нахождении соответствующего объекта
	}

	public class NewObjectEvent : AGameEvent{
		[DataMember]
		AMob newMob;
		public NewObjectEvent(AMob mob,long timeStamp){
			newMob = mob;
			guid = mob.Id;
			TimeStamp = timeStamp;
		}

		public override void updateMob(AMob mob)
		{
			mob.Copy(newMob);
		}
	}

	public class ObjectDirectionChanged : AGameEvent
	{
		[DataMember]
		Vector2 newRunDirection;
		public ObjectDirectionChanged(Vector2 direction,Guid id,long timeStamp)
		{
			newRunDirection = direction;
			guid = id;
			TimeStamp = timeStamp;
		}

		public override void updateMob(AMob mob)
		{
			mob.RunVector = newRunDirection;
		}
	}

	public class ObjectDeleted : AGameEvent
	{
		public ObjectDeleted(Guid id,long timeStamp)
		{
			guid = id;
			TimeStamp = timeStamp;
		}

		
		public override void updateMob(AMob mob)
		{
			//mob.IsActive = false; //это если на клиенте тоже сделать ObjectPool
		}
	}
}
