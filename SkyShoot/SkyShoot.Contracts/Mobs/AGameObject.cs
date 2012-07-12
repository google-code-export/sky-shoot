using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Mobs
{
	[DataContract]
	public class AGameObject
	{
		const int CommonAttributesShift = 32;
		int team = 0;//Определяет команду. Нечеловеки относятся к 0. Потом можно переписать на структуру принадлежности.
		/// <summary>
		/// основное перечисление всех возможных типов обектов игры
		/// </summary>
		[Flags]
		public enum EnumObjectType : ulong
		{
			/*
			 * 64-bit 00000000 00000000
			 *            \|/  ||||||||
			 *             |   ||/|/|/+-- LivingObject (something with mind and health)
			 *             |   || | |  
			 *             |   || | +---- Bullets
			 *             |   || |    
			 *             |   || |+----- Bonuses
			 *             |   ||      
			 *             |   |+-------- Walls      
			 *             |   |
			 *             |   +--------- Reserved
			 *             |
			 *             +------------- Common attributes
			 */
			[EnumMember]
			LivingObject = 0x0001 | Block, //1125899906842625
			[EnumMember]
			Player = LivingObject | 0x2,//1125899906842626
			[EnumMember]
			Mob = LivingObject | 0x4,//1125899906842628
			[EnumMember]
			Bullet = 0x010,//16
			[EnumMember]
			Flame = Bullet | 0x020,//48
			[EnumMember]
			PistolBullet = Bullet | 0x040,//80
			[EnumMember]
			ShotgunBullet = Bullet | 0x080,//144
			[EnumMember]
			RocketBullet = Bullet | 0x100,//272
			[EnumMember]
			Explosion = Bullet | 0x200,//528
			[EnumMember]
			SpiderBullet = Bullet | 0x400,//1040
			[EnumMember]
			HeaterBullet = Bullet | 0x800,//
			[EnumMember]
			PoisonBullet = Bullet | 0x500,//Правильный ли номер
			[EnumMember]
			Poisoning = 0x0001 | 0x500000000,//Правильный ли номер? Он живой объект, но не блок
			[EnumMember]
			PoisonTickBullet = Bullet | 0x600, //Правильный ли номер
			[EnumMember]
			Bonus = 0x01000,//4096
			[EnumMember]
			DoubleDamage = Bonus | 0x02000,//12288
			[EnumMember]
			Shield = Bonus | 0x04000,//20480
			[EnumMember]
			Remedy = Bonus | 0x08000,//36864
			[EnumMember]
			Speedup = Bonus | 0x10000,//69632
			[EnumMember]
			Mirror = Bonus | 0x20000,//135168
			[EnumMember]
			Wall = 0x0100000 | Block,//1125899907891200
			[EnumMember]
			Block = 100000000//0x1 << CommonAttributesShift, //1125899906842624
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
			set { if (!value) HealthAmount = -1; }
		}

		public int GetTeam()
		{
			return team;
		}

		public void SetTeam(int newTeam)
		{
			team = newTeam;
		}

		#endregion

		#region здоровье и урон
		[DataMember]
		public float Damage { get; set; }

		public AWeapon Weapon { get; set; }

		public Dictionary<WeaponType, AWeapon> Weapons { get; set; }

		[DataMember]
		public float HealthAmount { get; set; }

		[DataMember]
		public float MaxHealthAmount { get; set; }

		//[DataMember]
		//public AObtainableDamageModifier.WeaponType State { get; set; }

		#endregion

		#region перемещение и геометрия
		[DataMember]
		public Vector2 RunVector;// { get; set; }

		[DataMember]
		public Vector2 ShootVector;// { get; set; }

		[DataMember]
		public Vector2 Coordinates;

		public Vector2 PrevMoveDiff;

		[DataMember]
		public float Radius
		{
			get
			{
				return Bounding.Radius;
			}
			set
			{
				Bounding.Radius = value;
			}
		} // размер моба

		//Границы
		[DataMember]
		public Bounding Bounding { get; set; }

		protected float _speed;

		[DataMember]
		virtual public float Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		//скорость: пикселы в миллисекунду

		#endregion


		// расширить типом моба, размером, цветом, и т.д.
		#endregion

		#region служебные функции (конструкторы)
		public AGameObject(Vector2 coordinates, Guid id)
		{
			RunVector = ShootVector = new Vector2(0, 1);
			Coordinates = coordinates;
			Id = id;

			Bounding = new BoundingCircle();

			Weapons = new Dictionary<WeaponType, AWeapon>();
			//IsActive = true;
		}

		public virtual void Copy(AGameObject other)
		{
			Id = other.Id;
			//State = other.State;
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

			Bounding = other.Bounding;
		}

		public AGameObject()
		{
			Bounding = new BoundingCircle();
			//IsActive = true;
		}
		#endregion

		#region основные функции
		public virtual IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			return new AGameEvent[] { };
		}

		public virtual IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return new AGameEvent[] { };
		}

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

		public void ChangeWaponTo(WeaponType type)
		{
			if (Weapons.ContainsKey(type))
			{
				Weapon = Weapons[type];
			}
		}

		/*public virtual IEnumerable<AGameEvent> OnDead(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return new AGameEvent[] { };
		}*/
		#endregion

	}
}
