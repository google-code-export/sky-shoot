using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;

namespace SkyShoot.Service.Weapon
{
    public class Pistol : AWeapon
    {
        public Pistol(Guid id) : base(id) { }

        public override AProjectile[] CreateBullets(AMob owner, Vector2 direction)
        {
            PistolBullet[] bullets = new PistolBullet[] { new PistolBullet(owner, new Guid(), direction) };
            return bullets;
        }
    }
}