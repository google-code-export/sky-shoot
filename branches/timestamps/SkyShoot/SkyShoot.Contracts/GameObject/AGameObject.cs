using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Weapon;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.GameObject
{
	[DataContract]
	public class AGameObject
	{
		/// <summary>
		/// основное перечисление всех возможных типов обектов игры
		/// </summary>
		[Flags]
		public enum EnumObjectType : ulong
		{
			//!! переписать на сдвиги и номера
			/*                 76543210
			 * 64-bit  0000000 00000000
			 *         |/\\|/\ /|||||||
			 *         |  \|  | \|/\|/+-- LivingObject (something with mind and health)
			 *         |   |  |  |  |  
			 *         |   |  |  |  +---- Bullets
			 *         |   |  |  |    
			 *         |   |  |  |
			 *         |   |  |  +------- Bonuses
			 *         |   |  |        
			 *         |   |  +---------- Walls      
			 *         |   | 
			 *         |   |
			 *         |   +------------- Mobs
			 *         |    
			 *         +----------------- Common attributes
			 */

			[EnumMember]//!!
			Block = 0x020000000000000L, //0x1 << CommonAttributesShift, //1125899906842624
			[EnumMember]
			LivingObject = 0x0001UL | Block, //1125899906842625
			[EnumMember]
			Player = LivingObject | 0x2UL, //1125899906842626
			[EnumMember]
			Mob = LivingObject | 0x4UL, //1125899906842628

			[EnumMember]
			Bullet = 0x0010UL, //16
			[EnumMember]
			Flame = Bullet | 0x0020UL, //48
			[EnumMember]
			PistolBullet = Bullet | 0x0040UL, //80
			[EnumMember]
			ShotgunBullet = Bullet | 0x0080UL, //144
			[EnumMember]
			RocketBullet = Bullet | 0x0100UL, //272
			[EnumMember]
			Explosion = Bullet | 0x0200UL, //528
			[EnumMember]
			SpiderBullet = Bullet | 0x0400UL, //1040
			[EnumMember]
			HeaterBullet = Bullet | 0x0800UL, //
			[EnumMember]
			PoisonBullet = Bullet | 0x1000UL, //Правильный ли номер
			[EnumMember]
			TurretGunBullet = Bullet | 0x2000UL, //Правильный ли номер

			[EnumMember]
			Bonus = 0x0010000UL, //4096
			[EnumMember]
			DoubleDamage = Bonus | 0x0020000UL, //12288
			[EnumMember]
			Shield = Bonus | 0x0040000UL, //20480
			[EnumMember]
			Remedy = Bonus | 0x0080000UL, //36864
			[EnumMember]
			Mirror = Bonus | 0x0100000UL, //135168
			[EnumMember]
			Speedup = Bonus | 0x0200000UL, //69632

			[EnumMember]
			Wall = 0x01000000UL | Block, //1125899907891200
			[EnumMember]
			Brick = 0x02000000UL | Wall, 

			[EnumMember]
			ChildrenMob = 0x0002000000000UL | Mob,

			[EnumMember]
			Hydra = 0x0004000000000UL | Mob,

			[EnumMember]
			ParentMob = 0x0008000000000UL | Mob,

			[EnumMember]
			Poisoner = 0x0010000000000UL | Mob,

			[EnumMember]
			Poisoning = 0x0020000000000UL | Mob, // | ~Block, //!!
			[EnumMember]
			Spider = 0x0040000000000UL | Mob,

			[EnumMember]
			ShootingSpider = 0x0080000000000UL | Mob,

			[EnumMember]
			SpiderWithMind = 0x0100000000000UL | Mob,

			[EnumMember]
			Turret = 0x0200000000000UL | Mob,
			[EnumMember]
			Caterpillar = 0x0400000000000UL | Mob,

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
			get { return Is(EnumObjectType.Bullet); }
		}

		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public EnumObjectType ObjectType { get; set; }

		//[DataMember] //!! переименовать в IsAlive, ввести в енум IsActive. может ли объект быть аквтиным
		public virtual bool IsActive
		{
			get { return HealthAmount > 0; }
			set { if (!value) HealthAmount = -1; }
		}

		public Team TeamIdentity { set; get; } //Принадлежность команде

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
		public Vector2 RunVector; // { get; set; }

		[DataMember]
		public Vector2 ShootVector; // { get; set; }

		[DataMember]
		public Vector2 Coordinates;

		public Vector2 PrevMoveDiff;

		[DataMember]
		public float Radius
		{
			get { return Bounding.Radius; }
			set { Bounding.Radius = value; }
		}

		// размер моба

		//Границы
		[DataMember]
		public Bounding Bounding { get; set; }

		[DataMember]
		public virtual float Speed { get; set; }

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

		public virtual IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects,
													 long time)
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
			//lock (Weapon)
			{
				if (Weapons.ContainsKey(type))
				{
					Weapon = Weapons[type];
				}
			}
		}

		public virtual IEnumerable<AGameEvent> OnDead(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return new AGameEvent[] { };
		}

		#endregion
	}
}
