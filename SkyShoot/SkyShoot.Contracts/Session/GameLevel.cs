using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Session
{
    [DataContract]
    public class GameLevel
    {
		//public const int MOVEMENTTIME = 100; //in milliseconds
		public float levelHeight = 1000; 
		public float levelWidth = 1000;
		public float LEVELBORDER = Constants.LEVELBORDER;
		
        public GameLevel(TileSet usedTileSet)
        {
            UsedTileSet = usedTileSet;
        }
        
        public GameLevel()
        {
            
        }

        [DataMember]
        public TileSet UsedTileSet { get; set; }

    }
}
