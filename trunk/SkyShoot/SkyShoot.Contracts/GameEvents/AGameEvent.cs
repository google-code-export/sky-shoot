using System;
using System.Runtime.Serialization;

using SkyShoot.Contracts.Mobs;

using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.GameEvents
{
	[DataContract]
	[KnownType(typeof(NewObjectEvent))]
	[KnownType(typeof(ObjectDirectionChanged))]
	[KnownType(typeof(ObjectDeleted))]
	[KnownType(typeof(ObjectHealthChanged))]
	[KnownType(typeof(BonusesChanged))]
	[KnownType(typeof(WeaponChanged))]
	public abstract class AGameEvent
	{
		// now = DateTime.Now.Ticks/10000; //время в миллисекундах с начала игры
		[DataMember]
		public long TimeStamp { get; set; }

		[DataMember]
		public Guid GameObjectId { get; set; }

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
		[DataMember]
		public AGameObject NewObj;

		public NewObjectEvent(AGameObject obj, long timeStamp)
			: base(obj.Id, timeStamp)
		{
			NewObj = GameObjectConverter.OneObject(obj);
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

		public ObjectDirectionChanged(Vector2 direction, Guid id, long timeStamp)
			: base(id, timeStamp)
		{
			NewRunDirection = direction;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.RunVector = NewRunDirection;
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
		[DataMember]
		public float Health;

		public ObjectHealthChanged(float newHp, Guid id, long timeStamp)
			: base(id, timeStamp)
		{
			Health = newHp;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.HealthAmount = Health;
		}
	}

	[DataContract]
	public class WeaponChanged : AGameEvent
	{
		[DataMember]
		public SkyShoot.Contracts.Weapon.AWeapon weapon;

		public WeaponChanged(SkyShoot.Contracts.Weapon.AWeapon weapon, Guid id, long timeStamp)
			: base(id, timeStamp)
		{
			this.weapon = weapon;
		}

		public override void UpdateMob(AGameObject mob)
		{
			mob.Weapon = weapon;
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