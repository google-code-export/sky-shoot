using System;

using System.ServiceModel;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.ScreenManager;
using SkyShoot.Game.Screens;
using AMob = SkyShoot.Contracts.Mobs.AMob;

namespace SkyShoot.Game.Client.Game
{
    public sealed class GameController : ISkyShootCallback, ISkyShootService
    {
        private static readonly GameController LocalInstance = new GameController();

        public static Guid MyId { get; private set; }

        private ISkyShootService _service;

        public static GameController Instance
        {
            get { return LocalInstance; }
        }

        public GameModel GameModel { get; private set; }

        private GameController() {}

#region ServerInput

        public void GameStart(AMob[] mobs, Contracts.Session.GameLevel arena)
        {
            ScreenManager.ScreenManager.Instance.AddScreen(new GameplayScreen());

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
            //throw new System.NotImplementedException();
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
            //throw new System.NotImplementedException();
        }

#endregion

#region ClientInput

        public void HandleInput(InputState inputState)
        {
            GameModel.GetMob(MyId).RunVector = inputState.RunVector;
            var mouseState = inputState.CurrentMouseState;
            var mouseCoordinates = new Vector2(mouseState.X, mouseState.Y);

            var aMob = GameModel.GetMob(MyId);
            aMob.ShootVector = GameModel.Camera2D.ConvertToLocal(aMob.Coordinates) - mouseCoordinates;
            if(aMob.ShootVector.Length() > 0)
                aMob.ShootVector.Normalize();
        }

        public void MobShot(AMob mob, AProjectile[] projectiles)
        {
            throw new NotImplementedException();
        }

        public bool Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Guid? Login(string username, string password)
        {
            var channelFactory = new DuplexChannelFactory<ISkyShootService>(this, "SkyShootEndpoint");

            _service = channelFactory.CreateChannel();

            Guid? login = _service.Login(username, password);
            if (login.HasValue)
            {
                MyId = login.Value;
            }
            else
            {
                //todo popup
                Console.WriteLine("Connection failed");
            }

            Console.WriteLine(login);

            return login;
        }

        public GameDescription[] GetGameList()
        {
            return _service.GetGameList();
        }

        public GameDescription CreateGame(GameMode mode, int maxPlayers)
        {
            return _service.CreateGame(mode, maxPlayers);
        }

        public bool JoinGame(GameDescription game)
        {
            return _service.JoinGame(game);
        }

        public void Move(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public void Shoot(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public void TakeBonus(AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public void TakePerk(Perk perk)
        {
            throw new NotImplementedException();
        }

        public void LeaveGame()
        {
            throw new NotImplementedException();
        }
#endregion
    }
}
