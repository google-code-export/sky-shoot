using System;

using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
    [DataContract]
    public abstract class AProjectile
    {
        protected AProjectile(AMob owner, Guid id, Vector2 direction,float velocity, float damage, EnumBulletType type)
        {
            Owner = owner;
            Id = id;
            Direction = direction;
            Velocity = velocity;
            Damage = damage;
            Type = type;
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
//      public Vector2 Orientation { get; set; }
        public Vector2 Direction { get; set; }

        [DataMember]
        public float Speed { get; set; }

        [DataMember]
        public int Timer { get; set; }

        [DataMember]
        public float Damage { get; set; }

        [DataMember]
        public float Velocity { get; private set; }

        public enum EnumBulletType
        {
            Bullet,
            Rocket,
            Flame,
        }

        [DataMember]
        public EnumBulletType Type { get; private set; }
    }
}
