﻿using System;
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
		public float levelHeight; 
		public float levelWidth;
		public float LEVELBORDER;
		
        public GameLevel(TileSet usedTileSet)
        {
			levelHeight = 1000;
			levelWidth = 1000;
			LEVELBORDER = Constants.LEVELBORDER;
            UsedTileSet = usedTileSet;
        }
        
        public GameLevel()
        {
            
        }

        [DataMember]
        public TileSet UsedTileSet { get; set; }

    }
}
