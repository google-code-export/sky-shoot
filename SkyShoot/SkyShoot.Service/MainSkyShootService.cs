using System;
using System.ServiceModel;

using System.Collections.Generic;

using System.Diagnostics;
using SkyShoot.Contracts;
using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Service.Session;

namespace SkyShoot.Service
{

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AGameObject, ISkyShootService//, ISkyShootCallback
	{
		public static int globalID = 0;
		public int localID;
		//private ISkyShootCallback _callback;
		public string Name;
		public Queue<AGameEvent> NewEvents;
		public List<AGameBonus> bonuses;

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private readonly Session.SessionManager _sessionManager = Session.SessionManager.Instance;

		private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

		public MainSkyShootService() : base(new Vector2(0, 0), Guid.NewGuid()) 
		{
			IsPlayer = true;
			NewEvents = new Queue<AGameEvent>();
			localID = globalID;
			globalID ++;
 			bonuses = new List<AGameBonus>();
		}

		public void Disconnect() { this.LeaveGame(); }

		public bool Register(string username, string password)
		{
			/*bool result = _accountManager.Register(username, password);
			if(result)
			{
				Trace.WriteLine(username + "has registered");
			}
			else
			{
				Trace.WriteLine(username + "is not registered. The name of the employing or other errors");
			}
			return result; */
			return true;
		}

		public Guid? Login(string username, string password)
		{
			bool result = true;//_accountManager.Login(username, password);

			if (result)
			{
				Name = username;
				//_callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
				IsPlayer = true;

				ClientsList.Add(this);
			}
			else
			{
				return null;
			}

			return Id;
		}

		public GameDescription[] GetGameList()
		{
			return _sessionManager.GetGameList();
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tileSet)
		{
			try
			{
				var gameDescription = _sessionManager.CreateGame(mode, maxPlayers, this, tileSet);
				return gameDescription;
			}
			catch (Exception e)
			{
				Trace.Fail(this.Name + " unable to create game. " + e.Message);
				return null;
			}
		}

		public bool JoinGame(GameDescription game)
		{
			try
			{
				bool result = _sessionManager.JoinGame(game, this);
				if (result)
				{
					Trace.WriteLine(this.Name + "has joined the game ID=" + game.GameId);
				}
				else
				{
					Trace.WriteLine(this.Name + "has not joined the game ID=" + game.GameId);
				}
				return result;
			}
			catch(Exception e)
			{
				Trace.Fail(this.Name + "has not joined the game." + e.Message);
				return false;
			}
		}

		public override void Think(List<AGameObject> players)
		{
			throw new NotImplementedException();
		}

		public event SomebodyMovesHandler MeMoved;
		public event ClientShootsHandler MeShot;
		//public event SomebodySpawnsHandler MobSpawned;
		//public event SomebodyDiesHandler MobDied;

		//public Queue<AGameEvent> Move(Vector2 direction) // приходит снаружи от клиента
		public AGameEvent[] Move(Vector2 direction) // приходит снаружи от клиента
		{
			if (MeMoved != null)
			{
				MeMoved(this, direction);
			}
			return GetEvents();
		}

		public Queue<AGameEvent> Shoot(Vector2 direction)
		{
			if (MeShot != null)
			{
				MeShot(this, direction);
			}
			return null;// GetEvents();
		}

		public AGameEvent[] GetEvents()
		{
			var events = NewEvents.ToArray();//new Queue<AGameEvent>(NewEvents);
			NewEvents.Clear();
			return events;
		}
		/*
		public void TakeBonus(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			bonus.Owner.State |= bonus.Type;
		}

		public void TakePerk(Contracts.Perks.Perk perk)
		{
			throw new NotImplementedException();
		}
		*/
		public void LeaveGame()
		{
			bool result = _sessionManager.LeaveGame(this);
			if (!result)
			{
				Trace.WriteLine(Name + "left the game");
				return;
			}

			ClientsList.Remove(this);
		}

		public GameLevel GameStart(int gameId)
		{
			Trace.WriteLine("GameStarted");
			return _sessionManager.GameStarted(gameId);
		}


		public AGameObject[] SynchroFrame()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id, out session);
			if(session ==null)
			{
				return null;
			}
			return GameObjectConverter.ArrayedObjects(session.GetSynchroFrame());
		}

		public String[] PlayerListUpdate()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id, out session);
			if(session == null)
			{
				return new string[]{};
			}
			return session.LocalGameDescription.Players.ToArray();
		}

	}
}
