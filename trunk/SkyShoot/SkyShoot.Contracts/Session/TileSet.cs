using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyShoot.Contracts.Session
{
    [DataContract]
    public enum TileSet
    {
        [EnumMember]
        Snow,
        [EnumMember]
        Lava,
        [EnumMember]
        Grass,
        [EnumMember]
        Sand,
    }
}
