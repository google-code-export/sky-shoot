using System;

using System.Linq;

using System.Collections.Generic;

using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using System.Collections.Concurrent;
using System.Collections;

namespace SkyShoot.Service.Session
{
	public sealed class SessionManager
	{
		//private static readonly SessionManager LocalInstance = new SessionManager();

		public Dictionary<Guid, GameSession> SessionTable;
		public static List<SessionManager> Instances = new List<SessionManager>() { 
										new SessionManager(Guid.NewGuid()),new SessionManager(Guid.NewGuid())};
		/*
		public static SessionManager Instance
		{
			get { return LocalInstance; }
		}
		*/
		//Содержит текущие игры
		private readonly List<GameSession> _gameSessions;

		//Уникальный идентификатор, который присваивается каждой игре при её создании
		private int _gameId;
		public Guid ManagerId;

		protected SessionManager(Guid guid)
		{
			ManagerId = guid;
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
		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tileSet)
		{
			var gameSession = new GameSession(tileSet, maxPlayers, mode, _gameId);
			_gameSessions.Add(gameSession);
			//SessionTable.Add(client.Id, gameSession);
			_gameId++;
			//gameSession.AddPlayer(client);
			
			return gameSession.LocalGameDescription;
		}

	   //Возвращает список игр.
		public IEnumerable<GameDescription> GetGameList()
		{
			var gameSessions = _gameSessions.ToArray();

			return (from t in gameSessions where !t.IsStarted select t.LocalGameDescription);
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
		
		public GameLevel GameStarted(int gameId)
		{
			var game = _gameSessions.Find(x => x.LocalGameDescription.GameId == gameId);
			return game.IsStarted ? game.GameLevel : null;
		}
	}
}