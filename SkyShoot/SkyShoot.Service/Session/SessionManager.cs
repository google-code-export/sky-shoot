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
        private List<GameSession> _gameSessions;

        //Уникальный идентификатор, который присваивается каждой игре при её создании
        private int _gameID;

        public SessionManager()
        {
            _gameSessions = new List<GameSession>();
            _gameID = 1;
        }

        //Добавляем игрока в текущую игру.
        public GameSession JoinGame(GameDescription game, string playerName)
        {
            GameSession session = _gameSessions.Find(curGame => curGame.LocalGameDescription.GameID == game.GameID);

            try
            {
                if(session.LocalGameDescription.Players.Contains(playerName)){
                    session.LocalGameDescription.Players.Add(playerName);
                    return session;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        //Создаем новую игру
        public GameSession CreateGame(GameMode mode, int maxPlayers, Client.Client client,TileSet tileSet)
        {
            //List<string> playerNames;
            //playerNames = new List<string>();
            //playerNames.Add(playerName);

            List<Client.Client> clients = new List<Client.Client>();
            clients.Add(client);

            GameSession gameSession = new GameSession(tileSet, clients, maxPlayers, mode, _gameID);
            _gameSessions.Add(gameSession);

            return gameSession;
        }

        //Возвращает список игр.
        public GameDescription[] GetGameList()
        {
            var gameSessions = _gameSessions.ToArray();
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
                var game = _gameSessions.Find(gameSession => gameSession.LocalGameDescription.Players.Contains(playerName));
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