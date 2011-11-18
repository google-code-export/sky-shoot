using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Service.Session
{
    public class SessionManager
    {
        private static SessionManager _instance = null;

        public static SessionManager Instance
        {
            get
            {
                return _instance??(_instance = new SessionManager());
            }
        }

        //Содержит текущие игры
        private List<GameSession> _gameSessions;

        //Уникальный идентификатор, который присваивается каждой игре при её создании
        private int _gameID;

        private SessionManager()
        {
            _gameSessions = new List<GameSession>();
            _gameID = 1;
        }

        //Добавляем игрока в текущую игру.
        public bool JoinGame(GameDescription game, MainSkyShootService player)
        {
            GameSession session = _gameSessions.Find(curGame => curGame.LocalGameDescription.GameID == game.GameID);
			return session.AddPlayer(player);


            /*try
            {
                if(!session.LocalGameDescription.Players.Contains(player.Name)){
                    session.LocalGameDescription.Players.Add(player.Name);
                    session.players.Add(player);

                    //Т.к. наша игра сама решает, когда начать игру, то запускаем игру.
                    if (session.players.Count == session.LocalGameDescription.MaximumPlayersAllowed)
                    {
                        if (!StartGame(session.LocalGameDescription))
                        {
                            return false;
                        }
                    }

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }*/
        }
        
        //Создаем новую игру
        public GameDescription CreateGame(GameMode mode, int maxPlayers, MainSkyShootService client, TileSet tileSet)
        {

            GameSession gameSession = new GameSession(tileSet, maxPlayers, mode, _gameID);
            _gameSessions.Add(gameSession);

			gameSession.AddPlayer(client);
            //Т.к. наша игра сама решает, когда начать игру, то запускаем игру.
            /*if (maxPlayers == 1)
            {
                StartGame(gameSession.LocalGameDescription);
            }*/

            return gameSession.LocalGameDescription;
        }

        public bool StartGame(GameDescription game)
        {
            GameSession session = _gameSessions.Find(curGame => curGame.LocalGameDescription.GameID == game.GameID);

            //Вернет false если игра уже началась.
            return session.Start();
        }

        //Возвращает список игр.
        public GameDescription[] GetGameList()
        {
            var gameSessions = _gameSessions.ToArray();
            var gameDescriptions = new List<GameDescription>();

            for (int i = 0; i < gameSessions.Length; i++)
            {
                if (gameSessions[i].IsStarted) continue;
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
                game.players.RemoveAll(x => x.Name == playerName);
                if (game.players.Count == 0) _gameSessions.Remove(game);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}