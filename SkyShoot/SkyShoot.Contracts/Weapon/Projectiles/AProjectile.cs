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
			Damage = other.Damage;
			LifeDistance = other.LifeDistance;
			Owner = other.Owner;
		}

		public AProjectile(AProjectile other)
		{
			Copy(other);
		}

		public override void Think(System.Collections.Generic.List<AGameObject> players = null)
		{
			Coordinates += RunVector * Speed;
		}

		public AGameObject Owner { get; set; }

		public float LifeDistance { get; set; }

		public Vector2 OldCoordinates;
	}
}
