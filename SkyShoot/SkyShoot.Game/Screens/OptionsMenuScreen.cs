using System;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using SkyShoot.Game.Controls;

using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Screens
{
	internal class OptionsMenuScreen : GameScreen
	{
		private ListControl _keyboardList;

		private LabelControl _titleLabel;
		private LabelControl _cursorLabel;
		private LabelControl _keyboardLabel;
		private LabelControl _fullscreenLabel;
		private LabelControl _volumeLabel;

		private OptionControl _fullscreenButton;

		private ChoiceControl _arrowButton;
		private ChoiceControl _plusButton;
		private ChoiceControl _crossButton;
		private ChoiceControl _targetButton;

		private ButtonControl _backButton;
		private ButtonControl _upVolume;
		private ButtonControl _downVolume;
		private LabelControl _volumeValueLabel;

		AudioEngine engine;
		SoundBank soundBank;
		WaveBank waveBank;
		AudioCategory musicCategory;
		
		private readonly ContentManager _content;

		private static Texture2D _texture;

		private SpriteBatch _spriteBatch;

		public static short Curs = 1;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public OptionsMenuScreen()
		{
			CreateControls();
			InitializeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

		private void CreateControls()
		{
			_titleLabel = new LabelControl("Options")
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.1f, -70), 100, 30)
			};

			_fullscreenLabel = new LabelControl("FullScreen: ")
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.25f, -70), 80, 30)
			};

			_fullscreenButton = new OptionControl
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, 30), new UniScalar(0.25f, -70), 100, 30)
			};

			_keyboardLabel = new LabelControl("Keyboard:")
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -150), new UniScalar(0.4f, -70), 50, 30)
			};

			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(1.1f, -70), 100, 30)
			};

			_upVolume = new ButtonControl
			{
				Text = "+",
				Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.9f, -70), 100, 30)
			};

			_downVolume = new ButtonControl
			{
				Text = "-",
				Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.9f, -30), 100, 30)
			};

			_volumeLabel = new LabelControl("Volume:")
			{
				Bounds =
					new UniRectangle(new UniScalar(0.62f, -220), new UniScalar(0.94f, -70), 70, 30)
			};

			_volumeValueLabel = new LabelControl
			{
				Text = (Math.Round(Settings.Default.Volume,1)).ToString(),
				Bounds =
					new UniRectangle(new UniScalar(1.0f, -220), new UniScalar(0.94f, -70), 70, 30)
			};


			_cursorLabel = new LabelControl("Cursor:")
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -220), new UniScalar(0.58f, -70), 70, 30)
			};

			_arrowButton = new ChoiceControl
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -140), new UniScalar(0.7f, -70), 70, 30)
			};

			_crossButton = new ChoiceControl
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, 40), new UniScalar(0.7f, -70), 70, 30)
			};

			_keyboardList = new ListControl
			{
				Bounds = new UniRectangle(260f, 125f, 150f, 50f)
			};

			_keyboardList.Items.Add("A, S, D, W");
			_keyboardList.Items.Add("Arrows");
			_keyboardList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_keyboardList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_keyboardList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_keyboardList.SelectionMode = ListSelectionMode.Single;

			_plusButton = new ChoiceControl
			{
				Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.7f, -70), 70, 30)
			};

			_targetButton = new ChoiceControl
			{
				Bounds =
					new UniRectangle(new UniScalar(0.5f, 130), new UniScalar(0.7f, -70), 70, 30)
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_titleLabel);
			Desktop.Children.Add(_fullscreenLabel);
			Desktop.Children.Add(_fullscreenButton);
			Desktop.Children.Add(_keyboardLabel);
			Desktop.Children.Add(_keyboardList);
			Desktop.Children.Add(_cursorLabel);
			Desktop.Children.Add(_arrowButton);
			Desktop.Children.Add(_plusButton);
			Desktop.Children.Add(_crossButton);
			Desktop.Children.Add(_targetButton);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_upVolume);
			Desktop.Children.Add(_downVolume);
			Desktop.Children.Add(_volumeLabel);
			Desktop.Children.Add(_volumeValueLabel);


			// todo изменить подписку
			_keyboardList.SelectionChanged += KeyboardChoice;

			_fullscreenButton.Selected = Settings.Default.FullScreenSelected;

			_fullscreenButton.Changed += FullScreenSelected;
			_arrowButton.Changed += ArrowButtonPressed;
			_plusButton.Changed += PlusButtonPressed;
			_crossButton.Changed += CrossButtonPressed;
			_targetButton.Changed += TargetButtonPressed;
			_upVolume.Pressed += UpButtonPressed;
			_downVolume.Pressed += DownButtonPressed;

			engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
			waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");
			musicCategory = engine.GetCategory("Music");

			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_upVolume, UpButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_downVolume, DownButtonPressed);
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			Textures.Arrow = _content.Load<Texture2D>("Textures/Cursors/Arrow");
			Textures.Plus = _content.Load<Texture2D>("Textures/Cursors/Plus");
			Textures.Cross = _content.Load<Texture2D>("Textures/Cursors/Cross");
			Textures.Target = _content.Load<Texture2D>("Textures/Cursors/Target");

			_keyboardList.SelectedItems.Add(Settings.Default.KeyboardLayout == 0 ? 0 : 1);

			switch (Settings.Default.Cursor)
			{
				case 1:
					_arrowButton.Selected = true;
					break;
				case 2:
					_plusButton.Selected = true;
					break;
				case 3:
					_plusButton.Selected = true;
					break;
				case 4:
					_targetButton.Selected = true;
					break;
			}
		}

		public override void UnloadContent()
		{

		}

		private void ArrowButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Curs = 1;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();			
		}

		private void PlusButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Curs = 2;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void CrossButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Curs = 3;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void TargetButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Curs = 4;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void KeyboardChoice(object sender, EventArgs e)
		{
			Settings.Default.KeyboardLayout = (short)(_keyboardList.SelectedItems[0] == 0 ? 0 : 1);
			Settings.Default.Save();
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			if (Client.Game.GameController.Instance.IsGameStarted)
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);
			else
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}

		private void UpButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Settings.Default.Volume = MathHelper.Clamp(Settings.Default.Volume + 0.05f, 0.0f, 1.0f);
			Settings.Default.Save();
			musicCategory.SetVolume(Settings.Default.Volume);
			_volumeValueLabel.Text = (100*Math.Round(Settings.Default.Volume, 1)).ToString()+"%";
		}

		private void DownButtonPressed(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Settings.Default.Volume = MathHelper.Clamp(Settings.Default.Volume - 0.05f, 0.0f, 1.0f);
			Settings.Default.Save();
			musicCategory.SetVolume(Settings.Default.Volume);
			_volumeValueLabel.Text = (100*Math.Round(Settings.Default.Volume, 1)).ToString()+"%";
		}

		private void FullScreenSelected(object sender, EventArgs e)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			Settings.Default.FullScreenSelected = _fullscreenButton.Selected;
			Settings.Default.Save();
		}

		public override void Update(GameTime gameTime)
		{
			engine.Update();
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();

			_spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
			var pos1 = new Vector2(250f, 270f);
			var pos2 = new Vector2(340f, 270f);
			var pos3 = new Vector2(430f, 270f);
			var pos4 = new Vector2(520f, 270f);
			_spriteBatch.Draw(Textures.Arrow, pos1, Color.White);
			_spriteBatch.Draw(Textures.Plus, pos2, Color.White);
			_spriteBatch.Draw(Textures.Cross, pos3, Color.White);
			_spriteBatch.Draw(Textures.Target, pos4, Color.White);
			_spriteBatch.End();
		}
	}
}
