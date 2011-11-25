using System;

using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
    [DataContract]
    public class AProjectile
    {
        protected AProjectile(AMob owner, Guid id, Vector2 direction)
        {
            Owner = owner;
            Id = id;
            Direction = direction;
        }

        protected AProjectile()
        {
            Owner = null;
        }

				public void Copy(AProjectile other)
				{
					this.Coordinates = other.Coordinates;
					this.Damage = other.Damage;
					this.Direction = other.Direction;
					this.Id = other.Id;
					this.LifeTime = other.LifeTime;
					this.Owner = other.Owner;
					this.Speed = other.Speed;
					this.Type = other.Type;
				}

				public AProjectile(AProjectile other)
				{
					Copy(other);
				}

        [DataMember]
        public Guid Id { get; set; }

        public AMob Owner { get; set; }

        [DataMember]
        public Vector2 Coordinates;// { get; set; } // вероятно, set должен быть public-методом

        [DataMember]
        public Vector2 Direction;// { get; set; }

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
        public EnumBulletType Type { get; set; }
    }
}
