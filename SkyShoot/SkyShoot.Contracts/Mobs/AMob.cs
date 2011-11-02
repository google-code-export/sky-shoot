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
        public Guid Id { get; set; }

        [DataMember]
        public Vector2 RunVector { get; set; }

        [DataMember]
        public Vector2 ShootVector { get; set; }

        [DataMember]
        public Vector2 Coordinates { get; set; }

        [DataMember]
        public int HealthAmount { get; set; }

		[DataMember]
		public float Radius { get; set; } // размер моба

		[DataMember]
		public float Speed { get; set; } //скорость: пикселы в миллисекунду

        protected AMob(Vector2 coordinates, Guid id)
        {
            RunVector = ShootVector = new Vector2(0, 1);
            Coordinates = coordinates;
            Id = id;	
        }

        public AMob()
        {
            	
        }

        // расширить типом моба, размером, цветом, и т.д.
    }
}
