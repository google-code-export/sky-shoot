using System.Runtime.Serialization;

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
