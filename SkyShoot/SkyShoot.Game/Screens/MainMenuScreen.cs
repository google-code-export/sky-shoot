using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class MainMenuScreen : GameScreen
	{
		private GuiManager _gui;

		private static Texture2D _texture;

		private ContentManager _content;

		private SpriteBatch _spriteBatch;

		private readonly ButtonControl _playGameButton;
		private readonly ButtonControl _optionsButton;
		private readonly ButtonControl _logoffButton;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public MainMenuScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;

			_playGameButton = new ButtonControl
			{
				Text = "Multiplayer",
				Bounds =
					new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.2f, 0),
									 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			};

			_optionsButton = new ButtonControl
			{
				Text = "Options",
				Bounds =
					new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.35f, 0),
									 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			};

			_logoffButton = new ButtonControl
			{
				Text = "Logoff",
				Bounds =
					new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.5f, 0),
									 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
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

			Desktop.Children.Add(_playGameButton);		
			Desktop.Children.Add(_optionsButton);
			Desktop.Children.Add(_logoffButton);

			ScreenManager.Instance.Controller.RegisterListener(_playGameButton, PlayGameButtonPressed);
			ScreenManager.Instance.Controller.RegisterListener(_optionsButton, OptionsButtonPressed);
			ScreenManager.Instance.Controller.RegisterListener(_logoffButton, LogoffMenuButtonPressed);

			//_playGameButton.Pressed += PlayGameButtonPressed;
			//_optionsButton.Pressed += OptionsButtonPressed;
			//_logoffButton.Pressed += LogoffMenuButtonPressed;
		}

		public override void UnloadContent()
		{
			Desktop.Children.Remove(_playGameButton);
			Desktop.Children.Remove(_optionsButton);
			Desktop.Children.Remove(_logoffButton);

			_playGameButton.Pressed -= PlayGameButtonPressed;
			_optionsButton.Pressed -= OptionsButtonPressed;
			_logoffButton.Pressed -= LogoffMenuButtonPressed;
		}

		private void PlayGameButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MultiplayerScreen;
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.OptionsScreen;
		}

		private void LogoffMenuButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.LoginScreen;
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
