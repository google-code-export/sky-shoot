using System.Runtime.Serialization;
using SkyShoot.Contracts.Service;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public class GameLevel
	{
		[DataMember]
		public float Height;

		[DataMember]
		public float Width;

		public float LevelBorder;

		public GameLevel()
		{
		}

		public GameLevel(TileSet usedTileSet)
		{
			Height = 1000;
			Width = 1000;
			LevelBorder = Constants.LEVELBORDER;
			UsedTileSet = usedTileSet;
		}

		[DataMember]
		public TileSet UsedTileSet { get; set; }
	}
}
