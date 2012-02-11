using System.Runtime.Serialization;
using System.Collections.Generic;

namespace SkyShoot.Contracts.Session
{
	/* Вопрос что использовать, строки или список я оставил открытым,
	 * т.к. не совсем понял, использовать List<string> или List<Player>?
	 * Понятно, что во втором случае, придется создавать класс описывающий
	 * игрока. Либо использовать, к примеру List<AMob>.
	 */

	[DataContract]
	public class GameDescription
	{

		[DataMember]
		public int GameId { get; set; }

		[DataMember]
		public List<string> Players { get; set; }

		[DataMember]
		public int MaximumPlayersAllowed { get; set; }

		[DataMember]
		public GameMode GameType { get; set; }

		[DataMember]
		public TileSet UsedTileSet { get; set; }

		public override string ToString()
		{
			return "[ " + UsedTileSet + " ; " + GameType + " ; " + Players.Count + "/" + MaximumPlayersAllowed + " ]";
		}

		public GameDescription(List<string> players, int maxPlayersAllowed, GameMode gameType, int gameId, TileSet usedTileSet)
		{
			GameId = gameId;
			Players = players;
			MaximumPlayersAllowed = maxPlayersAllowed;
			GameType = gameType;
			UsedTileSet = usedTileSet;
		}

		public GameDescription()
		{

		}
	}
}
