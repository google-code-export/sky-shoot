using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Weapon
{
    public class Pistol : AWeapon
    {
        public Pistol(Guid id) : base(id) { }

        public override AProjectile[] CreateBullets(AMob owner, PointF direction)
        {
            PistolBullet[] bullets = new PistolBullet[] { new PistolBullet(owner, new Guid(), direction) };
            return bullets;
        }
    }
}