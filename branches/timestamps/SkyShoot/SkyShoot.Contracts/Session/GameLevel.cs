using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public class GameLevel
	{
		[DataMember]
		public int Height { get; private set; }

		[DataMember]
		public int Width { get; private set; }

		[DataMember]
		public TileSet UsedTileSet { get; private set; }

		public GameLevel(int width, int height, TileSet usedTileSet)
		{
			Width = width;
			Height = height;
			UsedTileSet = usedTileSet;
		}
	}
}
