using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Bonuses.Weapon.Projectiles;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Weapon.Bullets
{
    public abstract class ABullet : AProjectile
    {
        protected ABullet(AMob owner, Guid id, PointF direction,
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