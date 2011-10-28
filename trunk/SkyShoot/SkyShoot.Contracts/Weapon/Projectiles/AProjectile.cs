using System;
using System.Drawing;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Mobs;

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

        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public AMob Owner { get; private set; }

        [DataMember]
        public PointF Coordinates { get; protected set; } // вероятно, set должен быть public-методом

        [DataMember]
//        public PointF Orientation { get; protected set; }
        public PointF Direction { get; protected set; }
    }
}
