using System;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.XNA.Framework;
using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Weapon
{
	public abstract class AWeapon : AObtainableDamageModifier
	{
		protected AWeapon(Guid id) : base(id) { Owner = null; }

		protected AWeapon(Guid id, AGameObject owner) : base(id) 
		{
			this.Owner = owner;
		}

		public abstract AProjectile[] CreateBullets(AGameObject owner, Vector2 direction);

		protected int _reloadSpeed;

		protected long _reload = 0;

		public bool Reload(long shotTime)
		{
			if (shotTime - _reloadSpeed > _reload)
			{
				System.Diagnostics.Trace.WriteLine("Player is hitted " + shotTime + " : " + _reloadSpeed);
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