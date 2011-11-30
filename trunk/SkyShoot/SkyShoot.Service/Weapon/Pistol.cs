using System;

using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Service.Weapon
{
    public class Pistol : AWeapon
    {
		public Pistol(Guid id) : base(id) { this.Type = AObtainableDamageModifiers.Pistol; }

		public Pistol(Guid id, AMob owner) : base(id, owner) { this.Type = AObtainableDamageModifiers.Pistol; }

        public override AProjectile[] CreateBullets(AMob owner, Vector2 direction)
        {
            PistolBullet[] bullets = new PistolBullet[] { new PistolBullet(owner, Guid.NewGuid(), direction) };
            return bullets;
        }
	}
}