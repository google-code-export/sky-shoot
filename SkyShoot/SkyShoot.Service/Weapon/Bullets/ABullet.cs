using System;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using Microsoft.Xna.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
    public abstract class ABullet : AProjectile
    {
        protected ABullet(AMob owner, Guid id, Vector2 direction,
            float velocity, float damage, EnumBulletType type)
            : base(owner, id)
        {
            Direction = direction;
            Velocity = velocity;
            Damage = damage;

            Type = type;
        }

        public float Velocity { get; private set; }
        public float Damage { get; private set; }

        public enum EnumBulletType
        {
            Bullet,
            Rocket,
            Flame,
        }

        public EnumBulletType Type { get; private set; }
    }
}