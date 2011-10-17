using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyShoot.Contracts.Mobs
{
    [DataContract]
    public abstract class AMob
    {
        protected AMob(PointF coordinates,Guid id)
        {
            RunVector = ShootVector = new PointF(0, 1);
            Coordinates = coordinates;
            Id = id;
        }

        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public PointF RunVector { get; protected set; }

        [DataMember]
        public PointF ShootVector { get; protected set; }

        [DataMember]
        public PointF Coordinates { get; protected set; }

        [DataMember]
        public abstract bool IsPlayer { get; }

        // расширить типом моба, размером, цветом, и т.д.
    }
}
