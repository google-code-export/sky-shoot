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

		/// <summary>
		/// основное перечисление всех возможных типов обектов игры
		/// </summary>
		[Flags]
		public enum EnumObjectType : ulong
		{
		//!! переписать на сдвиги и номера
			/*                 76543210
			 * 64-bit xxxxxxx  00000000
			 *            \|/  ||||||||
			 *             |   |/||/|/+-- LivingObject (something with mind and health)
			 *             |   | || |  
			 *             |   | || +---- Bullets
			 *             |   | ||    
			 *             |   | ||+----- Bonuses
			 *             |   | |      
			 *             |   | +-------- Walls      
			 *             |   |
			 *             |   +--------- Mobs
			 *             |
			 *             +------------- Common attributes
			 */
			[EnumMember]
			LivingObject = 0x0001UL | Block, //1125899906842625
			[EnumMember]
			Player = LivingObject | 0x2UL,//1125899906842626
			[EnumMember]
			Mob = LivingObject | 0x4UL,//1125899906842628
			[EnumMember]
			Bullet = 0x010UL,//16
			[EnumMember]
			Flame = Bullet | 0x020UL,//48
			[EnumMember]
			PistolBullet = Bullet | 0x040UL,//80
			[EnumMember]
			ShotgunBullet = Bullet | 0x060UL,//144
			[EnumMember]
			RocketBullet = Bullet | 0x080UL,//272
			[EnumMember]
			Explosion = Bullet | 0x100UL,//528
			[EnumMember]
			SpiderBullet = Bullet | 0x120UL,//1040
			[EnumMember]
			HeaterBullet = Bullet | 0x140UL,//
			[EnumMember]
			PoisonBullet = Bullet | 0x160UL,//Правильный ли номер
			[EnumMember]
			Bonus = 0x01000UL,//4096
			[EnumMember]
			DoubleDamage = Bonus | 0x02000UL,//12288
			[EnumMember]
			Shield = Bonus | 0x04000UL,//20480
			[EnumMember]
			Remedy = Bonus | 0x08000UL,//36864
			[EnumMember]
			Mirror = Bonus | 0x06000UL,//135168
			[EnumMember]
			Speedup = Bonus | 0x10000UL,//69632
			[EnumMember]
			Wall = 0x100000UL | Block,//1125899907891200
			[EnumMember]
			Block = 0x100000000L, //0x1 << CommonAttributesShift, //1125899906842624
			[EnumMember]
			ChildrenMob = 0x02000000UL | Mob,
			[EnumMember]
			Hydra = 0x04000000UL | Mob,
			[EnumMember]
			ParentMob = 0x06000000UL | Mob,
			[EnumMember]
			Poisoner =  0x08000000UL | Mob,
			[EnumMember]
			Poisoning = 0x10000000UL | Mob,// | ~Block, //!!
			[EnumMember]
			Spider =    0x12000000UL | Mob,
			[EnumMember]
			ShootingSpider = 0x14000000UL | Mob,
			[EnumMember]
			SpiderWithMind = 0x16000000UL | Mob,
			[EnumMember]
			Turret = 0x18000000UL | Mob,

			// [EnumMember] //!!
			// Poisoning = 0x0001 | 0x500000000,//Правильный ли номер? Он живой объект, но не блок

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

		public Team TeamIdentity { set; get; }//Принадлежность команде
		
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

		public virtual IEnumerable<AGameEvent> OnDead(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return new AGameEvent[] { };
		}
		#endregion

	}
}
