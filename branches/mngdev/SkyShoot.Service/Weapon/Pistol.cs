using System;

using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Service.Weapon
{
	public class Pistol : AWeapon
	{
		public Pistol(Guid id) : base(id) { this.Type = AObtainableDamageModifiers.Pistol; _reloadSpeed = SkyShoot.Contracts.Constants.PISTOL_ATTACK_RATE; }

		public Pistol(Guid id, AGameObject owner) : base(id, owner) { this.Type = AObtainableDamageModifiers.Pistol; _reloadSpeed = SkyShoot.Contracts.Constants.PISTOL_ATTACK_RATE; }

		public override AProjectile[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			PistolBullet[] bullets = new PistolBullet[] { new PistolBullet(owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}