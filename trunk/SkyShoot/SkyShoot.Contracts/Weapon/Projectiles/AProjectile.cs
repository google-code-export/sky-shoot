using System;

using System.Runtime.Serialization;

using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
	[DataContract]
	public class AProjectile : AGameObject
	{
		protected AProjectile(AGameObject owner, Guid id, Vector2 direction)
		{
			Owner = owner;
			Id = id;
			RunVector = direction;
			Coordinates = owner.Coordinates;
		}

		public AProjectile()
		{
			Owner = null;
		}

		public void Copy(AProjectile other)
		{
			base.Copy(other);
			//this.Coordinates = other.Coordinates;
			this.Damage = other.Damage;
			//this.RunVector = other.RunVector;
			//this.Id = other.Id;
			this.LifeDistance = other.LifeDistance;
			this.Owner = other.Owner;
			//this.Speed = other.Speed;
			
		}

		public AProjectile(AProjectile other)
		{
			Copy(other);
		}

		public override void Think(System.Collections.Generic.List<AGameObject> players = null)
		{
			this.Coordinates += RunVector * Speed;
		}

		//[DataMember]
		//public Guid Id { get; set; }

		public AGameObject Owner { get; set; }

		//[DataMember]
		//public Vector2 Coordinates;// { get; set; } // вероятно, set должен быть public-методом

		//[DataMember]
		//public Vector2 RunVector;// { get; set; }

		//[DataMember]
		//public float Speed { get; set; }

		//[DataMember]
		public float LifeDistance { get; set; }

		

		public Vector2 OldCoordinates;

		
	}
}
