using System;
using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Service.Session
{
	public sealed class SessionManager
	{
		private static readonly SessionManager localInstance = new SessionManager();

		public Dictionary<Guid, GameSession> SessionTable;

        /// <summary>
        /// Содержит текущие игры
        /// </summary>
		private readonly List<GameSession> _gameSessions;

        /// <summary>
        /// Уникальный идентификатор, который присваивается каждой игре при её создании
        /// </summary>
		private int _gameId;

		private SessionManager()
		{
		    SessionTable = new Dictionary<Guid, GameSession>();
			_gameSessions = new List<GameSession>();
			_gameId = 1;
		}

        public static SessionManager Instance
        {
            get { return localInstance; }
        }

        /// <summary>
        /// Добавляем игрока в текущую игру.
        /// </summary>
		public bool JoinGame(GameDescription game, MainSkyShootService player)
		{
			GameSession session = _gameSessions.Find(curGame => curGame.LocalGameDescription.GameId == game.GameId);
			SessionTable.Add(player.Id, session);
			return session.AddPlayer(player);

		}
		
		/// <summary>
        /// Создаем новую игру
		/// </summary>
		public GameDescription CreateGame(GameMode mode, int maxPlayers, MainSkyShootService client, TileSet tileSet, int teams)
		{
			var gameSession = new GameSession(tileSet, maxPlayers, mode, _gameId, teams);
			_gameSessions.Add(gameSession);
			SessionTable.Add(client.Id, gameSession);
			_gameId++;
			gameSession.AddPlayer(client);
			
			return gameSession.LocalGameDescription;
		}

	    /// <summary>
        /// Возвращает список игр.
	    /// </summary>
		public GameDescription[] GetGameList()
		{
			var gameSessions = _gameSessions.ToArray();

			return (from t in gameSessions where !t.IsStarted select t.LocalGameDescription).ToArray();
		}

		/// <summary>
        /// Ищем игру, в которой находится игрок и удаляем его оттуда.
		/// </summary>
		public bool LeaveGame(MainSkyShootService player)
		{
			try
			{
				GameSession game;
				SessionTable.TryGetValue(player.Id,out game);
				var leavingPlayer = player;
				game.PlayerLeave(leavingPlayer);
				if (game.PlayersCount() == 0)
				{
					game.Stop();
					_gameSessions.Remove(game);
					
				}
				SessionTable.Remove(player.Id);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		
		public GameLevel GameStarted(int gameId)
		{
		    var game = _gameSessions.Find(x => x.LocalGameDescription.GameId == gameId);
		    return game != null ? (game.IsStarted ? game.GameLevel : null) : null;
		}
	}
}