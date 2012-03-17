using System;

using System.Runtime.Serialization;

using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Bonuses;

using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Contracts.Mobs
{
	[DataContract]
	public class AGameObject
	{

		[DataMember]
		public bool IsPlayer { get; set; }

		[DataMember]
		public float Damage { get; set; }// до тех пор пока пулю не сделаем мобом с оружием

		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public Vector2 RunVector;// { get; set; }

		[DataMember]
		public Vector2 ShootVector;// { get; set; }

		[DataMember]
		public Vector2 Coordinates;

		[DataMember]
		public float HealthAmount { get; set; }

		//[DataMember]
		public float Radius { get; set; } // размер моба

		[DataMember]
		public float Speed { get; set; } //скорость: пикселы в миллисекунду

		public AWeapon Weapon { get; set; }

		[DataMember]
		public bool IsActive { get; set; }

		[DataMember]
		public AObtainableDamageModifier.AObtainableDamageModifiers State { get; set; }

		[DataMember]
		public float MaxHealthAmount { get; set; }

		public AGameObject(Vector2 coordinates, Guid id)
		{
			RunVector = ShootVector = new Vector2(0, 1);
			Coordinates = coordinates;
			Id = id;
			IsActive = true;
		}

		public AGameObject(AGameObject other)
		{
			Copy(other);
		}

		public void Copy(AGameObject other)
		{
			Coordinates = other.Coordinates;
			Id = other.Id;
			HealthAmount = other.HealthAmount;
			IsPlayer = other.IsPlayer;
			Radius = other.Radius;
			RunVector = other.RunVector;
			ShootVector = other.ShootVector;
			Speed = other.Speed;
			State = other.State;
			MaxHealthAmount = other.MaxHealthAmount;
			IsActive = other.IsActive;
			Type = other.Type;
			this.Type = other.Type;
		}

		public AGameObject()
		{
		}

		public enum EnumObjectType
		{
			[EnumMember]
			Mob,
			[EnumMember]
			Bullet,
			[EnumMember]
			Rocket,
			[EnumMember]
			Flame,
			[EnumMember]
			Bonus,
			[EnumMember]
			LaserBullet,
			[EnumMember]
			ShutgunBullet
		}

		[DataMember]
		public EnumObjectType Type { get; set; }

		// расширить типом моба, размером, цветом, и т.д.
	}
}
