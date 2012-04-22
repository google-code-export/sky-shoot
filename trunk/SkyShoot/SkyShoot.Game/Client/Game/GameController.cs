using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.ServiceModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Controls;
using SkyShoot.Game.Screens;

using SkyShoot.Game.Client.View;

using SkyShoot.Contracts.Mobs;

using System.Security.Cryptography;

namespace SkyShoot.Game.Client.Game
{
	public class HashHelper
	{
		public static string GetMd5Hash(string input)
			// выдаёт последовательность из 32 шестнадцатеричных цифр (md5 хеш от аргумента)
		{
			MD5 md5Hasher = MD5.Create();

			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

			var sBuilder = new StringBuilder();

			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}

	public sealed class GameController : ISkyShootService
	{
		SoundManager _soundManager;

		public static Guid MyId { get; private set; }

		public bool IsGameStarted { get; private set; }

		private static GameController _localInstance;

		private ISkyShootService _service;

		public static GameController Instance
		{
			get { return _localInstance ?? (_localInstance = new GameController()); }
		}

		public GameModel GameModel { get; private set; }

		private GameController()
		{
			InitConnection();

			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;
		}

		public void GameStart(Contracts.Session.GameLevel arena)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);

			GameModel = new GameModel(GameFactory.CreateClientGameLevel(arena));

			var gameObjects = SynchroFrame();

			foreach (AGameObject mob in gameObjects)
			{
				var clientMob = GameFactory.CreateClientMob(mob);
				GameModel.AddMob(clientMob);
			}

			// GameModel initialized, set boolean flag  
			IsGameStarted = true;
		}

		#region бывший callbacks

		//public void Hi777t(AGameObject mob, AProjectile projectile)
		//{
		//  if (projectile != null)
		//    GameModel.RemoveProjectile(projectile.Id);
		//  GameModel.GetMob(mob.Id).HealthAmount = mob.HealthAmount;
		//}

		public void MobDead(AGameObject mob)
		{			
			_soundManager.SoundPlay(SoundManager.SoundEnum.Spider);
			GameModel.RemoveMob(mob.Id);
			// todo //!!
			//GameModel.GameLevel.AddTexture(mob.Is(AGameObject.EnumObjectType.Player)
			//                                ? Textures.DeadPlayerTexture
			//                                : Textures.DeadSpiderTexture, TypeConverter.XnaLite2Xna(mob.Coordinates));
		}

		public void MobMoved(AGameObject mob, XNA.Framework.Vector2 direction)
		{
			// Trace.WriteLine("MobMoved!");
			GameModel.GetMob(mob.Id).RunVector = direction; // TypeConverter.Vector2_m2s(direction);
		}

		//public void BonusDropped(AObtainableDamageModifier bonus)
		//{
		//  throw new NotImplementedException();
		//}

		//public void BonusExpired(AObtainableDamageModifier bonus)
		//{
		//  var player = GameModel.GetMob(MyId);
		//  player.State &= ~bonus.Type;
		//}

		//public void BonusDisappeared(AObtainableDamageModifier bonus)
		//{
		//  throw new NotImplementedException();
		//}

		public void GameOver()
		{
			GameModel = null;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			IsGameStarted = false;
		}

		public void PlayerLeft(AGameObject mob)
		{
			if (IsGameStarted)
				GameModel.RemoveMob(mob.Id);
		}

		//public void MobShot(AGameObject mob, AProjectile[] projectiles)
		//{
		//  // update ShootVector
		//  var clientMob = GameModel.GetMob(mob.Id);
		//  clientMob.ShootVector = projectiles[projectiles.Length - 1].RunVector;

		//  // add projectiles
		//  foreach (var aProjectile in projectiles)
		//  {
		//    GameModel.AddProjectile(GameFactory.CreateClientProjectile(aProjectile));
		//  }
		//}

		#endregion

		#region ClientInput

		private DateTime _dateTime;
		private const int Rate = 1000 / 10;

