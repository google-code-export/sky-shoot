﻿using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;

using SkyShoot.Contracts.Weapon.Projectiles;

using Microsoft.Xna.Framework;

namespace SkyShoot.Client.Game
{
    public class GameController:Contracts.Service.ISkyShootCallback
    {

        public GameModel GameModel { get; private set; }

        public GameController()
        {
            //todo initialize connection with server

            //todo temporary!
            GameStart(new AMob[0], new Contracts.Session.GameLevel(Contracts.Session.TileSet.Sand));
        }

        public void GameStart(AMob[] mobs, Contracts.Session.GameLevel arena)
        {
            GameModel = new GameModel(GameFactory.CreateClientGameLevel(arena));
            foreach (AMob mob in mobs)
            {
                var clientMob = GameFactory.CreateClientMob(mob);
                GameModel.AddMob(clientMob);
            }
        }

        public void Shoot(AProjectile projectile)
        {
            GameModel.AddProjectile(GameFactory.CreateClientProjectile(projectile));
        }

        public void SpawnMob(AMob mob)
        {
            throw new System.NotImplementedException();
        }

        public void Hit(AMob mob, AProjectile projectile)
        {
            //todo
            GameModel.GetMob(mob.Id).HealthAmount -= 10;
        }

        public void MobDead(AMob mob)
        {
            GameModel.RemoveMob(mob.Id);
        }

        public void MobMoved(AMob mob, Vector2 direction)
        {
            throw new System.NotImplementedException();
        }

        public void BonusDropped(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void BonusExpired(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void BonusDisappeared(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void GameOver()
        {
            throw new System.NotImplementedException();
        }

        public void PlayerLeft(AMob mob)
        {
            throw new System.NotImplementedException();
        }

        public void SynchroFrame(AMob[] mob)
        {
            throw new System.NotImplementedException();
        }
    }
}
