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
		[Flags]
		public enum EnumObjectType
		{
			/*
			 * 32-bit 00000000
			 *        ||||||||
			 *        ||/|/|/+-- LivingObject (something with mind and health)
			 *        || | |  
			 *        || | +---- Bullets
			 *        || |    
			 *        || |+----- Bonuses
			 *        ||      
			 *        |+-------- Walls      
			 *        |
			 *        +--------- Reserved
			 */
			[EnumMember]
			LivingObject = 0x0001,
			[EnumMember]
			Player = LivingObject | 0x2,
			[EnumMember]
			Mob = LivingObject | 0x4,
			[EnumMember]
			Bullet = 0x010,
			[EnumMember]
			Flame = Bullet | 0x020,
			[EnumMember]
			LaserBullet = Bullet | 0x040,
			[EnumMember]
			ShutgunBullet = Bullet | 0x080,
			[EnumMember]
			Rocket = Bullet | 0x100,
			[EnumMember]
			Bonus = 0x01000,
			[EnumMember]
			DoubleDamage = Bonus | 0x02000,
			[EnumMember]
			Shield = Bonus | 0x04000,
			[EnumMember]
			Remedy = Bonus | 0x08000,
			[EnumMember]
			Wall = 0x0100000,
		}

		#region основные свойства
		#region административные
		/// <summary>
		/// проверка вида объекта
		/// </summary>
		/// <returns></returns>
		public bool Is(EnumObjectType objectType)
		{
			return (ObjectType & objectType) == objectType;
		}

		[Obsolete("This prorepty is deprecated. Use Is funciton.")]
		public bool IsPlayer 
		{ 
			get { return Is(EnumObjectType.Player); }
		}

		[Obsolete("This prorepty is deprecated. Use Is funciton.")]
		public bool IsBullet
		{
			get
			{
				return Is(EnumObjectType.Bullet);
			}
		}
	
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public EnumObjectType ObjectType 
		{
			get;
			set;
		}

		//[DataMember]
		public bool IsActive 
		{ 
			get { return HealthAmount > 0; }
			set { HealthAmount = value ? 1 : -1; }
			}

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
			//IsActive = true;
		}

		public AGameObject(AGameObject other)
		{
			Copy(other);
		}

		public virtual void Copy(AGameObject other)
		{
			Id = other.Id;
			State = other.State;
			//IsActive = other.IsActive;
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
			//IsActive = true;
		}
		#endregion

		#region основные функции
		//public virtual void Move()
		//{
		//  RunVector = Vector2.Normalize(RunVector);
		//  ShootVector = RunVector;
		//}

		public virtual void Think(List<AGameObject> players, long time)
		{}

		public virtual void Do(AGameObject obj, long time)
		{}

		public virtual Vector2 ComputeMovement(long updateDelay, GameLevel gameLevel)
		{
			// если вектор не ноль то можно делать нормирование
			if (RunVector.LengthSquared() > 0.01f)
			{
				RunVector = Vector2.Normalize(RunVector);
			}
			var newCoord = RunVector * Speed * updateDelay + Coordinates;
			return newCoord;
		}

		#endregion
	}
}
