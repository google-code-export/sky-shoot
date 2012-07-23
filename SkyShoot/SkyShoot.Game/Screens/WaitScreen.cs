using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Game;
using SkyShoot.Game.Network;

namespace SkyShoot.Game.Screens
{
	internal class WaitScreen : GameScreen
	{
		private static Texture2D _texture;

		private readonly ContentManager _content;

		private ListControl _playersList;
		private ButtonControl _leaveButton;

		private List<string> _players;
		private SpriteFont _spriteFont;

		private int _updateCount;

		public WaitScreen()
		{
			CreateControls();
			InititalizeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
			_updateCount = 0;
		}

		public static string Tile { get; set; }

		public static string GameMode { get; set; }

		public static string MaxPlayers { get; set; }

		public static int GameId { get; set; }

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_02_fix");
			_spriteFont = _content.Load<SpriteFont>("Times New Roman");

			// вывод списка игроков
			GameDescription[] gameDescriptions = ConnectionManager.Instance.GetGameList();

			if (gameDescriptions == null)
				return;

			GameDescription gameDescription = gameDescriptions.FirstOrDefault(description => GameId == description.GameId);

			// todo переписать
			if (gameDescription != null) 
				_players = gameDescription.Players;

			// todo ?
			if (_players == null)
				return;

			foreach (string player in _players)
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
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);

			DrawString(SpriteBatch, "Players", 20f, 25f);
			DrawString(SpriteBatch, "Map: ", 280f, 260f);
			DrawString(SpriteBatch, Tile, 400f, 260f);
			DrawString(SpriteBatch, "Game Mode:", 280f, 290f);
			DrawString(SpriteBatch, GameMode, 400f, 290f);
			DrawString(SpriteBatch, "Max Players:", 280f, 320f);
			DrawString(SpriteBatch, MaxPlayers, 400f, 320f);

			SpriteBatch.End();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// обновляемся только каждый 6-ой апдейт (через каждые 100 мс)
			if (_updateCount++ % 6 != 0)
				return;

			var level = ConnectionManager.Instance.GameStart(GameId);
			if (level != null)
			{
				// game started
				GameController.Instance.GameStart(level, GameId);
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
			ConnectionManager.Instance.LeaveGame();
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MultiplayerScreen);
		}

		private void DrawString(SpriteBatch spriteBatch, string text, float positionX, float positionY)
		{
			// spriteBatch.DrawString(_spriteFont, text, new Vector2(positionX, positionY), Color.Red);
			// spriteBatch.
			spriteBatch.DrawString(
				_spriteFont,
				text,
				new Vector2(positionX, positionY),
				Color.Red, 0, new Vector2(0f, 0f), 0.8f, SpriteEffects.None,
				layerDepth: 1f);
		}
	}
}
