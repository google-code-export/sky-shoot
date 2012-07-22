using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SkyShoot.Contracts.Service;
using SkyShoot.Game.Input;
using SkyShoot.Game.Network;
using SkyShoot.Game.Screens;
using SkyShoot.Game.Utils;
using SkyShoot.Contracts.Utils;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Game
{
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
		}

		#endregion

		public static Guid MyId { get; private set; }

		// todo temporary
		public static long StartTime { get; set; }

		public bool IsGameStarted { get; private set; }

		public GameModel GameModel { get; private set; }

		private void Shoot(Vector2 direction)
		{
			ConnectionManager.Instance.Shoot(TypeConverter.Xna2XnaLite(direction));
		}

		private void Move(Vector2 direction)
		{
			ConnectionManager.Instance.Move(TypeConverter.Xna2XnaLite(direction));
		}

		public void GameStart(Contracts.Session.GameLevel arena, int gameId)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);

			var timeHelper = new TimeHelper(StartTime);

			var logger = new Logger(Logger.SolutionPath + "\\logs\\client_game_" + gameId + ".txt", timeHelper);

			GameModel = new GameModel(GameFactory.CreateClientGameLevel(arena), timeHelper);

			GameModel.Update(new GameTime());

			Trace.Listeners.Add(logger);
			Trace.WriteLine("Game initialized (model created, first synchroframe applied)");
			Trace.Listeners.Remove(Logger.ClientLogger);

			// GameModel initialized, set boolean flag  
			IsGameStarted = true;
		}

		public void GameOver()
		{
			ConnectionManager.Instance.Stop();
			GameModel = null;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			IsGameStarted = false;
		}

		#region обработка ввода

		private DateTime _dateTime;
		private const int RATE = 1000 / 10;

		public void HandleInput(Controller controller)
		{
			DrawableGameObject player = GameModel.GetGameObject(MyId);

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
	}
}