using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Contracts.SynchroFrames;
using SkyShoot.Contracts.Utils;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Bonuses;
using SkyShoot.Service.Session;
using SkyShoot.Service.Statistics;
using SkyShoot.ServProgram.Account;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
		InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AGameObject, ISkyShootService
	{
		#region private fields

		private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

		private static int _globalID;

		private int _localID;

		private float _speed;

		private string _username;

		private readonly InstanceContext channelContext;

		private readonly Queue<AGameEvent> _filteredEvents = new Queue<AGameEvent>();

		private readonly IAccountManager _accountManager = SimpleAccountManager.Instance;
		private readonly SessionManager _sessionManager = SessionManager.Instance;

		#endregion

		#region public fields

		public string Name;
		public Queue<AGameEvent> NewEvents;
		public List<AGameBonus> Bonuses;
		public ExpTracker Tracker;

		public delegate void SomebodyMovesHandler(AGameObject sender, Vector2 direction);
		public delegate void ClientShootsHandler(AGameObject sender, Vector2 direction);
		public delegate void ClientChangeWeaponHandler(AGameObject sender, WeaponType type);

		public event SomebodyMovesHandler MeMoved;
		public event ClientShootsHandler MeShot;
		public event ClientChangeWeaponHandler MeChangeWeapon;

		#endregion

		public override float Speed
		{
			get
			{
				AGameBonus speedUpBonus = GetBonus(EnumObjectType.Speedup);
				float speedUp = speedUpBonus == null ? 1f : speedUpBonus.DamageFactor;
				var speed = _speed * speedUp;
				return speed;
			}
			set { _speed = value; }
		}

		public MainSkyShootService()
			: base(new Vector2(0, 0), Guid.NewGuid())
		{
			channelContext = OperationContext.Current.InstanceContext;
			channelContext.Faulted += OnChannelStopped;
			channelContext.Closed += OnChannelStopped;
			ObjectType = EnumObjectType.Player;
			NewEvents = new Queue<AGameEvent>();
			_localID = _globalID;
			_globalID++;
			Bonuses = new List<AGameBonus>();

			InitWeapons();
		}

		void OnChannelStopped(object sender, EventArgs e)
		{
			channelContext.Faulted -= OnChannelStopped;
			channelContext.Closed -= OnChannelStopped;
			LeaveGame();
			Logout();
			// all the trolology of closing the session on a high level
		}

		#region private methods

		// Начальная статистика
		private void InitStatistics()
		{
			Tracker = new LinearExpTracker();
		}

		private void InitWeapons()
		{
			Weapons.Add(WeaponType.Pistol, new Weapon.Pistol(Guid.NewGuid(), this));
			Weapons.Add(WeaponType.Shotgun, new Weapon.Shotgun(Guid.NewGuid(), this));
			Weapons.Add(WeaponType.RocketPistol, new Weapon.RocketPistol(Guid.NewGuid(), this));
			Weapons.Add(WeaponType.Heater, new Weapon.Heater(Guid.NewGuid(), this));
			Weapons.Add(WeaponType.FlamePistol, new Weapon.FlamePistol(Guid.NewGuid(), this));
			Weapons.Add(WeaponType.TurretMaker, new Weapon.TurretMaker(Guid.NewGuid(), this));
			Weapons.Add(WeaponType.MobGenerator, new Weapon.MobGenerator(Guid.NewGuid(), this));

			ChangeWaponTo(WeaponType.Pistol);
		}

		private IEnumerable<AGameEvent> AddBonus(AGameBonus bonus, long time)
		{
			if (bonus.Is(EnumObjectType.Remedy))
			{
				var health = HealthAmount;
				var potentialHealth = health + health * bonus.DamageFactor;
				HealthAmount = potentialHealth > MaxHealthAmount ? MaxHealthAmount : potentialHealth;
				AGameEvent gameEvent = new ObjectHealthChanged(HealthAmount, Id, time);
				return new[]
				       {
				       	new ObjectDeleted(bonus.Id, time),
				       	gameEvent
				       };
			}
			Bonuses.RemoveAll(b => b.ObjectType == bonus.ObjectType);
			Bonuses.Add(bonus);
			bonus.Taken(time);
			//t = new BonusesChanged(Id, time, MergeBonuses());
			return new AGameEvent[]
			       {
			       	new ObjectDeleted(bonus.Id, time)
			       	//t
			       };
		}

		private void DeleteExpiredBonuses(long time)
		{
			Bonuses.RemoveAll(b => b.IsExpired(time));
		}

		private EnumObjectType MergeBonuses()
		{
			return Bonuses.Aggregate((EnumObjectType)0, (current, bonus) => current | bonus.ObjectType);
		}

		#endregion

		#region public methods

		public AGameBonus GetBonus(EnumObjectType bonusType)
		{
			return Bonuses.FirstOrDefault(bonus => bonus.Is(bonusType));
		}

		public void Disconnect()
		{
			LeaveGame();
		}

		#region service implementation

		#region регистрация, логин и создание игры

		public AccountManagerErrorCode Register(string username, string password)
		{
			return _accountManager.Register(username, password);
		}

		public Guid? Login(string username, string password, out AccountManagerErrorCode errorCode)
		{
			errorCode = _accountManager.Login(username, password);
			if (errorCode == AccountManagerErrorCode.Ok)
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

			_username = username;

			return Id;
		}

		public AccountManagerErrorCode Logout()
		{
			return _accountManager.Logout(_username);
		}

		public GameDescription[] GetGameList()
		{
			return _sessionManager.GetGameList();
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tileSet, int teams)
		{
			try
			{
				var gameDescription = _sessionManager.CreateGame(mode, maxPlayers, this, tileSet, teams);
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
			catch (Exception e)
			{
				Trace.Fail(Name + " has not joined the game. " + e.Message);
				return false;
			}
		}


		public void LeaveGame()
		{
			bool result = _sessionManager.LeaveGame(this);
			if (!result)
			{
				Trace.WriteLine(Name + " has left the game.");
				return;
			}

			System.Console.WriteLine(Name + " has left the game.");
			Trace.WriteLine(Name + " has left the game.");

			ClientsList.Remove(this);
		}

		public GameLevel GameStart(int gameId)
		{
			// Обнуление
			InitStatistics();
			return _sessionManager.GameStarted(gameId);
		}

		public String[] PlayerListUpdate()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id, out session);
			if (session == null)
			{
				return new string[] { };
			}
			return session.LocalGameDescription.Players.ToArray();
		}

		#endregion

		#region процесс игры

		public void Move(Vector2 direction)
		{
			if (MeMoved != null)
			{
				MeMoved(this, direction);
			}
		}

		public void Shoot(Vector2 direction)
		{
			if (MeShot != null)
			{
				MeShot(this, direction);
			}
		}

		public void ChangeWeapon(WeaponType type)
		{
			if (MeChangeWeapon != null)
			{
				MeChangeWeapon(this, type);
			}
		}

		public AGameEvent[] GetEvents()
		{
			// TODO uncomment just for test
			// Thread.Sleep(5000);

			AGameEvent[] events;
			try
			{
				lock (NewEvents)
				{
					events = NewEvents.ToArray();
					//System.Console.WriteLine(NewEvents.Count);
					NewEvents.Clear();
				}

				#region Фильтрация эвентов

				_filteredEvents.Clear();
				for (int i = 0; i < events.Count(); i++)
				{
					bool mismatch = true;
					for (int j = i + 1; j < events.Count(); j++)
					{
						if ((events[i].GameObjectId == events[j].GameObjectId) & (events[i].Type == events[j].Type))
						{
							mismatch = false;
							break;
						}
					}
					if (mismatch) _filteredEvents.Enqueue(events[i]);
				}
				events = _filteredEvents.ToArray();
				//System.Console.WriteLine(_filteredEvents.Count);

				#endregion

				Logger.PrintEvents(events);
			}
			catch (Exception exc)
			{
				Trace.WriteLine("GetEvents:" + exc);
				throw;
			}
			return events;
		}

		public SynchroFrame SynchroFrame()
		{
			try
			{
				GameSession session;
				_sessionManager.SessionTable.TryGetValue(Id, out session);
				if (session == null)
				{
					return null;
				}

				AGameObject[] gameObjects = GameObjectConverter.ArrayedObjects(session.GetSynchroFrame());

				var synchroFrame = new SynchroFrame(gameObjects, session.GetTime());

				Trace.WriteLine(synchroFrame);

				return synchroFrame;
			}
			catch (Exception exc)
			{
				Trace.WriteLine("ERROR: Syncroframe broken", exc.ToString());
			}
			return null;
		}

		public Stats? GetStats()
		{
			return Tracker.GetStats();
		}

		#endregion

		#endregion

		#region GameObject behavior

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			// empty array
			var res = base.Do(obj, newObjects, time);
			if (obj.Is(EnumObjectType.Bonus))
			{
				obj.IsActive = false;
				var bonus = new AGameBonus();
				bonus.Copy(obj);

				return AddBonus(bonus, time);
			}
			return res;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, List<AGameObject> newGameObjects,
														long time)
		{
			// empty list
			var res = base.Think(players, newGameObjects, time);
			DeleteExpiredBonuses(time);
			// var l = Bonuses.Count;
			return res; // l != Bonuses.Count ? new AGameEvent[] { new BonusesChanged(Id, time, MergeBonuses()) } : res;
		}
		#endregion

		#endregion
	}
}