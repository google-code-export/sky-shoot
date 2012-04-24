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
			[EnumMember]
			Pistol,
			[EnumMember]
			Shotgun,
			[EnumMember]
			RocketPistol,
			[EnumMember]
			SpiderPistol
		}
		public Guid Id { get; set; }
		public AWeaponType WeaponType { get; set; }
		
		public AGameObject Owner { get; set; }
		protected int ReloadSpeed;

		protected long Reload;

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

		public bool IsReload(long shotTime)
		{
			if (shotTime - ReloadSpeed > Reload)
			{
				// System.Diagnostics.Trace.WriteLine("Player is hitted " + shotTime + " : " + _reloadSpeed);
				Reload = shotTime;
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