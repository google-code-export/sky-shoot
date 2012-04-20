using System;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Weapon
{
	public abstract class AWeapon
	{
		[Flags]
		public enum AWeaponType
		{
			//[EnumMember]
			//DoubleDamage,
			//[EnumMember]
			//Shield,
			[EnumMember]
			Pistol,
			[EnumMember]
			Shotgun,
			[EnumMember]
			RocketPistol
		}
		public Guid Id { get; set; }
		public AWeaponType WeaponType { get; set; }
		
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

		public abstract AGameObject[] CreateBullets(AGameObject owner, Vector2 direction);

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

		public void ApplyModifier(AGameObject[] projectiles, float damage)
		{
			foreach (AGameObject projectile in projectiles)
			{
				if(projectile==null)
					continue;
				projectile.Damage *= damage;
				projectile.Radius *= damage;
			}
		}

	}
}