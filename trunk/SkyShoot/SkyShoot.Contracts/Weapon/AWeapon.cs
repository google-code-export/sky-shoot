using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Weapon
{
	public abstract class AWeapon
	{
		protected int ReloadSpeed;

		protected long Reload;

		protected AWeapon(Guid id, AGameObject owner = null)
		{
			Id = id;
			Owner = owner;
		}

		public Guid Id { get; set; }

		public WeaponType WeaponType { get; set; }

		public AGameObject Owner { get; set; }

		public abstract AGameObject[] CreateBullets(Vector2 direction);

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
				if (projectile != null)
				{
					projectile.Damage *= damage;
					projectile.Radius *= damage;
				}
			}
		}
	}
}