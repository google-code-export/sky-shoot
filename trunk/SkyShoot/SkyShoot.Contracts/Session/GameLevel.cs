using System.Runtime.Serialization;
using SkyShoot.Contracts.Service;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public class GameLevel
	{
		//public const int MOVEMENTTIME = 100; //in milliseconds
		[DataMember]
		public float LevelHeight;

		[DataMember]
		public float LevelWidth;

		public float LevelBorder;

		public GameLevel()
		{
		}

		public GameLevel(TileSet usedTileSet)
		{
			LevelHeight = 1000;
			LevelWidth = 1000;
			LevelBorder = Constants.LEVELBORDER;
			UsedTileSet = usedTileSet;
		}

		[DataMember]
		public TileSet UsedTileSet { get; set; }
	}
}
