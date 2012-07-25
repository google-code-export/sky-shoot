using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Service;
using SkyShoot.Game.Game;
using SkyShoot.Game.Network;
using InputControl = SkyShoot.Game.Input.InputControl;

namespace SkyShoot.Game.Screens
{
	internal class NewAccountScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _loginLabel;
		private LabelControl _passwordLabel;

		private InputControl _loginBox;
		private InputControl _passwordBox;

		private ButtonControl _backButton;
		private ButtonControl _okButton;

		public NewAccountScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/screens/screen_05_fix");
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			// Login Input
			_loginBox = new InputControl
			{
				IsHidden = false,
				Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
				Text = string.Empty
			};

			// Password Input
			_passwordBox = new InputControl
			{
				IsHidden = true,
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
				RealText = string.Empty,
				Text = string.Empty
			};

			// Login Label
			_loginLabel = new LabelControl("Username")
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
			};

			// Password Label
			_passwordLabel = new LabelControl("Password")
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
			};

			// Back Button
			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32)
			};

			// Login Button
			_okButton = new ButtonControl
			{
				Text = "Create",
				Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_loginBox);
			Desktop.Children.Add(_passwordBox);
			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_okButton);

			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_okButton, OkButtonPressed);
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}

		private void OkButtonPressed(object sender, EventArgs args)
		{
			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				MessageBox.Next = ScreenManager.ScreenEnum.NewAccountScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				MessageBox.Next = ScreenManager.ScreenEnum.NewAccountScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.RealText;
				Settings.Default.Save();

				AccountManagerErrorCode errorCode = ConnectionManager.Instance.Register(_loginBox.Text, _passwordBox.RealText);

				if (errorCode == AccountManagerErrorCode.Ok)
				{
					if (GameController.Instance.Login(_loginBox.Text, _passwordBox.RealText, out errorCode).HasValue)
					{
						ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
					}
				}
				else
				{
					string message = "Registration failed: ";
					switch (errorCode)
					{
						case AccountManagerErrorCode.UnknownExceptionOccured:
							message += "Unknown exception occured";
							break;
						case AccountManagerErrorCode.UsernameTaken:
							message += "This username is already taken, please try another";
							break;
						case AccountManagerErrorCode.UnknownError:
							message += "Unknown error occured";
							break;
						default:
							message += "Unexpected error code returned";
							break;
					}
					MessageBox.Message = message;
					MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
					ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
				}
			}
		}
	}
}
