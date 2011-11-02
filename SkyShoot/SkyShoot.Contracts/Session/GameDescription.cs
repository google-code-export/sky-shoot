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
        public GameDescription(List<string> players, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
            GameID = gameID;
            Players = players;
            MaximumPlayersAllowed = maxPlayersAllowed;
            GameType = gameType;
        }

        public GameDescription()
        {
           
        }

        [DataMember]
        public int GameID { get; set; }

        [DataMember]
        public List<string> Players { get; set; }

        [DataMember]
        public int MaximumPlayersAllowed { get; set; }

        [DataMember]
        public GameMode GameType { get; set; }
    }
}
