using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkyShoot.Game.Client.View;
using SkyShoot.Game.ScreenManager;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SkyShoot.Game.Screens
{
    class MainMenuScreen : ScreenManager.GameScreen
    {
        private GuiManager _gui;
        private Screen _mainScreen;
		private static Texture2D _texture;
		private ContentManager _content;
    	private SpriteBatch spriteBatch;

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

            ButtonControl _playGameButton = new ButtonControl()
            {
                Text = "Multiplayer",
                Bounds = new UniRectangle(new UniScalar(0.30f,0),new UniScalar(0.2f,0),new UniScalar(0.4f,0), new UniScalar(0.1f,0)),
            };
            _mainScreen.Desktop.Children.Add(_playGameButton);

            ButtonControl _optionsButton = new ButtonControl()
            {
                Text = "Options",
                Bounds = new UniRectangle(new UniScalar(0.30f,0),new UniScalar(0.35f,0),new UniScalar(0.4f,0), new UniScalar(0.1f,0)),
            };
            _mainScreen.Desktop.Children.Add(_optionsButton);

            ButtonControl _logoffButton = new ButtonControl()
            {
                Text = "Logoff",
                Bounds = new UniRectangle(new UniScalar(0.30f,0),new UniScalar(0.5f,0),new UniScalar(0.4f,0), new UniScalar(0.1f,0)),
            };
            _mainScreen.Desktop.Children.Add(_logoffButton);

            _playGameButton.Pressed += PlayGameButtonPressed;
            _optionsButton.Pressed += OptionsButtonPressed;
            _logoffButton.Pressed += LogoffMenuButtonPressed;

        }
        
        void PlayGameButtonPressed(object sender, EventArgs e)
        {
			ExitScreen();
            ScreenManager.AddScreen(new MultiplayerScreen()); 
        }


        void OptionsButtonPressed(object sender, EventArgs e)
        {
			ExitScreen();
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        void LogoffMenuButtonPressed(object sender, EventArgs e)
        {
            ExitScreen();
            ScreenManager.AddScreen(new LoginScreen());
        }

		public override void Update(GameTime gameTime, bool otherHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherHasFocus, coveredByOtherScreen);
		}

        public override void Draw(GameTime gameTime)
		{
			spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin();
			spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			spriteBatch.End();
			_gui.Draw(gameTime);
			base.Draw(gameTime);
        }
    }
}
