using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using SkyShoot.Contracts.Service;
using Microsoft.Xna.Framework;
using SkyShoot.Service.Session;
using SkyShoot.Contracts.Mobs;


namespace SkyShoot.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.PerSession)]
    public class MainSkyShootService : AMob, ISkyShootService, ISkyShootCallback
    {
        private ISkyShootCallback _callback;
        public string Name;

        private Account.AccountManager _accountManager = new Account.AccountManager();
        private Session.SessionManager _sessionManager = Session.SessionManager.Instance;

        private static List<MainSkyShootService> _clientsList = new List<MainSkyShootService>();

        public MainSkyShootService() : base(new Microsoft.Xna.Framework.Vector2(0, 0), new Guid()) {}

        public bool Register(string username, string password)
        {
            bool result = _accountManager.Register(username, password);
            return result;
        }

        public bool Login(string username, string password)
        {
            bool result = _accountManager.Login(username, password);

            if (result)
            {
                this.Name = username;
                this._callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
                this.IsPlayer = true;

                _clientsList.Add(this);
            }

            return result;
        }

        public Contracts.Session.GameDescription[] GetGameList()
        {
            return _sessionManager.GetGameList();
        }

        public bool CreateGame(Contracts.Session.GameMode mode, int maxPlayers)
        {
            
            return ( _sessionManager.CreateGame(mode, maxPlayers, this, Contracts.Session.TileSet.Grass) != null );
        }

        public bool JoinGame(Contracts.Session.GameDescription game)
        {
            return _sessionManager.JoinGame(game, this);
        }

        public event SomebodyMovesHadler MeMoved;

        public void Move(Vector2 direction) // приходит снаружи от клиента
        {
            if (MeMoved != null)
            {
                MeMoved(this, direction);
            }
        }

        public void Shoot(Vector2 direction)
        {
            throw new NotImplementedException();
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
            _callback.GameStart(mobs, arena);
        }

        public void Shoot(Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            _callback.Shoot(projectile);
        }

        public void SpawnMob(Contracts.Mobs.AMob mob)
        {
            _callback.SpawnMob(mob);
        }

        public void Hit(Contracts.Mobs.AMob mob, Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            _callback.Hit(mob, projectile);
        }

        public void MobDead(Contracts.Mobs.AMob mob)
        {
            _callback.MobDead(mob);
        }

        public void MobMoved(Contracts.Mobs.AMob mob, Vector2 direction)
        {
            if (mob == this)
                return;

            _callback.MobMoved(mob, direction);
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
            _callback.PlayerLeft(mob);
        }

        public void SynchroFrame(Contracts.Mobs.AMob[] mobs)
        {
            _callback.SynchroFrame(mobs);
        }
    }
}
