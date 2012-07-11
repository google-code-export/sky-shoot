using System;

using System.Text;

using System.Security.Cryptography;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;

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

	public sealed class GameController
	{
		#region singleton

		private static GameController _localInstance;

		public static GameController Instance
		{
			get { return _localInstance ?? (_localInstance = new GameController()); }
		}

		private GameController()
		{
			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;
		}

		#endregion

		private SoundManager _soundManager;

		public static Guid MyId { get; private set; }

		public bool IsGameStarted { get; private set; }

		public GameModel GameModel { get; private set; }

		public void GameStart(Contracts.Session.GameLevel arena)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);

			GameModel = new GameModel(GameFactory.CreateClientGameLevel(arena));

			var gameObjects = ConnectionManager.Instance.SynchroFrame();

			foreach (AGameObject mob in gameObjects)
			{
				var clientMob = GameFactory.CreateClientMob(mob);
				GameModel.AddGameObject(clientMob);
			}

			// GameModel initialized, set boolean flag  
			IsGameStarted = true;
		}

		#region бывший callbacks

		public void GameObjectDead(AGameObject mob)
		{
			GameModel.RemoveGameObject(mob.Id);
			// todo //!!
			//GameModel.GameLevel.AddTexture(mob.Is(AGameObject.EnumObjectType.Player)
			//                                ? Textures.DeadPlayerTexture
			//                                : Textures.DeadSpiderTexture, TypeConverter.XnaLite2Xna(mob.Coordinates));
		}

		public void GameOver()
		{
			GameModel = null;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			IsGameStarted = false;
		}

//		public void PlayerLeft(AGameObject mob)
//		{
//			if (IsGameStarted)
//				GameModel.RemoveGameObject(mob.Id);
//		}

		#endregion

		#region обработка ввода

		private DateTime _dateTime;
		private const int RATE = 1000 / 10;

		public void HandleInput(Controller controller)
		{
			var player = GameModel.GetGameObject(MyId);

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

		public Guid? Login(string username, string password, out AccountManagerErrorCode errorCode)
		{
			// TODO check for null
			Guid? id = ConnectionManager.Instance.Login(username, password, out errorCode);
			if (id.HasValue)
			{
				MyId = id.Value;
			}
			return MyId;
		}

		public void Shoot(Vector2 direction)
		{
			ConnectionManager.Instance.Shoot(TypeConverter.Xna2XnaLite(direction));
		}

		public void Move(Vector2 direction)
		{
			ConnectionManager.Instance.Move(TypeConverter.Xna2XnaLite(direction));
		}
	}
}