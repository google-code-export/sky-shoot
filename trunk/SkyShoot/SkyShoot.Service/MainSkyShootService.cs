using System;

using System.ServiceModel;

using System.Collections.Generic;

using System.Diagnostics;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Service.Weapon;

namespace SkyShoot.Service
{
	static class TypeConverter
	{
		public static AMob Mob(AMob m)
		{
			return new AMob(m);
		}

		public static AMob[] Mobs(AMob[] ms)
		{
			var rs = new AMob[ms.Length];
			for (int i = 0; i < ms.Length; i++)
			{
				rs[i] = new AMob(ms[i]);
			}
			return rs;
		}
	}

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AMob, ISkyShootService, ISkyShootCallback
	{
		private ISkyShootCallback _callback;
		public string Name;

		public AWeapon Weapon { get; set; }

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private readonly Session.SessionManager _sessionManager = Session.SessionManager.Instance;

		private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

	    public MainSkyShootService() : base(new Vector2(0, 0), Guid.NewGuid()) {}

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

		public GameDescription CreateGame(GameMode mode, int maxPlayers)
		{
            try
            {
                var gameDescription = _sessionManager.CreateGame(mode, maxPlayers, this, TileSet.Grass);
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
        public event SomebodySpawnsHandler MobSpawned;
        public event SomebodyDiesHandler MobDied;

		public void Move(Vector2 direction) // приходит снаружи от клиента
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

		public void TakeBonus(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			bonus.Owner.State |= bonus.Type;
		}

		public void TakePerk(Contracts.Perks.Perk perk)
		{
			throw new NotImplementedException();
		}

		public void LeaveGame()
		{
			bool result = _sessionManager.LeaveGame(Name);
			if (!result)
			{
                Trace.WriteLine(Name + "left the game");
				return;
			}

			ClientsList.Remove(this);
		}

		public void GameStart(AMob[] mobs, GameLevel arena)
		{
			var mobsCopy = new AMob[mobs.Length];
			for (int i = 0; i < mobs.Length; i++)
				mobsCopy[i] = new AMob(mobs[i]);

			_callback.GameStart(mobsCopy, arena);
		}

		public void SpawnMob(AMob mob)
		{
			var mobCopy = new AMob(mob);
			_callback.SpawnMob(mobCopy);
		}

		public void Hit(AMob mob, AProjectile projectile)
		{
			var mobCopy = new AMob(mob);
			_callback.Hit(mobCopy, projectile);
		}

		public void MobDead(AMob mob)
		{
			var mobCopy = new AMob(mob);
			_callback.MobDead(mobCopy);
		}

		public void MobMoved(AMob mob, Vector2 direction)
		{
			if (mob == this)
				return;

			var mobCopy = new AMob(mob);
			_callback.MobMoved(mobCopy, direction);
		}

		public void MobShot(AMob mob, AProjectile[] projectiles)
		{
			if (mob == this)
				return;

			var mobCopy = new AMob(mob);
			_callback.MobShot(mobCopy, projectiles);
		}

		public void BonusDropped(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			_callback.BonusDropped(bonus);
		}

		public void BonusExpired(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			_callback.BonusExpired(bonus);
		}

		public void BonusDisappeared(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			_callback.BonusDisappeared(bonus);
		}

		public void GameOver()
		{
			_callback.GameOver();
		}

		public void PlayerLeft(AMob mob)
		{
			var mobCopy = new AMob(mob);
			_callback.PlayerLeft(mobCopy);
		}

		public void SynchroFrame(AMob[] mobs)
		{
			var mobsCopy = new AMob[mobs.Length];
			for (int i = 0; i < mobs.Length; i++)
				mobsCopy[i] = new AMob(mobs[i]);

			_callback.SynchroFrame(mobsCopy);
		}
	}
}
