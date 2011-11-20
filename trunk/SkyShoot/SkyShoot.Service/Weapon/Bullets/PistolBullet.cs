using System;

using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Service.Weapon.Bullets
{
    public class PistolBullet : AProjectile
    {
        private const float SPEED = 10;
        private const float DAMAGE = 2;
        private const int LIFE_TIME = 120;
        private const EnumBulletType TYPE = EnumBulletType.Bullet;

        public PistolBullet(AMob owner, Guid id, Vector2 direction)
            : base(owner, id, direction, SPEED, DAMAGE, LIFE_TIME, TYPE) { } 
    }
}