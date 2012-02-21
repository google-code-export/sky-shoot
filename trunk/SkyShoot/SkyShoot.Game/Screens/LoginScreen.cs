using System;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class LoginScreen : GameScreen
	{
		private GuiManager _gui;

		private readonly LabelControl _loginLabel;
		private readonly LabelControl _passwordLabel;

		private readonly Controls.InputControl _loginBox;
		private readonly Controls.InputControl _passwordBox;

		private readonly ButtonControl _exitButton;
		private readonly ButtonControl _loginButton;
		private readonly ButtonControl _newAccountButton;

		private static Texture2D _texture;

		private ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public LoginScreen()
		{
			// Login Input
			_loginBox = new Controls.InputControl
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
				Text = Settings.Default.login
			};

			// Password Input
			_passwordBox = new Controls.InputControl
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
				Text = Settings.Default.password
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
				Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32)
			};

			// New Account Button
			_newAccountButton = new ButtonControl
			{
				Text = "Create new account",
				Bounds = new UniRectangle(new UniScalar(0.5f, -75f), new UniScalar(0.4f, 70), 150, 32)
			};

			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			_gui = ScreenManager.Instance.Gui;
			_gui.Screen = this;

			if (_content == null)
				_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			Desktop.Children.Add(_passwordBox);
			Desktop.Children.Add(_loginBox);
			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_loginButton);
			Desktop.Children.Add(_newAccountButton);

			_loginButton.Pressed += LoginButtonPressed;
			_exitButton.Pressed += ExitButtonPressed;
			_newAccountButton.Pressed += NewAccountButtonPressed;
		}

		public override void UnloadContent()
		{
			Desktop.Children.Remove(_passwordBox);
			Desktop.Children.Remove(_loginBox);
			Desktop.Children.Remove(_loginLabel);
			Desktop.Children.Remove(_passwordLabel);
			Desktop.Children.Remove(_exitButton);
			Desktop.Children.Remove(_loginButton);
			Desktop.Children.Remove(_newAccountButton);

			_loginButton.Pressed -= LoginButtonPressed;
			_exitButton.Pressed -= ExitButtonPressed;
			_newAccountButton.Pressed -= NewAccountButtonPressed;
		}

		private void ExitButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.Game.Exit();
		}

		private void LoginButtonPressed(object sender, EventArgs args)
		{
			FocusedControl = null;
			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MessageScreen;
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MessageScreen;
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.Text;
				Settings.Default.Save();

				if (GameController.Instance.Login(_loginBox.Text, _passwordBox.Text).HasValue)
				{
					ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MainMenuScreen;
				}
			}
		}

		private void NewAccountButtonPressed(object sender, EventArgs args)
		{
			FocusedControl = null;
			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MessageScreen;
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MessageScreen;
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.Text;
				Settings.Default.Save();

				if (GameController.Instance.Register(_loginBox.Text, _passwordBox.Text))
				{
					if (GameController.Instance.Login(_loginBox.Text, _passwordBox.Text).HasValue)
					{
						ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MainMenuScreen;
					}
				}
				else
				{
					MessageBox.Message = "Registration failed";
					ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MessageScreen;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();

			_gui.Draw(gameTime);
			base.Draw(gameTime);
		}
	}
}
