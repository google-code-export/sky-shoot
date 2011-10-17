using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyShoot.Contracts.Session
{
    [DataContract]
    public class GameLevel
    {
        public GameLevel(TileSet usedTileSet)
        {
            UsedTileSet = usedTileSet;
        }

        [DataMember]
        public TileSet UsedTileSet
        {
            get;
            private set;
        }
    }
}
