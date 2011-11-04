using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Bonuses
{
    [DataContract]
    public abstract class AObtainableDamageModifier
    {

        [Flags]
        public enum AObtainableDamageModifiers
        {
            [EnumMember]
            DoubleDamage,
            [EnumMember]
            Shield,
            [EnumMember]
            Pistol,
            [EnumMember]
            Shotgun
        }

        [DataMember]
        public Guid Id { get; set; }

        public AMob Owner { get; set; }

        public AObtainableDamageModifiers Type;

        protected AObtainableDamageModifier(Guid id)
        {
            Id = id;
        }

        public AObtainableDamageModifier()
        {      
        }

        // расширить
    }
}
