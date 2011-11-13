using System;

using System.Collections.Generic;

using System.ServiceModel;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;

using SkyShoot.Service.Weapon;

using Microsoft.Xna.Framework;

namespace SkyShoot.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.PerSession)]
    public class MainSkyShootService : AMob, ISkyShootService, ISkyShootCallback
    {
        private ISkyShootCallback _callback;
        public string Name;

        public AWeapon Weapon { get; set; }

        //private readonly Account.AccountManager _accountManager = new Account.AccountManager();
        private readonly Session.SessionManager _sessionManager = Session.SessionManager.Instance;

        private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

        public MainSkyShootService() : base(new Vector2(0, 0), new Guid()) { }

        public bool Register(string username, string password)
        {
            //bool result = _accountManager.Register(username, password);
            //return result;
            return true;
        }

        public bool Login(string username, string password)
        {
            bool result = true; //_accountManager.Login(username, password);

            if (result)
            {
                this.Name = username;
                this._callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
                this.IsPlayer = true;

                ClientsList.Add(this);
            }

            return result;
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

        public event SomebodyMovesHadler MeMoved;
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
            bool result = _sessionManager.LeaveGame(Name);
            if (!result)
            { /* что-то сделать, например, добавить сообщение в лог */
                return;
            }

            ClientsList.Remove(this);
        }

        public void GameStart(AMob[] mobs, Contracts.Session.GameLevel arena)
        {
            _callback.GameStart(mobs, arena);
        }

        public void SpawnMob(AMob mob)
        {
            _callback.SpawnMob(mob);
        }

        public void Hit(AMob mob, Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            _callback.Hit(mob, projectile);
        }

        public void MobDead(AMob mob)
        {
            _callback.MobDead(mob);
        }

        public void MobMoved(AMob mob, Vector2 direction)
        {
            if (mob == this)
                return;

            _callback.MobMoved(mob, direction);
        }

        public void MobShot(AMob mob, Contracts.Weapon.Projectiles.AProjectile[] projectiles)
        {
            if (mob == this)
                return;

            _callback.MobShot(mob, projectiles);
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
            _callback.PlayerLeft(mob);
        }

        public void SynchroFrame(AMob[] mobs)
        {
            _callback.SynchroFrame(mobs);
        }
    }
}
