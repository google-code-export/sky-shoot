using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Service.Session
{
    public class SessionManager
    {
        //Содержит текущие игры
        private List<GameDescription> GameDescriptions;

        //Уникальный идентификатор, который присваивается каждой игре при её создании
        private int GameID;

        public SessionManager()
        {
            GameDescriptions = new List<GameDescription>();
            GameID = 1;
        }

        //Добавляем игрока в текущую игру.
        public bool JoinGame(GameDescription game, string playerName)
        {
            game = GameDescriptions.Find(curGame => curGame.GameID == game.GameID);

            try
            {
                if(game.Players.Contains(playerName)){
                    game.Players.Add(playerName);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Создаем новую игру
        public GameDescription CreateGame(GameMode mode, int maxPlayers, string playerName)
        {
            List<string> playerNames;
            playerNames = new List<string>();
            playerNames.Add(playerName);

            var gameDescription = new GameDescription(playerNames, maxPlayers, mode, GameID++);
            GameDescriptions.Add(gameDescription);

            return gameDescription;
        }

        //Возвращает список игр.
        public GameDescription[] GetGameList()
        {
            return GameDescriptions.ToArray();
        }

        //Ищем игру, в которой находится игрок и удаляем его оттуда.
        public bool LeaveGame(string playerName)
        {
            try
            {
                var game = GameDescriptions.Find(gameDescription => gameDescription.Players.Contains(playerName));
                game.Players.Remove(playerName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}