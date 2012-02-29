using System;

using System.ServiceModel;

using System.Collections.Generic;

using System.Diagnostics;

using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Service.Session;

namespace SkyShoot.Service
{
	static class TypeConverter
	{
		public static AGameObject Mob(AGameObject m)
		{
			return new AGameObject(m);
		}

		public static AGameObject[] Mobs(AGameObject[] ms)
		{
			var rs = new AGameObject[ms.Length];
			for (int i = 0; i < ms.Length; i++)
			{
				rs[i] = new AGameObject(ms[i]);
			}
			return rs;
		}

		public static AProjectile Projectile(AProjectile p)
		{
			return new AProjectile(p);
		}

		public static AProjectile[] Projectiles(AProjectile[] ps)
		{
			var rs = new AProjectile[ps.Length];
			for (int i = 0; i < ps.Length; i++)
			{
				rs[i] = new AProjectile(ps[i]);
			}
			return rs;
		}
	}

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AGameObject, ISkyShootService//, ISkyShootCallback
	{
		public static int globalID = 0;
		public int localID;
		private ISkyShootCallback _callback;
		public string Name;
		public Queue<AGameEvent> NewEvents;

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private readonly Session.SessionManager _sessionManager = Session.SessionManager.Instance;

		private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

		public MainSkyShootService() : base(new Vector2(0, 0), Guid.NewGuid()) 
		{
			NewEvents = new Queue<AGameEvent>();
			localID = globalID; globalID++; 
		}

		public void Disconnect() { _sessionManager.LeaveGame(this); }

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
				_callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
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

		public event SomebodyMovesHandler MeMoved;
		public event ClientShootsHandler MeShot;
		//public event SomebodySpawnsHandler MobSpawned;
		//public event SomebodyDiesHandler MobDied;

		public Queue<AGameEvent> Move(Vector2 direction) // приходит снаружи от клиента
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
			return GetEvents();
		}

		public Queue<AGameEvent> GetEvents()
		{
			Queue<AGameEvent> events = new Queue<AGameEvent>(NewEvents);
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
			Trace.WriteLine("callback: GameStarted");
			return _sessionManager.GameStarted(gameId);
		}


		public AGameObject[] SynchroFrame()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id,out session);
			return session.GetSynchroFrame();
		}

		public String[] PlayerListUpdate()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id, out session);
			return session.LocalGameDescription.Players.ToArray();
		}

		/*
		public void SpawnMob(AGameObject mob)
		{
			var mobCopy = TypeConverter.Mob(mob);
			//Trace.WriteLine("callback.SpawnMob(mID: " + mob.Id + ")");

			try
			{
				_callback.SpawnMob(mobCopy);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void Hit(AGameObject mob, AProjectile projectile)
		{
			var mobCopy = TypeConverter.Mob(mob);
			var projCopy = projectile==null ? null : TypeConverter.Projectile(projectile);

			try
			{
				_callback.Hit(mobCopy, projCopy);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void MobDead(AGameObject mob)
		{
			var mobCopy = TypeConverter.Mob(mob);

			try
			{
				_callback.MobDead(mobCopy);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void MobMoved(AGameObject mob, Vector2 direction)
		{
			if (mob == this)
				return;

			var mobCopy = TypeConverter.Mob(mob);

			try
			{
				_callback.MobMoved(mobCopy, direction);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void MobShot(AGameObject mob, AProjectile[] projectiles)
		{
			var mobCopy = TypeConverter.Mob(mob);
			var projsCopy = TypeConverter.Projectiles(projectiles);

			try
			{
				_callback.MobShot(mobCopy, projsCopy);
			}
			catch (Exception)  { this.Disconnect(); }
		}

		public void BonusDropped(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			try
			{
				_callback.BonusDropped(bonus);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void BonusExpired(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			try
			{
				_callback.BonusExpired(bonus);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void BonusDisappeared(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			try
			{
				_callback.BonusDisappeared(bonus);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void GameOver()
		{
			try
			{
				_callback.GameOver();
				Trace.WriteLine(localID + ": GameOver");
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void PlayerLeft(AGameObject mob)
		{
			var mobCopy = TypeConverter.Mob(mob);

			try
			{
				_callback.PlayerLeft(mobCopy);
			}
			catch (Exception ) { this.Disconnect(); }
		}

		public void SynchroFrame(AGameObject[] mobs)
		{
			var mobsCopy = TypeConverter.Mobs(mobs);

			try
			{
				_callback.SynchroFrame(mobsCopy);
			}
			catch (Exception ) { this.Disconnect(); }
		}
		
		// передает массив игроков данной игры
		public void PlayerListChanged(String[] names)
		{
			try
			{
				_callback.PlayerListChanged(names);
			}
			catch(Exception){ this.Disconnect(); }

		}*/


	}
}
