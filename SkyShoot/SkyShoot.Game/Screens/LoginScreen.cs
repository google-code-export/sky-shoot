using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Service;
using SkyShoot.Game.Game;
using InputControl = SkyShoot.Game.Input.InputControl;

namespace SkyShoot.Game.Screens
{
	internal class LoginScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _loginLabel;
		private LabelControl _passwordLabel;

		private InputControl _loginBox;
		private InputControl _passwordBox;

		private ButtonControl _exitButton;
		private ButtonControl _loginButton;
		private ButtonControl _newAccountButton;

		public LoginScreen()
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
			// Login Input
			_loginBox = new InputControl
							{
								IsHidden = false,
								Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
								Text = Settings.Default.login
							};

			// Password Input
			_passwordBox = new InputControl
							{
								IsHidden = true,
								Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
								RealText = Settings.Default.password,
								Text = InputControl.HiddenText(Settings.Default.password)
							};

			// Login Label
			_loginLabel = new LabelControl("Username")
							{
								Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
							};

			// Password Label
			_passwordLabel = new LabelControl("Password")
								{
									Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
								};

			// Login Button
			_loginButton = new ButtonControl
							{
								Text = "Login",
								Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
							};

			// Back Button
			_exitButton = new ButtonControl
							{
								Text = "Exit",
								Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32),
							};

			// New Account Button
			_newAccountButton = new ButtonControl
									{
										Text = "Create new account",
										Bounds = new UniRectangle(new UniScalar(0.5f, -75f), new UniScalar(0.4f, 70), 150, 32)
									};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_loginBox);
			Desktop.Children.Add(_passwordBox);
			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_newAccountButton);
			Desktop.Children.Add(_loginButton);

			ScreenManager.Instance.Controller.AddListener(_loginButton, LoginButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, ExitButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_newAccountButton, NewAccountButtonPressed);
		}

		private void ExitButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.Game.Exit();
		}

		private void LoginButtonPressed(object sender, EventArgs args)
		{
			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.RealText;
				Settings.Default.Save();

				AccountManagerErrorCode errorCode;

				if (GameController.Instance.Login(_loginBox.Text, _passwordBox.RealText, out errorCode).HasValue &&
					errorCode == AccountManagerErrorCode.Ok)
				{
					ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
				}
				else
				{
					string message;
					switch (errorCode)
					{
						case AccountManagerErrorCode.UnknownExceptionOccured:
							message = "Unknown exception occured";
							break;
						case AccountManagerErrorCode.InvalidUsernameOrPassword:
							message = "Invalid username or password";
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
			}
		}

		private void NewAccountButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.NewAccountScreen);
		}
	}
}