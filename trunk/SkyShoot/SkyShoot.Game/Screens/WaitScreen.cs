using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class WaitScreen : GameScreen
	{
		private static Texture2D _texture;

		private readonly SoundManager _soundManager;
		private readonly ContentManager _content;

		private ListControl _playersList;
		private ButtonControl _leaveButton;

		private List<string> _tmpPlayersList;

		private SpriteBatch _spriteBatch;
		private SpriteFont _spriteFont;

		private int _updateCount;

		public WaitScreen()
		{
			CreateControls();
			InititalizeControls();

			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;
			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
			_updateCount = 0;
		}

		public static string Tile { get; set; }

		public static string GameMode { get; set; }

		public static string MaxPlayers { get; set; }

		public static int GameId { get; set; }

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_02_fix");
			_spriteFont = _content.Load<SpriteFont>("Times New Roman");

			// todo rename variable
			// вывод списка игрков
			GameDescription[] tmpGameDescriptionList = ConnectionManager.Instance.GetGameList();

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

			// ToDo: Разобраться
			_playersList.SelectedItems.Add(4);
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		public void ChangePlayerList(string[] names)
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

			DrawString(_spriteBatch, "Players", 20f, 25f);
			DrawString(_spriteBatch, "Map: ", 280f, 260f);
			DrawString(_spriteBatch, Tile, 400f, 260f);
			DrawString(_spriteBatch, "Game Mode:", 280f, 290f);
			DrawString(_spriteBatch, GameMode, 400f, 290f);
			DrawString(_spriteBatch, "Max Players:", 280f, 320f);
			DrawString(_spriteBatch, MaxPlayers, 400f, 320f);

			_spriteBatch.End();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// обновляемся только каждый 30 апдейт (два раза в секунду)
			if (_updateCount++ % 30 != 0)
				return;
			var level = ConnectionManager.Instance.GameStart(GameId);
			if (level != null)
			{
				// game started
				GameController.Instance.GameStart(level);
			}
			else
			{
				// game has not started, update player list
				ChangePlayerList(ConnectionManager.Instance.PlayerListUpdate());
			}
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
		}

		private void LeaveButtonPressed(object sender, EventArgs args)
		{
			_soundManager.SoundPlay(SoundManager.SoundEnum.Click);

			ConnectionManager.Instance.LeaveGame();
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MultiplayerScreen);
		}

		private void DrawString(SpriteBatch spriteBatch, string text, float positionX, float positionY)
		{
			spriteBatch.DrawString(
				_spriteFont,
				text,
				new Vector2(positionX, positionY),
				Color.Red, 0, new Vector2(0f, 0f), 0.8f, SpriteEffects.None,
				layerDepth: 1f);
		}
	}
}
