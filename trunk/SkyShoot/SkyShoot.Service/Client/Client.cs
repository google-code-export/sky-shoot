using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Service;
using Microsoft.Xna.Framework;

namespace SkyShoot.Service.Client
{
    public class Client : SkyShoot.Contracts.Mobs.AMob, SkyShoot.Contracts.Service.ISkyShootCallback 
    {
        public string Name;
        private ISkyShootCallback _callback;

        public Client(string username, ISkyShootCallback callback, bool isPlayer = false) : base(new Microsoft.Xna.Framework.Vector2(0, 0), new Guid())
        {
            this.Name = username;
            this._callback = callback;
            this.IsPlayer = isPlayer;
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

        /*
         *        void GameSession_SomebodyMoves(object sender, MoveEventArgs e)
        {
            throw new NotImplementedException();
        }

         * */

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

        //public override bool IsPlayer
        //{
        //    get;
        //    set;
        //}
    }
}