using System;

using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Service.Weapon.Bullets
{
    public class ShotgunBullet : AProjectile
    {
        private const float SPEED = 10;
        private const float DAMAGE = 2;
        private const float LIFE_DISTANCE= 3000;
        private const EnumBulletType TYPE = EnumBulletType.Bullet;

        public ShotgunBullet(AMob owner, Guid id, Vector2 direction)
            : base(owner, id, direction) 
        {
            Speed = SPEED;
            Damage = DAMAGE;
			LifeDistance = LIFE_DISTANCE;
            Type = TYPE;
        }
    }
}