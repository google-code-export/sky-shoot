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
        private List<GameSession> GameSessions;

        //Уникальный идентификатор, который присваивается каждой игре при её создании
        private int GameID;

        public SessionManager()
        {
            GameSessions = new List<GameSession>();
            GameID = 1;
        }

        //Добавляем игрока в текущую игру.
        public bool JoinGame(GameDescription game, string playerName)
        {
            game = GameSessions.Find(curGame => curGame.LocalGameDescription.GameID == game.GameID).LocalGameDescription;

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
        public GameDescription CreateGame(GameMode mode, int maxPlayers, string playerName,TileSet tileSet)
        {
            List<string> playerNames;
            playerNames = new List<string>();
            playerNames.Add(playerName);

            var gameSession = new GameSession(tileSet, playerNames, maxPlayers, mode, GameID);
            GameSessions.Add(gameSession);

            return gameSession.LocalGameDescription;
        }

        //Возвращает список игр.
        public GameDescription[] GetGameList()
        {
            var gameSessions = GameSessions.ToArray();
            var gameDescriptions = new List<GameDescription>();

            for (int i = 0; i < gameSessions.Length; i++)
            {
                gameDescriptions.Add(gameSessions[i].LocalGameDescription);
            }

            return gameDescriptions.ToArray();
        }

        //Ищем игру, в которой находится игрок и удаляем его оттуда.
        public bool LeaveGame(string playerName)
        {
            try
            {
                var game = GameSessions.Find(gameSession => gameSession.LocalGameDescription.Players.Contains(playerName));
                game.LocalGameDescription.Players.Remove(playerName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}