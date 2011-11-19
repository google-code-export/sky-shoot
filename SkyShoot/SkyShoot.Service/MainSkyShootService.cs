using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
//using System.ServiceModel.Web;
using System.Text;

using SkyShoot.Contracts.Service;
using Microsoft.Xna.Framework;
using SkyShoot.Service.Session;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Service.Weapon;
using SkyShoot.Contracts.Weapon.Projectiles;


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
			AMob[] rs = new AMob[ms.Length];
			for (int i = 0; i < ms.Length; i++)
			{
				rs[i] = new AMob(ms[i]);
			}
			return rs;
		}
	}

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
			InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AMob, ISkyShootService, ISkyShootCallback
	{
		private ISkyShootCallback _callback;
		public string Name;

		public AWeapon Weapon { get; set; }

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private Session.SessionManager _sessionManager = Session.SessionManager.Instance;

		private static List<MainSkyShootService> _clientsList = new List<MainSkyShootService>();

		public MainSkyShootService() : base(new Microsoft.Xna.Framework.Vector2(0, 0), new Guid()) { }

		public bool Register(string username, string password)
		{
			//bool result = _accountManager.Register(username, password);
			//return result;
		    return true;
		}

		public Guid? Login(string username, string password)
		{
            bool result = true;//_accountManager.Login(username, password);

			if (result)
			{
				this.Name = username;
				this._callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
				this.IsPlayer = true;

				_clientsList.Add(this);
			}
			else
			{
				return null;
			}

			return this.Id;
		}

		public Contracts.Session.GameDescription[] GetGameList()
		{
			return _sessionManager.GetGameList();
		}

		public bool CreateGame(Contracts.Session.GameMode mode, int maxPlayers)
		{

			return (_sessionManager.CreateGame(mode, maxPlayers, this, Contracts.Session.TileSet.Grass) != null);
		}

		public bool JoinGame(Contracts.Session.GameDescription game)
		{
			return _sessionManager.JoinGame(game, this);
		}

		public event SomebodyMovesHandler MeMoved;
		public event ClientShootsHandler MeShot;

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
			bool result = _sessionManager.LeaveGame(this.Name);
			if (!result)
			{ /* что-то сделать, например, добавить сообщение в лог */
				return;
			}

			_clientsList.Remove(this);
		}

		public void GameStart(Contracts.Mobs.AMob[] mobs, Contracts.Session.GameLevel arena)
		{
			Contracts.Mobs.AMob[] Mobs = new Contracts.Mobs.AMob[mobs.Length];
			for (int i = 0; i < mobs.Length; i++)
				Mobs[i] = new AMob(mobs[i]);

			_callback.GameStart(Mobs, arena);
		}

		public void SpawnMob(Contracts.Mobs.AMob mob)
		{
			AMob Mob = new AMob(mob);
			_callback.SpawnMob(Mob);
		}

		public void Hit(Contracts.Mobs.AMob mob, Contracts.Weapon.Projectiles.AProjectile projectile)
		{
			AMob Mob = new AMob(mob);
			_callback.Hit(Mob, projectile);
		}

		public void MobDead(Contracts.Mobs.AMob mob)
		{
			AMob Mob = new AMob(mob);
			_callback.MobDead(Mob);
		}

		public void MobMoved(Contracts.Mobs.AMob mob, Vector2 direction)
		{
			if (mob == this)
				return;

			AMob Mob = new AMob(mob);
			_callback.MobMoved(Mob, direction);
		}

		public void MobShot(Contracts.Mobs.AMob mob, SkyShoot.Contracts.Weapon.Projectiles.AProjectile[] projectiles)
		{
			if (mob == this)
				return;

			AMob Mob = new AMob(mob);
			_callback.MobShot(Mob, projectiles);
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

		public void PlayerLeft(Contracts.Mobs.AMob mob)
		{
			AMob Mob = new AMob(mob);
			_callback.PlayerLeft(Mob);
		}

		public void SynchroFrame(Contracts.Mobs.AMob[] mobs)
		{
			Contracts.Mobs.AMob[] Mobs = new Contracts.Mobs.AMob[mobs.Length];
			for (int i = 0; i < mobs.Length; i++)
				Mobs[i] = new AMob(mobs[i]);

			_callback.SynchroFrame(Mobs);
		}
	}
}
