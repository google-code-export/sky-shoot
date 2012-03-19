using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Bonuses;

using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Contracts.Mobs
{
	[DataContract]
	public class AGameObject
	{
		/// <summary>
		/// основное перечисление всех возможных типов обектов игры
		/// </summary>
		public enum EnumObjectType
		{
			[EnumMember]
			Player,
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

		#region основные свойства
		#region административные
		//[DataMember]
		public bool IsPlayer 
		{ 
			get { return ObjectType == EnumObjectType.Player; }
		}
		public bool IsBullet
		{
			get
			{
				return 
					ObjectType == EnumObjectType.Bullet || 
					ObjectType == EnumObjectType.Flame ||
					ObjectType == EnumObjectType.LaserBullet ||
					ObjectType == EnumObjectType.ShutgunBullet || 
					ObjectType == EnumObjectType.Rocket;
			}
		}
	
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public EnumObjectType ObjectType { get; set; }

		[DataMember]
		public bool IsActive { get; set; }

		#endregion

		#region здоровье и урон
		[DataMember]
		public float Damage { get; set; }

		public AWeapon Weapon { get; set; }

		[DataMember]
		public float HealthAmount { get; set; }

		[DataMember]
		public float MaxHealthAmount { get; set; }

		[DataMember]
		public AObtainableDamageModifier.AObtainableDamageModifiers State { get; set; }

		#endregion

		#region перемещение и геометрия
		[DataMember]
		public Vector2 RunVector;// { get; set; }

		[DataMember]
		public Vector2 ShootVector;// { get; set; }

		[DataMember]
		public Vector2 Coordinates;

		[DataMember]
		public float Radius { get; set; } // размер моба
	
		[DataMember]
		public float Speed { get; set; } //скорость: пикселы в миллисекунду

		#endregion


		// расширить типом моба, размером, цветом, и т.д.
		#endregion

		#region служебные функции (конструкторы)
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

		public virtual void Copy(AGameObject other)
		{
			Id = other.Id;
			State = other.State;
			IsActive = other.IsActive;
			ObjectType = other.ObjectType;

			HealthAmount = other.HealthAmount;
			Damage = other.Damage;
			MaxHealthAmount = other.MaxHealthAmount;

			Coordinates = other.Coordinates;
			Radius = other.Radius;
			RunVector = other.RunVector;
			ShootVector = other.ShootVector;
			Speed = other.Speed;
		}

		public AGameObject()
		{
			IsActive = true;
		}
		#endregion

		#region основные функции
		public virtual void Move()
		{
			RunVector = Vector2.Normalize(RunVector);
			ShootVector = RunVector;
		}

		public virtual void Think(List<AGameObject> players)
		{}

		public virtual void Do(AGameObject obj)
		{}

		public virtual Vector2 ComputeMovement(long updateDelay, GameLevel gameLevel)
		{

			var newCoord = RunVector * Speed * updateDelay + Coordinates;
			return newCoord;
		}

		#endregion
	}
}
