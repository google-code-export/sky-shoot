using System;
using Microsoft.Xna.Framework;
using SkyShoot.Game.Client.Players;

namespace SkyShoot.Game.Client.Weapon.Bullets
{
    public abstract class ABullet : AProjectile
    {

        public enum EnumBulletType
        {
            Bullet,
            Rocket,
            Flame,
        }

        public EnumBulletType Type { get; private set; }

        public float Velocity { get; private set; }

        protected ABullet(AMob owner, Guid id, Vector2 direction,
            float velocity, EnumBulletType type) : base(owner, id)
        {
            Direction = direction;
            Velocity = velocity;

            Type = type;
        }

    }
}
