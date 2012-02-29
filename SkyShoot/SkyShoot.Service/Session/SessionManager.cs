using System;

using System.Linq;

using System.Collections.Generic;

using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using System.Collections.Concurrent;

namespace SkyShoot.Service.Session
{
	public sealed class SessionManager
	{
		private static readonly SessionManager LocalInstance = new SessionManager();

		public Dictionary<Guid, GameSession> SessionTable;

		public static SessionManager Instance
		{
			get { return LocalInstance; }
		}

		//Содержит текущие игры
		private readonly List<GameSession> _gameSessions;

		//Уникальный идентификатор, который присваивается каждой игре при её создании
		private int _gameId;

		private SessionManager()
		{
			SessionTable = new Dictionary<Guid,GameSession>();
			_gameSessions = new List<GameSession>();
			_gameId = 1;
		}

		//Добавляем игрока в текущую игру.
		public bool JoinGame(GameDescription game, MainSkyShootService player)
		{
			GameSession session = _gameSessions.Find(curGame => curGame.LocalGameDescription.GameId == game.GameId);
			SessionTable.Add(player.Id, session);
			return session.AddPlayer(player);

		}
		
		//Создаем новую игру
		public GameDescription CreateGame(GameMode mode, int maxPlayers, MainSkyShootService client, TileSet tileSet)
		{
			var gameSession = new GameSession(tileSet, maxPlayers, mode, _gameId);
			_gameSessions.Add(gameSession);
			SessionTable.Add(client.Id, gameSession);
			_gameId++;
			gameSession.AddPlayer(client);
			
			return gameSession.LocalGameDescription;
		}

	   //Возвращает список игр.
		public GameDescription[] GetGameList()
		{
			var gameSessions = _gameSessions.ToArray();

			return (from t in gameSessions where !t.IsStarted select t.LocalGameDescription).ToArray();
		}

		//Ищем игру, в которой находится игрок и удаляем его оттуда.
		public bool LeaveGame(MainSkyShootService player)
		{
			try
			{
				GameSession game;
				SessionTable.TryGetValue(player.Id,out game);
				var leavingPlayer = player;
				game.PlayerLeave(leavingPlayer);
				if (game.Players.Count == 0)
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
		//временно
		public GameLevel GameStarted(int gameId)
		{
			var game = _gameSessions.Find(x => x.LocalGameDescription.GameId == gameId);
			return game.IsStarted ? game.GameLevel : null;
		}
	}
}