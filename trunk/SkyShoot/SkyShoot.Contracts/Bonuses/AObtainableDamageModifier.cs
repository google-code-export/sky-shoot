using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyShoot.Contracts.Bonuses
{
    [DataContract]
    public abstract class AObtainableDamageModifier
    {
        protected AObtainableDamageModifier(Guid id)
        {
            Id = id;
        }

        public AObtainableDamageModifier()
        {
      
        }

        [DataMember]
        public Guid Id { get; set; }

        // расширить
    }
}
