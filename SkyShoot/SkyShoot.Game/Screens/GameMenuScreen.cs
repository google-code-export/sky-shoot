using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Game.Controls;
using Microsoft.Xna.Framework.Audio;

namespace SkyShoot.Game.Screens
{
	class GameMenuScreen : GameScreen
	{
		AudioEngine engine;
		SoundBank soundBank;
		WaveBank waveBank;

		private static Texture2D _texture;

		private readonly ContentManager _content;

		private SpriteBatch _spriteBatch;

		private ButtonControl _continueButton;
		private ButtonControl _optionsButton;
		private ButtonControl _exitButton;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public GameMenuScreen()
		{
			CreateControls();
			InitializeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

		private void CreateControls()
		{
			_continueButton = new ButtonControl
			{
				Text = "Continue",
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

			_exitButton = new ButtonControl
			{
				Text = "Exit",
				Bounds =
					new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.5f, 0),
													 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_continueButton);
			Desktop.Children.Add(_optionsButton);
			Desktop.Children.Add(_exitButton);

			ScreenManager.Instance.Controller.AddListener(_continueButton, ContinueButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_optionsButton, OptionsButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, ExitButtonPressed);

			engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
			waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_06");
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		private void ContinueButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.OptionsMenuScreen);
		}

		private void ExitButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}
	}
}
