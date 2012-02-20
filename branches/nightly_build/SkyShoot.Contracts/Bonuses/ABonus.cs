using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyShoot.Contracts.Bonuses
{
    [DataContract]
    public abstract class ABonus
    {
        protected ABonus(Guid id)
        {
            Id = id;
        }

        [DataMember]
        public Guid Id { get; private set; }

        // расширить
    }
}
