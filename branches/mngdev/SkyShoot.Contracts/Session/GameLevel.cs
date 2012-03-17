using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

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
