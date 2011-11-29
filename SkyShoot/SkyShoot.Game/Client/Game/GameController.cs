using System;

using System.ServiceModel;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;

using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.Screens;
using SkyShoot.Game.ScreenManager;

using AMob = SkyShoot.Contracts.Mobs.AMob;

namespace SkyShoot.Game.Client.Game
{
    public sealed class GameController : ISkyShootCallback, ISkyShootService
    {
        public static Guid MyId { get; private set; }

        public bool IsGameStarted { get; private set; }

        private static readonly GameController LocalInstance = new GameController();

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
            // todo setActive
            foreach (GameScreen screen in ScreenManager.ScreenManager.Instance.GetScreens()) screen.ExitScreen();
            ScreenManager.ScreenManager.Instance.AddScreen(new GameplayScreen());

            GameModel = new GameModel(GameFactory.CreateClientGameLevel(arena));
            
            foreach (AMob mob in mobs)
            {
                var clientMob = GameFactory.CreateClientMob(mob);
                GameModel.AddMob(clientMob);
            }

            // GameModel initialized, set boolean flag  
            IsGameStarted = true;
        }

        public void SpawnMob(AMob mob)
        {
            var clientMob = GameFactory.CreateClientMob(mob);
            GameModel.AddMob(clientMob);
        }

        public void Hit(AMob mob, AProjectile projectile)
        {
            GameModel.GetMob(mob.Id).HealthAmount -= projectile.Damage;
            GameModel.RemoveProjectile(projectile.Id);
        }

        public void MobDead(AMob mob)
        {
            GameModel.RemoveMob(mob.Id);
        }

        public void MobMoved(AMob mob, Vector2 direction)
        {
            GameModel.GetMob(mob.Id).RunVector = direction;
        }

        public void BonusDropped(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void BonusExpired(AObtainableDamageModifier bonus)
        {
            var player = GameModel.GetMob(MyId);
            player.State &= ~bonus.Type;
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
        	for (int i = 0; i < ScreenManager.ScreenManager.Instance.GetScreens().Length; i++)
        	{
        		if (ScreenManager.ScreenManager.Instance.GetScreens()[i] is WaitScreen)
        		{
        			if (ScreenManager.ScreenManager.Instance.GetScreens()[i].IsActive)
        			{
        				WaitScreen screen;
        				screen = (WaitScreen) ScreenManager.ScreenManager.Instance.GetScreens()[i];
						//screen.RemovePlayer(mob.IsPlayer);
        			}
        		}	
        	}
            //todo popup window
            var clientMob = GameFactory.CreateClientMob(mob);
            GameModel.RemoveMob(clientMob.Id);
        }

        public void MobShot(AMob mob, AProjectile[] projectiles)
        {
            // update ShootVector
            var clientMob = GameModel.GetMob(mob.Id);
            clientMob.ShootVector = projectiles[projectiles.Length - 1].Direction;

            // add projectiles
            foreach (var aProjectile in projectiles)
            {
                GameModel.AddProjectile(GameFactory.CreateClientProjectile(aProjectile));
            }
        }

        public void SynchroFrame(AMob[] mobs)
        {
            if (!IsGameStarted)
                return;

            foreach (var mob in mobs)
            {
                var clientMob = GameModel.GetMob(mob.Id);
                if(clientMob == null)
                    continue;

                clientMob.Coordinates = mob.Coordinates;
                clientMob.HealthAmount = mob.HealthAmount;
                clientMob.RunVector = mob.RunVector;
                clientMob.ShootVector = mob.ShootVector;
                clientMob.Speed = mob.Speed;
                clientMob.State = mob.State;
            }
        }

		public void PlayerListChanged(String[] names)
        {
			for (int i = 0; i < ScreenManager.ScreenManager.Instance.GetScreens().Length; i++)
			{
				if (ScreenManager.ScreenManager.Instance.GetScreens()[i] is WaitScreen)
				{
					if (ScreenManager.ScreenManager.Instance.GetScreens()[i].IsActive)
					{
						WaitScreen screen;
						screen = (WaitScreen) ScreenManager.ScreenManager.Instance.GetScreens()[i];
						screen.ChangePlayerList(names);
					}
				}
			}			
        }

#endregion

#region ClientInput

        private DateTime _dateTime;
        private const int Rate = 1000/10;

        public void HandleInput(InputState inputState)
        {
            // current RunVector
            Vector2 currentRunVector = inputState.RunVector(inputState.CurrentKeyboardState);
            // previous RunVector
            Vector2 previousRunVector = inputState.RunVector(inputState.LastKeyboardState);

            if (!currentRunVector.Equals(previousRunVector))
            {
                Move(currentRunVector);
                GameModel.GetMob(MyId).RunVector = currentRunVector;
            }

            var mouseState = inputState.CurrentMouseState;
            var mouseCoordinates = new Vector2(mouseState.X, mouseState.Y);

            var aMob = GameModel.GetMob(MyId);
            aMob.ShootVector = mouseCoordinates - GameModel.Camera2D.ConvertToLocal(aMob.Coordinates);
            if(aMob.ShootVector.Length() > 0)
                aMob.ShootVector.Normalize();

            if (inputState.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                if ((DateTime.Now - _dateTime).Milliseconds > Rate)
                {
                    _dateTime = DateTime.Now;
                    Shoot(aMob.ShootVector);
                }
            }
        }

        public bool Register(string username, string password)
        {
            var channelFactory = new DuplexChannelFactory<ISkyShootService>(this, "SkyShootEndpoint");

            _service = channelFactory.CreateChannel();

            try
            {
                return _service.Register(username, password);
            }
            catch (EndpointNotFoundException)
            {
                return false;
            }
        }

        public Guid? Login(string username, string password)
        {
            var channelFactory = new DuplexChannelFactory<ISkyShootService>(this, "SkyShootEndpoint");

            _service = channelFactory.CreateChannel();

            Guid? login = null;
            try
            {
                login = _service.Login(username, password);
            }
            catch (EndpointNotFoundException)
            {
                
            }
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

        public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tile)
        {
            return _service.CreateGame(mode, maxPlayers, tile);
        }

        public bool JoinGame(GameDescription game)
        {
            return _service.JoinGame(game);
        }

        public void Move(Vector2 direction)
        {
            _service.Move(direction);
        }

        public void Shoot(Vector2 direction)
        {
            _service.Shoot(direction);
        }

        public void TakeBonus(AObtainableDamageModifier bonus)
        {
            _service.TakeBonus(bonus);
        }

        public void TakePerk(Perk perk)
        {
            _service.TakePerk(perk);
        }

        public void LeaveGame()
        {
            _service.LeaveGame();
        }

#endregion
    }
}
