using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;

namespace SkyShootService.Session
{
    public class SessionManager
    {
        private List<GameDescription> GameDescriptions;

        private int GameID;

        public SessionManager()
        {
            GameDescriptions = new List<GameDescription>();
            GameID = 1;
        }

        public bool JoinGame(GameDescription game, string PlayerName)
        {
            try
            {
                game.Players[game.Players.Length] = PlayerName;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public GameDescription CreateGame(GameMode mode, int maxPlayers, string playerName)
        {
            /* IMHO лучше использовать List<string>, чем string[]. Т.к. это очень сильно 
             * упростит программирование и удаление отключившихся игроков. А так же работу метода JoinGame
             */

            string[] PlayerNames;
            PlayerNames = new string[maxPlayers];
            PlayerNames[0] = playerName;

            var gameDescription = new GameDescription(PlayerNames, maxPlayers, mode, GameID++);
            GameDescriptions.Add(gameDescription);

            return gameDescription;
        }

        public GameDescription[] GetGameList()
        {
            return GameDescriptions.ToArray();
        }
    }
}