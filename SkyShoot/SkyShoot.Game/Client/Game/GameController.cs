using System;

using System.Text;

using System.Security.Cryptography;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;

using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Contracts.Weapon;

using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Client.Game
{
	public class HashHelper
	{
        /// <summary>
        /// выдаёт последовательность из 32 шестнадцатеричных цифр (md5 хеш от аргумента)
        /// </summary>
		public static string GetMd5Hash(string input)
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

		public static GameController Instance
		{
			get { return _localInstance ?? (_localInstance = new GameController()); }
		}

		public GameModel GameModel { get; private set; }

		private GameController()
		{
			// InitConnection();

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

		#region обработка ввода

		private DateTime _dateTime;
		private const int RATE = 1000 / 10;

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
				if ((DateTime.Now - _dateTime).Milliseconds > RATE)
				{
					_dateTime = DateTime.Now;
					Shoot(player.ShootVectorM);
				}
			}
		}

		#endregion

		#region регистрация и прочее

		public bool Register(string username, string password)
		{
			// TODO обращаться напрямую
			return ConnectionManager.Instance.Register(username, password);
		}

		public Guid? Login(string username, string password)
		{
			// TODO check for null
			MyId = ConnectionManager.Instance.Login(username, password).Value;
			return MyId;
		}

		public GameDescription[] GetGameList()
		{
			return ConnectionManager.Instance.GetGameList();
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tile, int teams)
		{
			return ConnectionManager.Instance.CreateGame(mode, maxPlayers, tile);
		}

		public bool JoinGame(GameDescription game)
		{
			return ConnectionManager.Instance.JoinGame(game);
		}

		public void LeaveGame()
		{
			ConnectionManager.Instance.LeaveGame();
		}

		public Contracts.Session.GameLevel GameStart(int gameId)
		{
			return ConnectionManager.Instance.GameStart(gameId);
		}

		public string[] PlayerListUpdate()
		{
			return ConnectionManager.Instance.PlayerListUpdate();
		}

		#endregion

		#region сама игра
		
		public AGameObject[] SynchroFrame()
		{
			return ConnectionManager.Instance.SynchroFrame();
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

		// TODO в ConnectionManager
		//		public void TakePerk(Perk perk)
		//		{
		//			try
		//			{
		//				//_service.TakePerk(perk);
		//			}
		//			catch (Exception e)
		//			{
		//				FatalError(e);
		//			}
		//		}

		public void Shoot(Vector2 direction)
		{
			Shoot(TypeConverter.Xna2XnaLite(direction));
		}

		public void Move(Vector2 direction)
		{
			Move(TypeConverter.Xna2XnaLite(direction));
		}

		public AGameEvent[] Move(XNA.Framework.Vector2 direction)
		{
			return ConnectionManager.Instance.Move(direction);
		}

		public AGameEvent[] ChangeWeapon(WeaponType type)
		{
			// TODO в ConnectionManager
			// do nothing
			//			try
			//			{
			//				// var sw = new Stopwatch();
			//				// sw.Start();
			//				return _service.ChangeWeapon(type);
			//				// sw.Stop();
			//				// Trace.WriteLine("SW:serv:ChangeWeapon " + sw.ElapsedMilliseconds);
			//			}
			//			catch (Exception e)
			//			{
			//				FatalError(e);
			//				return null;
			//			}
			return ConnectionManager.Instance.ChangeWeapon(type);
		}

		public Stats? GetStats() // Статистика
		{
			try
			{
				return _service.GetStats();
			}
			catch
			{
				return null;
			}
		}
		
		public AGameEvent[] Shoot(XNA.Framework.Vector2 direction)
		{
			// TODO в ConnectionManager через очередь
			//			try
			//			{
			//				// var sw = new Stopwatch();
			//				// sw.Start();
			//				return _service.Shoot(direction);
			//				// sw.Stop();
			//				// Trace.WriteLine("SW:serv:Shoot " + sw.ElapsedMilliseconds);
			//			}
			//			catch (Exception e)
			//			{
			//				FatalError(e);
			//				return null;
			//			}
			return ConnectionManager.Instance.Shoot(direction);
		}

		public AGameEvent[] GetEvents()
		{
			// TODO в ConnectionManager через очередь
			//			try
			//			{
			//				return _service.GetEvents();
			//			}
			//			catch (Exception e)
			//			{
			//				FatalError(e);
			//				return null;
			//			}
			return ConnectionManager.Instance.GetEvents();
		}

		#endregion
	}
}