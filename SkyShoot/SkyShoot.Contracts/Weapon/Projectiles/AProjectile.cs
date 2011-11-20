using System;

using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
    [DataContract]
    public abstract class AProjectile
    {
        protected AProjectile(AMob owner, Guid id, Vector2 direction)
        {
            Owner = owner;
            Id = id;
            Direction = direction;
        }

        public AProjectile()
        {
            Owner = null;
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public AMob Owner { get; set; }

        [DataMember]
        public Vector2 Coordinates { get; set; } // вероятно, set должен быть public-методом

        [DataMember]
        public Vector2 Direction { get; set; }

        [DataMember]
        public float Speed { get; set; }

        [DataMember]
        public int LifeTime { get; set; }

        [DataMember]
        public float Damage { get; set; }

        public enum EnumBulletType
        {
            Bullet,
            Rocket,
            Flame,
        }

        [DataMember]
        public EnumBulletType Type { get; protected set; }

        protected AProjectile(AProjectile projectile)
        {
            Owner = projectile.Owner;
            Id = projectile.Id;
            Direction = projectile.Direction;
            Speed = projectile.Speed;
            Damage = projectile.Damage;
            LifeTime = projectile.LifeTime;
            Type = projectile.Type;
        }
    }
}
