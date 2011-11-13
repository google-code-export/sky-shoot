using System;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.ScreenManager;

using SkyShoot.Game.Client.View;
using SkyShoot.Game.Client.Players;

using AMob = SkyShoot.Contracts.Mobs.AMob;

namespace SkyShoot.Game.Client.Game
{
    public class GameController:Contracts.Service.ISkyShootCallback
    {

        public GameModel GameModel { get; private set; }

        //todo temporary!
        private static readonly Guid Id= new Guid();
        private readonly AMob _testMob = new Player(Vector2.Zero, Id, Textures.PlayerTexture);
        private readonly AMob[] _mobs = new AMob[1]; 

        public GameController()
        {
            //todo initialize connection with server
            var service = new MainService.SkyShootServiceClient();
            Console.WriteLine(service.Login("test", "test"));

            //todo temporary!
            _mobs[0] = _testMob;
            GameStart(_mobs, new Contracts.Session.GameLevel(Contracts.Session.TileSet.Sand));
        }

        //
        //<-- server input -->
        //
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

        //
        //<-- client input -->
        //
        public void HandleInput(InputState inputState)
        {
            GameModel.GetMob(Id).RunVector = inputState.RunVector;
        }



        public void MobShot(AMob mob, AProjectile[] projectiles)
        {
            throw new NotImplementedException();
        }
    }
}
