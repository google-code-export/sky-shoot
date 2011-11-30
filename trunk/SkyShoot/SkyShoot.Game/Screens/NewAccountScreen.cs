using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Game.ScreenManager;

using SkyShoot.Game.Client.Game;


namespace SkyShoot.Game.Screens
{
    class NewAccountScreen:GameScreen
    {
        private GuiManager _gui;
        private LabelControl _loginLabel;
        private InputControl _loginBox;
        private LabelControl _passwordLabel;
        private InputControl _passwordBox;
        private ButtonControl _backButton;
        private ButtonControl _okButton;
        private Screen _mainScreen;
		private static Texture2D _texture;
		private ContentManager _content;
		private SpriteBatch _spriteBatch;

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;
			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
            );

            // Login Input
            _loginBox = new InputControl
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
                Text = ""
            };
            _mainScreen.Desktop.Children.Add(_loginBox);

            // Password Input
            _passwordBox = new InputControl
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
                Text = ""
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

            // Back Button
            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32)
            };
            _backButton.Pressed += BackButtonPressed;
            _mainScreen.Desktop.Children.Add(_backButton);

            // Login Button
            _okButton = new ButtonControl
            {
                Text = "OK",
                Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
            };
            _okButton.Pressed += OkButtonPressed;
            _mainScreen.Desktop.Children.Add(_okButton);

        }

        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
        }

        private void OkButtonPressed(object sender, EventArgs args)
        {
			if (_loginBox.Text.Length < 3)
			{
				Settings.Default.Message = 0;
				Settings.Default.Save();
				ScreenManager.AddScreen(new MessageBox());
			}
			else if (_passwordBox.Text.Length < 3)
			{
				Settings.Default.Message = 1;
				Settings.Default.Save();
				ScreenManager.AddScreen(new MessageBox());
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
						ScreenManager.AddScreen(new MultiplayerScreen());
						ExitScreen();
					}
				}
			}
            
        }


        public override void Draw(GameTime gameTime)
        {
			_spriteBatch = ScreenManager.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }
}
