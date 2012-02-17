using System;

using System.Diagnostics;
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

using SkyShoot.Game.Client.View;

using AMob = SkyShoot.Contracts.Mobs.AMob;

namespace SkyShoot.Game.Client.Game
{
	public sealed class GameController : ISkyShootGameCallback, ISkyShootGameService
	{
		public static Guid MyId { get; private set; }

		public bool IsGameStarted { get; private set; }

		private static readonly GameController LocalInstance = new GameController();

		private ISkyShootGameService _gameService;
		private ISkyShootAdministratorService _adminService;

		public static GameController Instance
		{
			get { return LocalInstance; }
		}

		public GameModel GameModel { get; private set; }

		private GameController()
		{
			InitConnection();
		}

		#region ServerInput

		public void GameStart(AMob[] mobs, Contracts.Session.GameLevel arena)
		{
			ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.GameplayScreen;

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
			if (projectile != null)
				GameModel.RemoveProjectile(projectile.Id);
			GameModel.GetMob(mob.Id).HealthAmount = mob.HealthAmount;
		}

		public void MobDead(AMob mob)
		{
			GameModel.RemoveMob(mob.Id);
			GameModel.GameLevel.AddTexture(mob.IsPlayer ? Textures.DeadPlayerTexture : Textures.DeadSpiderTexture,
			                               mob.Coordinates);
		}

		public void MobMoved(AMob mob, Vector2 direction)
		{
			GameModel.GetMob(mob.Id).RunVector = direction;
		}

		public void BonusDropped(AObtainableDamageModifier bonus)
		{
			throw new NotImplementedException();
		}

		public void BonusExpired(AObtainableDamageModifier bonus)
		{
			var player = GameModel.GetMob(MyId);
			player.State &= ~bonus.Type;
		}

		public void BonusDisappeared(AObtainableDamageModifier bonus)
		{
			throw new NotImplementedException();
		}

		public void GameOver()
		{
			ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MainMenuScreen;
		}

		public void PlayerLeft(AMob mob)
		{
			//TODO! issue 26? 

			/*for (int i = 0; i < ScreenManager.ScreenManager.Instance.GetScreens().Length; i++)
			{
				if (ScreenManager.ScreenManager.Instance.GetScreens()[i] is WaitScreen)
				{
					if (ScreenManager.ScreenManager.Instance.GetScreens()[i].IsActive)
					{
						var waitScreen = (WaitScreen)ScreenManager.ScreenManager.Instance.GetScreens()[i];
						waitScreen.RemovePlayer(mob.IsPlayer);
					}
				}	
			}*/

			if (IsGameStarted)
				GameModel.RemoveMob(mob.Id);
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
				AMob clientMob;
				try
				{
					clientMob = GameModel.GetMob(mob.Id);
				}
				catch
				{
					continue;
				}
				if (clientMob == null)
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
			//TODO! issue 26?
			for (int i = 0; i < ScreenManager.ScreenManager.Instance.GetScreens().Length; i++)
			{
				if (ScreenManager.ScreenManager.Instance.GetScreens()[i] is WaitScreen)
				{
					//todo!
					//if (ScreenManager.ScreenManager.Instance.GetScreens()[i].IsActive)
					//{
					var screen = (WaitScreen) ScreenManager.ScreenManager.Instance.GetScreens()[i];
					screen.ChangePlayerList(names);
					//}
				}
			}
		}
		#endregion

		#region ClientInput

		private DateTime _dateTime;
		private const int Rate = 1000 / 10;

		public void HandleInput(InputState inputState)
		{
			var player = GameModel.GetMob(MyId);

			if (player == null)
				return;

			// current RunVector
			Vector2 currentRunVector = inputState.RunVector(inputState.CurrentKeyboardState,
			                                                inputState.CurrentGamePadState);
			// previous RunVector
			Vector2 previousRunVector = inputState.RunVector(inputState.LastKeyboardState,
			                                                 inputState.LastGamePadState);

			if (!currentRunVector.Equals(previousRunVector))
			{
				Move(currentRunVector);
				player.RunVector = currentRunVector;
			}

			var mouseState = inputState.CurrentMouseState;
			var mouseCoordinates = new Vector2(mouseState.X, mouseState.Y);

			player.ShootVector = mouseCoordinates - GameModel.Camera2D.ConvertToLocal(player.Coordinates);
			if (player.ShootVector.Length() > 0)
				player.ShootVector.Normalize();

			if (inputState.CurrentMouseState.LeftButton == ButtonState.Pressed)
			{
				if ((DateTime.Now - _dateTime).Milliseconds > Rate)
				{
					_dateTime = DateTime.Now;
					Shoot(player.ShootVector);
				}
			}
		}

		private void InitConnection()
		{
			var channelFactory = new DuplexChannelFactory<ISkyShootGameService>(this, "SkyShootEndpoint");
			_gameService = channelFactory.CreateChannel();
		}

		private void FatalError(Exception e)
		{
			Trace.WriteLine(e);

			// close all screens
			// foreach (GameScreen screen in ScreenManager.ScreenManager.Instance.GetScreens()) screen.ExitScreen();
			// back to multiplayer screen
			ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.LoginScreen;

			MessageBox.Message = "Connection error!";
			ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
		}

		public bool Register(string username, string password)
		{
			try
			{
				return _adminService.Register(username, password);
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public Guid? Login(string username, string password)
		{
			Guid? login = null;
			try
			{
				login = _adminService.Login(username, password);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
			if (login.HasValue)
			{
				MyId = login.Value;
			}
			else
			{
				MessageBox.Message = "Connection error!";
				ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
			}

			return login;
		}

		public GameDescription[] GetGameList()
		{
			try
			{
				return _adminService.GetGameList();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tile)
		{
			try
			{
				return _adminService.CreateGame(mode, maxPlayers, tile);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public bool JoinGame(GameDescription game)
		{
			try
			{
				return _adminService.JoinGame(game);
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public void Move(Vector2 direction)
		{
			try
			{
				_gameService.Move(direction);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public void Shoot(Vector2 direction)
		{
			try
			{
				_gameService.Shoot(direction);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public void TakeBonus(AObtainableDamageModifier bonus)
		{
			try
			{
				_gameService.TakeBonus(bonus);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public void TakePerk(Perk perk)
		{
			try
			{
				_gameService.TakePerk(perk);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public void LeaveGame()
		{
			try
			{
				_adminService.LeaveGame();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}
		#endregion
	}
}