using System;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.Game;


namespace SkyShoot.Game.Screens
{
    class LoginScreen:ScreenManager.GameScreen
    {
        private GuiManager _gui;
        private LabelControl _loginLabel;
        private Controls.InputControl _loginBox;
        private LabelControl _passwordLabel;
        private Controls.InputControl _passwordBox;
        private ButtonControl _exitButton;
        private ButtonControl _loginButton;
        private ButtonControl _newAccountButton;
        private Screen _mainScreen;
		private static Texture2D _texture;
		private ContentManager _content;
		private SpriteBatch _spriteBatch;

        public override void LoadContent()
        {
            base.LoadContent();
			_gui = ScreenManager.ScreenManager.Instance.Gui;
			Viewport viewport = ScreenManager.ScreenManager.Instance.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;
			if (_content == null)
				_content = new ContentManager(ScreenManager.ScreenManager.Instance.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f) 
            );

            // Login Input
            _loginBox = new Controls.InputControl
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
                Text = Settings.Default.login 
            };
            _mainScreen.Desktop.Children.Add(_loginBox);

            // Password Input
            _passwordBox = new Controls.InputControl
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
                Text = Settings.Default.password
            };
            _mainScreen.Desktop.Children.Add(_passwordBox);

            // Login Label
            _loginLabel = new LabelControl("Username")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
            };
            _mainScreen.Desktop.Children.Add(_loginLabel);
            
            // Password Label
            _passwordLabel = new LabelControl("Password")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
            };
            _mainScreen.Desktop.Children.Add(_passwordLabel);

            // Login Button
            _loginButton = new ButtonControl
            {
                Text = "Login",
                Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
            };
            _loginButton.Pressed += LoginButtonPressed;
            _mainScreen.Desktop.Children.Add(_loginButton);

            // Back Button
            _exitButton = new ButtonControl
            {
                Text = "Exit",
                Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32)
            };
            _exitButton.Pressed += ExitButtonPressed;
            _mainScreen.Desktop.Children.Add(_exitButton);

            // New Account Button
            _newAccountButton = new ButtonControl
            {
                Text = "Create new account",
                Bounds = new UniRectangle(new UniScalar(0.5f, -75f), new UniScalar(0.4f, 70), 150, 32)
            };
            _newAccountButton.Pressed += NewAccountButtonPressed;
            _mainScreen.Desktop.Children.Add(_newAccountButton);

        }

        private void ExitButtonPressed(object sender, EventArgs args)
        {
			ScreenManager.ScreenManager.Instance.Game.Exit();
        }

        private void LoginButtonPressed(object sender, EventArgs args)
        {
			_mainScreen.FocusedControl = null;
			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.Text;
				Settings.Default.Save();


				if (GameController.Instance.Login(_loginBox.Text, _passwordBox.Text).HasValue)
				{
					ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MainMenuScreen;
				}
			}
        }

        private void NewAccountButtonPressed(object sender, EventArgs args)
        {
            _mainScreen.FocusedControl = null;
			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
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
						ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MainMenuScreen;
					}
				}
				else
				{
					MessageBox.Message = "Registration failed";
					ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MessageScreen;
				}
            }
        }

        public override void Draw(GameTime gameTime)
        {
			_spriteBatch = ScreenManager.ScreenManager.Instance.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }
    }
}
