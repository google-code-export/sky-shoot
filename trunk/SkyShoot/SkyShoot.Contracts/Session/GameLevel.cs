﻿using System.Runtime.Serialization;
using SkyShoot.Contracts.Service;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public class GameLevel
	{
		//public const int MOVEMENTTIME = 100; //in milliseconds
		[DataMember]
		public float levelHeight; 

		[DataMember]
		public float levelWidth;

		public float LEVELBORDER;

        public GameLevel()
        {
        }

		public GameLevel(TileSet usedTileSet)
		{
			levelHeight = 1000;
			levelWidth = 1000;
			LEVELBORDER = Constants.LEVELBORDER;
			UsedTileSet = usedTileSet;
		}

		[DataMember]
		public TileSet UsedTileSet { get; set; }

	}
}
