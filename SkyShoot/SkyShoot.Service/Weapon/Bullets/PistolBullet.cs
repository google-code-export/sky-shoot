using System;

using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Service.Weapon.Bullets
{
    public class PistolBullet : AProjectile
    {
        private const float SPEED = 0.1f;
        private const float DAMAGE = 2;
        private const float LIFE_DISTANCE = 3000;
        private const EnumBulletType TYPE = EnumBulletType.Bullet;

        public PistolBullet(AMob owner, Guid id, Vector2 direction)
            : base(owner, id, direction) 
        {
            Speed = SPEED;
            Damage = DAMAGE;
            LifeDistance = LIFE_DISTANCE;
            Type = TYPE;
        } 
    }
}