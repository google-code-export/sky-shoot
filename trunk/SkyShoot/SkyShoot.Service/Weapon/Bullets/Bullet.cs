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
            _velocity = velocity;
            _damage = damage;

            Type = type;
        }

        public float _velocity { get; private set; }
        public float _damage { get; private set; }

        public enum EnumBulletType
        {
            Bullet,
            Rocket,
            Flame,
        }

        public EnumBulletType Type { get; private set; }
    }
}