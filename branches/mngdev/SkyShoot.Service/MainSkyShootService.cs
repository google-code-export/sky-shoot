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
		public static int GlobalID = 0;
		public int LocalID;
		//private ISkyShootCallback _callback;
		public string Name;
		public Queue<AGameEvent> NewEvents;
		public List<AGameBonus> Bonuses;

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private readonly SessionManager _sessionManager = SessionManager.Instance;

		private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

		public MainSkyShootService() : base(new Vector2(0, 0), Guid.NewGuid()) 
		{
			ObjectType = EnumObjectType.Player;
			NewEvents = new Queue<AGameEvent>();
			LocalID = GlobalID;
			GlobalID ++;
 			Bonuses = new List<AGameBonus>();
		}

		public void AddBonus(AGameBonus bonus)
		{
			if (bonus.Is(EnumObjectType.Remedy))
			{
				var health = HealthAmount;
				var potentialHealth = health + health * bonus.DamageFactor;
				HealthAmount = potentialHealth > MaxHealthAmount ? MaxHealthAmount : potentialHealth;
				return;
			}
			Bonuses.RemoveAll(b => b.ObjectType == bonus.ObjectType);
			Bonuses.Add(bonus);
		}

		public AGameBonus GetBonus(EnumObjectType bonusType)
		{
			foreach (AGameBonus bonus in Bonuses)
			{
				if (bonus.ObjectType.Equals(bonusType))
				{
					return bonus;
				}
			}
			return null;
		}

		public void Disconnect() { LeaveGame(); }

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
				ObjectType = EnumObjectType.Player;

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
				Trace.Fail(Name + " unable to create game. " + e.Message);
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
					Trace.WriteLine(Name + " has joined the game ID = " + game.GameId);
				}
				else
				{
					Trace.WriteLine(Name + " has not joined the game ID = " + game.GameId);
				}
				return result;
			}
			catch(Exception e)
			{
				Trace.Fail(Name + " has not joined the game. " + e.Message);
				return false;
			}
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
			// Trace.WriteLine("GameStarted");
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

		public override Vector2 ComputeMovement(long updateDelay, GameLevel gameLevel)
		{
			var newCoord = base.ComputeMovement(updateDelay, gameLevel);
			newCoord.X = MathHelper.Clamp(newCoord.X, 0, gameLevel.levelHeight);
			newCoord.Y = MathHelper.Clamp(newCoord.Y, 0, gameLevel.levelWidth);

			return newCoord;
		}

		public override void Do(AGameObject obj, long time)
		{
			base.Do(obj, time);
			if(obj.Is(EnumObjectType.Bonus))
			{
				obj.IsActive = false;
				var bonus = new AGameBonus(obj);
				//bonus.Copy(obj);
				Bonuses.Add(bonus);
				bonus.taken(time);
			}
		}
		public void DeleteExpiredBonuses(long time)
		{
			Bonuses.RemoveAll(b => b.IsExpired(time));
		}

		public override void Think(List<AGameObject> players, long time)
		{
			base.Think(players, time);
			DeleteExpiredBonuses(time);
		}
	}
}
