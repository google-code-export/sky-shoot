using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Game.Game;

namespace SkyShoot.Game.Screens
{
	internal class MainMenuScreen : GameScreen
	{
		private static Texture2D _texture;

		private readonly SoundManager _soundManager;
		private readonly ContentManager _content;

		private SpriteBatch _spriteBatch;

		private ButtonControl _playGameButton;
		private ButtonControl _optionsButton;
		private ButtonControl _logoffButton;

		public MainMenuScreen()
		{
			CreateControls();
			InitializeControls();

			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;
			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
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
			_soundManager.SoundPlay(SoundManager.SoundEnum.Click);

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MultiplayerScreen);
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			_soundManager.SoundPlay(SoundManager.SoundEnum.Click);

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.OptionsMenuScreen);
		}

		private void LogoffMenuButtonPressed(object sender, EventArgs e)
		{
			_soundManager.SoundPlay(SoundManager.SoundEnum.Click);

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}
	}
}