		public void HandleInput(Controller controller)
		{
			var player = GameModel.GetMob(MyId);

			if (player == null)
				return;

			// current RunVector
			Vector2? currentRunVector = controller.RunVector;

			if (currentRunVector.HasValue)
			{
				Move(currentRunVector.Value);
				player.RunVectorM = currentRunVector.Value;
			}

			Vector2 mouseCoordinates = controller.SightPosition;

			player.ShootVectorM = mouseCoordinates - GameModel.Camera2D.ConvertToLocal(player.CoordinatesM);
			if (player.ShootVector.Length() > 0)
				player.ShootVector.Normalize();

			if (controller.ShootButton == ButtonState.Pressed)
			{
				if ((DateTime.Now - _dateTime).Milliseconds > Rate)
				{
					_dateTime = DateTime.Now;
					Shoot(player.ShootVectorM);
				}
			}
		}

		private void InitConnection()
		{
			try
			{
				var channelFactory = new ChannelFactory<ISkyShootService>("SkyShootEndpoint");
				_service = channelFactory.CreateChannel();
			}
			catch (Exception e)
			{
				FatalError(e);
				// Trace.WriteLine("Can't connect to SkyShootService");
				// Trace.WriteLine(e);
				// !! @todo catch this!
				MessageBox.Message = "Connection error!";
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
				// throw;
			}
		}

		private void FatalError(Exception e)
		{
			Trace.WriteLine(e);

			MessageBox.Message = "Connection error!";
			MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
		}

		public bool Register(string username, string password)
		{
			try
			{
				return _service.Register(username, HashHelper.GetMd5Hash(password));
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
				login = _service.Login(username, HashHelper.GetMd5Hash(password));
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
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}

			return login;
		}

		public GameDescription[] GetGameList()
		{
			try
			{
				return _service.GetGameList();
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
				return _service.CreateGame(mode, maxPlayers, tile);
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
				return _service.JoinGame(game);
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public void Move(Vector2 direction)
		{
			Move(TypeConverter.Xna2XnaLite(direction));
		}

		public AGameEvent[] Move(XNA.Framework.Vector2 direction)
		{
			try
			{
				// var sw = new Stopwatch();
				// sw.Start();
				return _service.Move(direction);
				// sw.Stop();
				// Trace.WriteLine("SW:serv:Move " + sw.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public AGameEvent[] ChangeWeapon(AWeapon.AWeaponType type)
		{
			// do nothing
			return null;
			throw new NotImplementedException();
		}

		public AGameEvent[]  Shoot(XNA.Framework.Vector2 direction)
		{
			try
			{
				// var sw = new Stopwatch();
				// sw.Start();
				return _service.Shoot(direction);
				// sw.Stop();
				// Trace.WriteLine("SW:serv:Shoot " + sw.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public AGameEvent[] GetEvents()
		{
			try
			{
				return _service.GetEvents();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public void Shoot(Vector2 direction)
		{
			Shoot(TypeConverter.Xna2XnaLite(direction));
		}

		//public void TakeBonus(AObtainableDamageModifier bonus)
		//{
		//  try
		//  {
		//    //_service.TakeBonus(bonus);
		//  }
		//  catch (Exception e)
		//  {
		//    FatalError(e);
		//  }
		//}

		public void TakePerk(Perk perk)
		{
			try
			{
				//_service.TakePerk(perk);
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
				_service.LeaveGame();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public Contracts.Session.GameLevel GameStart(int gameId)
		{
			try
			{
				return _service.GameStart(gameId);
			}
			catch (Exception e)
			{
				FatalError(e);
				throw;
			}
		}

		public AGameObject[] SynchroFrame()
		{
			try
			{
				return _service.SynchroFrame();
			}
			catch (Exception exc)
			{
				Trace.WriteLine("game:SynchroFrame"+exc);
				return new AGameObject[] {};
			}
		}

		public string[] PlayerListUpdate()
		{
			try
			{
				return _service.PlayerListUpdate();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
				return new string[] {};
			}
		}

		#endregion
	}
}