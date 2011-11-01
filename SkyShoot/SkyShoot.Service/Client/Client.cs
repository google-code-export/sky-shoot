using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyShoot.Service.Client
{
    public class Client : SkyShoot.Contracts.Mobs.AMob, SkyShoot.Contracts.Service.ISkyShootCallback 
    {
        public string Name;

        public Client(string username) : base(new Microsoft.Xna.Framework.Vector2(0, 0), new Guid())
        {
            this.Name = username;
        }

        public void GameStart(Contracts.Mobs.AMob mob, Contracts.Session.GameLevel arena)
        {
            throw new NotImplementedException();
        }

        public void Shoot(Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            throw new NotImplementedException();
        }

        public void SpawnMob(Contracts.Mobs.AMob mob)
        {
            throw new NotImplementedException();
        }

        public void Hit(Contracts.Mobs.AMob mob, Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            throw new NotImplementedException();
        }

        public void MobDead(Contracts.Mobs.AMob mob)
        {
            throw new NotImplementedException();
        }

        public void BonusDropped(Contracts.Bonuses.AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public void BonusExpired(Contracts.Bonuses.AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public void BonusDisappeared(Contracts.Bonuses.AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public void GameOver()
        {
            throw new NotImplementedException();
        }

        public void PlayerLeft(Contracts.Mobs.AMob mob)
        {
            throw new NotImplementedException();
        }

        public void SynchroFrame(Contracts.Mobs.AMob[] mob)
        {
            throw new NotImplementedException();
        }

        public override bool IsPlayer
        {
            get { throw new NotImplementedException(); }
        }
    }
}