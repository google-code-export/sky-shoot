using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Session
{
    [DataContract]
    public class GameDescription
    {
        public GameDescription(string[] players, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
            GameID = gameID;
            Players = players;
            MaximumPlayersAllowed = maxPlayersAllowed;
            GameType = gameType;
        }

        [DataMember]
        public int GameID { get; private set; }

        [DataMember]
        public string[] Players { get; private set; }

        [DataMember]
        public int MaximumPlayersAllowed { get; private set; }

        [DataMember]
        public GameMode GameType { get; private set; }
    }
}
