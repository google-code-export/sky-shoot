using System;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Mobs
{
    [DataContract]
    public abstract class AMob
    {

        [DataMember]
        public abstract bool IsPlayer { get; }

        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public Vector2 RunVector { get; protected set; }

        [DataMember]
        public Vector2 ShootVector { get; protected set; }

        [DataMember]
        public Vector2 Coordinates { get; protected set; }

        [DataMember]
        public int HealthAmount { get; set; }

        protected AMob(Vector2 coordinates, Guid id)
        {
            RunVector = ShootVector = new Vector2(0, 1);
            Coordinates = coordinates;
            Id = id;
        }

        // расширить типом моба, размером, цветом, и т.д.
    }
}
