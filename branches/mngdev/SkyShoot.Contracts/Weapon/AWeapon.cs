using System;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Weapon
{
	public abstract class AWeapon : AObtainableDamageModifier
	{
		protected AWeapon(Guid id) : base(id) { Owner = null; }

		protected AWeapon(Guid id, AGameObject owner) : base(id) 
		{
			Owner = owner;
		}

		public abstract AProjectile[] CreateBullets(AGameObject owner, Vector2 direction);

		protected int ReloadSpeed;

		protected long _reload;

		public bool Reload(long shotTime)
		{
			if (shotTime - ReloadSpeed > _reload)
			{
				System.Diagnostics.Trace.WriteLine("Player is hitted " + shotTime + " : " + ReloadSpeed);
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