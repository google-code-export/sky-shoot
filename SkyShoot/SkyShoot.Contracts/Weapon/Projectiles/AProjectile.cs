using System;
using System.Drawing;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Mobs;

using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
    [DataContract]
    public abstract class AProjectile
    {
        protected AProjectile(AMob owner, Guid id)
        {
            Owner = owner;
            Id = id;
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
    }
}
