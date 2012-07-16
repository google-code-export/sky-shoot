using System;
using System.Runtime.Serialization;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;

using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.GameEvents
{
	public enum EventType
	{
		NewObjectEvent,
		ObjectDirectionChangedEvent,
		ObjectShootEvent,
		ObjectDeletedEvent,
		WeaponChangedEvent,
		ObjectHealthChanged,
		EmptyGameEvent
	}

	[DataContract]
	[KnownType(typeof(NewObjectEvent))]
	[KnownType(typeof(ObjectDirectionChanged))]
	[KnownType(typeof(ObjectDeleted))]
	[KnownType(typeof(ObjectHealthChanged))]
	//[KnownType(typeof(BonusesChanged))]
	//[KnownType(typeof(WeaponChanged))]
	public abstract class AGameEvent
	{
		// now = DateTime.Now.Ticks/10000; //время в миллисекундах с начала игры
		[DataMember]
		public long TimeStamp { get; set; }

		[DataMember]
		public Guid? GameObjectId { get; set; }

		[DataMember]
		public EventType Type;

		protected AGameEvent(Guid? id, long timeStamp)
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

	/// <summary>
	/// Вспомогательный класс, обозначающий пустое событие
	/// Используется клиентом для обозначения того,
	/// что нужно запросить с сервера GameEvent'ы
	/// // todo rename
	/// </summary>
	public class EmptyEvent : AGameEvent
	{
		public EmptyEvent(Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Type = EventType.EmptyGameEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			throw new NotImplementedException();
		}
	}

	[DataContract]
	public class NewObjectEvent : AGameEvent
	{
		[DataMember]
		public AGameObject NewObj;

		public NewObjectEvent(AGameObject obj, long timeStamp)
			: base(obj.Id, timeStamp)
		{
			NewObj = GameObjectConverter.OneObject(obj);
			Type = EventType.NewObjectEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.Copy(NewObj);
		}
	}

	[DataContract]
	public class ObjectDirectionChanged : AGameEvent
	{
		[DataMember]
		public Vector2 NewRunDirection;

		public ObjectDirectionChanged(Vector2 direction, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			NewRunDirection = direction;
			Type = EventType.ObjectDirectionChangedEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.RunVector = NewRunDirection;
		}
	}

	public class ObjectShootEvent : AGameEvent
	{
		public Vector2 ShootDirection;

		public ObjectShootEvent(Vector2 shootDirection, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			ShootDirection = shootDirection;
			Type = EventType.ObjectShootEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			throw new NotImplementedException();
		}
	}

	[DataContract]
	public class ObjectDeleted : AGameEvent
	{
		public ObjectDeleted(Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Type = EventType.ObjectDeletedEvent;
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
		public float Health;

		public ObjectHealthChanged(float newHp, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Health = newHp;
			Type = EventType.ObjectHealthChanged;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.HealthAmount = Health;
		}
	}

	/*[DataContract]*/
	public class WeaponChanged : AGameEvent
	{
		/*[DataMember] */
		public AWeapon Weapon;
		public WeaponType WeaponType;

		public WeaponChanged(AWeapon weapon, WeaponType weaponType, Guid? id, long timeStamp)
			: base(id, timeStamp)
		{
			Weapon = weapon;
			WeaponType = weaponType;
			Type = EventType.WeaponChangedEvent;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.Weapon = Weapon;
		}
	}

	//[DataContract]
	//public class BonusesChanged : AGameEvent
	//{
	//  [DataMember]
	//  public AGameObject.EnumObjectType Bonuses;

	//  public BonusesChanged(Guid id, long timeStamp, AGameObject.EnumObjectType bonuses) :
	//    base(id, timeStamp)
	//  {
	//    Bonuses = bonuses;
	//  }

	//  public override void UpdateMob(AGameObject mob)
	//  {
	//    //todo //!! придумать что-то, ибо переносить в Mob бонусы нехорошо
	//  }
	//}
}