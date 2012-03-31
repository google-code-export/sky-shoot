using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Contracts.Session;

using SkyShoot.Game.Controls;

using SkyShoot.Game.Client.Game;
using Microsoft.Xna.Framework.Audio;

namespace SkyShoot.Game.Screens
{
	internal class WaitScreen : GameScreen
	{
		private AudioEngine _engine;
		private SoundBank _soundBank;
		WaveBank _waveBank;

		public static String Tile { get; set; }

		public static String GameMode { get; set; }

		public static String MaxPlayers { get; set; }

		public static Int32 GameId { get; set; }

		private ListControl _playersList;
		private ButtonControl _leaveButton;

		private List<string> _tmpPlayersList;

		private readonly ContentManager _content;
		private static Texture2D _texture;

		private SpriteBatch _spriteBatch;
		private SpriteFont _spriteFont;

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public WaitScreen()
		{
			CreateControls();
			InititalizeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
			_updateCount = 0;
		}

		private void CreateControls()
		{
			_playersList = new ListControl
			               	{
			               		Bounds = new UniRectangle(-60f, -4f, 200f, 300f)
			               	};

			_leaveButton = new ButtonControl
			               	{
			               		Text = "Leave",
			               		Bounds = new UniRectangle(new UniScalar(0.5f, -378f), new UniScalar(0.4f, 190f), 120, 32)
			               	};

			_playersList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_playersList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_playersList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_playersList.SelectionMode = ListSelectionMode.Single;
		}

		private void InititalizeControls()
		{
			Desktop.Children.Add(_playersList);
			Desktop.Children.Add(_leaveButton);

			ScreenManager.Instance.Controller.AddListener(_leaveButton, LeaveButtonPressed);

			_engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			_soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
			_waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");
		}

		private int _updateCount ;
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			// обновляемся только каждый 30 апдейт (два раза в секунду)
			if(_updateCount++ % 30 != 0)
				return;
			var level = GameController.Instance.GameStart(GameId);
			if (level != null)
			{
				// game started
				GameController.Instance.GameStart(level);
			}
			else
			{
				// game has not started, update player list
				ChangePlayerList(GameController.Instance.PlayerListUpdate());
			}
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_02_fix");
			_spriteFont = _content.Load<SpriteFont>("Times New Roman");

			// todo rename variable
			// вывод списка игрков
			GameDescription[] tmpGameDescriptionList = GameController.Instance.GetGameList();

			if (tmpGameDescriptionList == null)
				return;

			foreach (GameDescription gameDescription in tmpGameDescriptionList)
			{
				if (GameId == gameDescription.GameId)
				{
					_tmpPlayersList = gameDescription.Players;
				}
			}

			if (_tmpPlayersList == null)
				return;

			foreach (string player in _tmpPlayersList)
			{
				_playersList.Items.Add(player);
			}
			// todo Разобраться
			_playersList.SelectedItems.Add(4);
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		private void LeaveButtonPressed(object sender, EventArgs args)
		{
			Cue cue = _soundBank.GetCue("RICOCHET");
			cue.Play();

			GameController.Instance.LeaveGame();
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MultiplayerScreen);
		}

		public void ChangePlayerList(String[] names)
		{
			_playersList.Items.Clear();
			foreach (string playerName in names)
			{
				_playersList.Items.Add(playerName);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.DrawString(_spriteFont, "Players", new Vector2(20f, 25f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, "Map: ", new Vector2(280f, 260f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, Tile, new Vector2(400f, 260f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, "Game Mode:", new Vector2(280f, 290f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, GameMode, new Vector2(400f, 290f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, "Max Players:", new Vector2(280f, 320f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, MaxPlayers, new Vector2(400f, 320f), Color.Red, 0,
			                        new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.End();
		}
	}
}
