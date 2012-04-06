using System;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Weapon
{
	public abstract class AWeapon //: AObtainableDamageModifier
	{
		[Flags]
		public enum AObtainableDamageModifiers
		{
			[EnumMember]
			DoubleDamage,
			[EnumMember]
			Shield,
			[EnumMember]
			Pistol,
			[EnumMember]
			Shotgun
		}
		public Guid Id { get; set; }
		public AObtainableDamageModifiers WheaponType { get; set; }
		
		public AGameObject Owner { get; set; }
		protected AWeapon(Guid id) 
		{
			Owner = null;
			Id = id;
		}

		protected AWeapon(Guid id, AGameObject owner) 
		{
			Id = id;
			Owner = owner;
		}

		public abstract AProjectile[] CreateBullets(AGameObject owner, Vector2 direction);

		protected int ReloadSpeed;

		protected long _reload;

		public bool Reload(long shotTime)
		{
			if (shotTime - ReloadSpeed > _reload)
			{
				// System.Diagnostics.Trace.WriteLine("Player is hitted " + shotTime + " : " + _reloadSpeed);
				_reload = shotTime;
				return true;
			}
			return false;
		}

		public void ApplyModifier(AProjectile[] projectiles, float damage)
		{
			foreach (AProjectile projectile in projectiles)
			{
				projectile.Damage *= damage;
			}
		}

	}
}