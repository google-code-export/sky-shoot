using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Bonuses;
using SkyShoot.Service.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AGameObject, ISkyShootService
	{
		public static int GlobalID;
		public int LocalID;
		public string Name;
		public Queue<AGameEvent> NewEvents;
		public List<AGameBonus> Bonuses;

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private readonly SessionManager _sessionManager = SessionManager.Instance;

		private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

		public MainSkyShootService()
			: base(new Vector2(0, 0), Guid.NewGuid())
		{
			ObjectType = EnumObjectType.Player;
			NewEvents = new Queue<AGameEvent>();
			LocalID = GlobalID;
			GlobalID++;
			Bonuses = new List<AGameBonus>();

			InitWeapons();
		}

		private void InitWeapons()
		{
			Weapons.Add(Contracts.Weapon.AWeapon.AWeaponType.Pistol, new Weapon.Pistol(Guid.NewGuid(), this));
			Weapons.Add(Contracts.Weapon.AWeapon.AWeaponType.Shotgun, new Weapon.Shotgun(Guid.NewGuid(), this));

			ChangeWaponTo(Contracts.Weapon.AWeapon.AWeaponType.Pistol);
		}

		public AGameEvent[] AddBonus(AGameBonus bonus, long time)
		{
			AGameEvent t;
			if (bonus.Is(EnumObjectType.Remedy))
			{
				var health = HealthAmount;
				var potentialHealth = health + health * bonus.DamageFactor;
				HealthAmount = potentialHealth > MaxHealthAmount ? MaxHealthAmount : potentialHealth;
				t = new ObjectHealthChanged(HealthAmount, Id, time);
			}
			else
			{
				Bonuses.RemoveAll(b => b.ObjectType == bonus.ObjectType);
				Bonuses.Add(bonus);
				bonus.Taken(time);
				t = new BonusesChanged(Id, time, MergeBonuses());
			}
			return new[]
								{
									new ObjectDeleted(bonus.Id, time), 
									t
								};
		}

		public AGameBonus GetBonus(EnumObjectType bonusType)
		{
			return Bonuses.FirstOrDefault(bonus => bonus.Is(bonusType));
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
			catch (Exception e)
			{
				Trace.Fail(Name + " has not joined the game. " + e.Message);
				return false;
			}
		}

		public event SomebodyMovesHandler MeMoved;
		public event ClientShootsHandler MeShot;
		public event ClientChangeWeaponHandler MeChangeWeapon;

		public AGameEvent[] ChangeWeapon(SkyShoot.Contracts.Weapon.AWeapon.AWeaponType type)
		{
			if (MeChangeWeapon != null)
			{
				MeChangeWeapon(this, type);
			}
			return null;// GetEvents();
		}

		public AGameEvent[] Move(Vector2 direction) // приходит снаружи от клиента
		{
			if (MeMoved != null)
			{
				MeMoved(this, direction);
			}
			return null;// GetEvents();
		}

		public AGameEvent[] Shoot(Vector2 direction)
		{
			if (MeShot != null)
			{
				MeShot(this, direction);
			}
			return null;// GetEvents();
		}

		public AGameEvent[] GetEvents()
		{
			AGameEvent[] events;
			try
			{
			lock(NewEvents)
			{
				events = NewEvents.ToArray();//new Queue<AGameEvent>(NewEvents);
				NewEvents.Clear();
			}
			var sb = new StringBuilder();
			sb.Append("GetEvents:");
			sb.Append(events.Length);
			foreach (var e in events)
			{
				sb.Append(":");
				sb.Append(e);
			}
			Trace.WriteLine(sb);
			}
			catch (Exception exc)
			{
				Trace.WriteLine("GetEvents:" + exc);
				throw;
			}
			return events;
			return null;
		}

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
			try
			{
				GameSession session;
				_sessionManager.SessionTable.TryGetValue(Id, out session);
				if (session == null)
				{
					return null;
				}
				return GameObjectConverter.ArrayedObjects(session.GetSynchroFrame());

			}
			catch (Exception exc)
			{
				Trace.WriteLine("ERROR: Syncroframe broken", exc.ToString());				
			}
			return null;
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

		public override Vector2 ComputeMovement(long updateDelay, GameLevel gameLevel)
		{
			AGameBonus speedUpBonus = this.GetBonus(AGameObject.EnumObjectType.Speedup);
			float speedUp = speedUpBonus == null ? 1f : speedUpBonus.DamageFactor;
			float oldSpeed = this.Speed;
			this.Speed *= speedUp;
			var newCoord = base.ComputeMovement(updateDelay, gameLevel);
			this.Speed = oldSpeed;

			newCoord.X = MathHelper.Clamp(newCoord.X, 0, gameLevel.levelHeight);
			newCoord.Y = MathHelper.Clamp(newCoord.Y, 0, gameLevel.levelWidth);

			return newCoord;
		}

		private EnumObjectType MergeBonuses()
		{
			return Bonuses.Aggregate((EnumObjectType)0, (current, bonus) => current | bonus.ObjectType);
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, long time)
		{
			// empty array
			var res = base.Do(obj, time);
			if (obj.Is(EnumObjectType.Bonus))
			{
				obj.IsActive = false;
				var bonus = new AGameBonus();
				bonus.Copy(obj);

				return AddBonus(bonus, time);
			}
			return res;
		}
		public void DeleteExpiredBonuses(long time)
		{
			Bonuses.RemoveAll(b => b.IsExpired(time));
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, long time)
		{
			// empty list
			var res = base.Think(players, time);
			var l = Bonuses.Count;
			DeleteExpiredBonuses(time);
			return l != Bonuses.Count ? new AGameEvent[] { new BonusesChanged(Id, time, MergeBonuses()) } : res;
		}
	}
}
