using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Controls;
using InputControl = Nuclex.UserInterface.Controls.Desktop.InputControl;

namespace SkyShoot.Game.Screens
{
	internal class NewAccountScreen : GameScreen
	{
		private GuiManager _gui;

		private readonly LabelControl _loginLabel;
		private readonly LabelControl _passwordLabel;

		private readonly InputControl _loginBox;
		private readonly InputControl _passwordBox;

		private readonly ButtonControl _backButton;
		private readonly ButtonControl _okButton;

		private static Texture2D _texture;

		private ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public NewAccountScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;

			// Login Input
			_loginBox = new InputControl
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
				Text = ""
			};

			// Password Input
			_passwordBox = new InputControl
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
				Text = ""
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
				Text = "OK",
				Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
			};
		}

		public override void LoadContent()
		{
			base.LoadContent();

			_gui = ScreenManager.Instance.Gui;
			_gui.Screen = this;

			if (_content == null)
				_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			Desktop.Children.Add(_loginBox);
			Desktop.Children.Add(_passwordBox);
			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_okButton);

			_backButton.Pressed += BackButtonPressed;
			_okButton.Pressed += OkButtonPressed;
		}

		public override void UnloadContent()
		{
			Desktop.Children.Remove(_loginBox);
			Desktop.Children.Remove(_passwordBox);
			Desktop.Children.Remove(_loginLabel);
			Desktop.Children.Remove(_passwordLabel);
			Desktop.Children.Remove(_backButton);
			Desktop.Children.Remove(_okButton);
			Desktop.Children.Remove(_loginBox);
			Desktop.Children.Remove(_passwordBox);
			Desktop.Children.Remove(_loginLabel);
			Desktop.Children.Remove(_passwordLabel);
			Desktop.Children.Remove(_backButton);
			Desktop.Children.Remove(_okButton);

			_backButton.Pressed -= BackButtonPressed;
			_okButton.Pressed -= OkButtonPressed;
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{

		}

		private void OkButtonPressed(object sender, EventArgs args)
		{
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
						ScreenManager.Instance.ActiveScreen =
							ScreenManager.ScreenEnum.MultiplayerScreen;
					}
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
			
			base.Draw(gameTime);
			_gui.Draw(gameTime);
		}
	}
}
