using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Service;
using SkyShoot.Game.Game;

namespace SkyShoot.Game.Screens
{
	internal class MainMenuScreen : GameScreen
	{
		private static Texture2D _texture;

		private ButtonControl _playGameButton;
		private ButtonControl _optionsButton;
		private ButtonControl _logoffButton;

		public MainMenuScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/screens/screen_05_fix");
		}

		public override void UnloadContent()
		{
			ContentManager.Unload();
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			_playGameButton = new ButtonControl
			{
				Text = "Multiplayer",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.2f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_optionsButton = new ButtonControl
			{
				Text = "Options",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.35f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_logoffButton = new ButtonControl
			{
				Text = "Logoff",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.5f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_playGameButton);
			Desktop.Children.Add(_optionsButton);
			Desktop.Children.Add(_logoffButton);

			ScreenManager.Instance.Controller.AddListener(_playGameButton, PlayGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_optionsButton, OptionsButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_logoffButton, LogoffMenuButtonPressed);
		}

		private void PlayGameButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MultiplayerScreen);
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.OptionsMenuScreen);
		}

		private void LogoffMenuButtonPressed(object sender, EventArgs e)
		{
			AccountManagerErrorCode errorCode = GameController.Instance.Logout();
			if (errorCode != AccountManagerErrorCode.Ok) {
				string message;
				switch (errorCode)
				{
					case AccountManagerErrorCode.UnknownExceptionOccured:
						message = "Unknown exception occured";
						break;
					case AccountManagerErrorCode.UserIsAlreadyOffline:
						message = "You is already offline";
						break;
					case AccountManagerErrorCode.UnknownError:
						message = "Unknown error occured";
						break;
					default:
						message = "Unexpected error code returned";
						break;
				}
				MessageBox.Message = message;
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}
	}
}
