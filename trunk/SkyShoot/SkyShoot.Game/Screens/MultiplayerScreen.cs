using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Contracts.Session;
using SkyShoot.Game.Controls;

using SkyShoot.Game.Client.Game;

namespace SkyShoot.Game.Screens
{
	internal class MultiplayerScreen : GameScreen
	{
		private GuiManager _gui;

		private readonly LabelControl _mapLabel;

		private readonly ButtonControl _backButton;
		private readonly ButtonControl _createGameButton;
		private readonly ButtonControl _joinGameButton;
		private readonly ButtonControl _refreshButton;

		private readonly ListControl _gameList;

		private GameDescription[] _tempGameList;

		private static Texture2D _texture;

		private ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public MultiplayerScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;

			// CreateGame Button
			_createGameButton = new ButtonControl
			{
				Text = "Create Game",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -160f), 120,
									 32)
			};

			// Back Button
			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -80f), 120, 32)
			};

			// JoinGame Button
			_joinGameButton = new ButtonControl
			{
				Text = "Join Game",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -120f), 120,
									 32)
			};

			// Label of maps
			_mapLabel = new LabelControl
			{
				Bounds = new UniRectangle(300.0f, -30.0f, 200.0f, 24.0f),
				Text = "Games"
			};

			// Games List
			_gameList = new ListControl
			{
				Bounds = new UniRectangle(300f, -10f, 225f, 300f)
			};

			// Refresh Button
			_refreshButton = new ButtonControl
			{
				Text = "Refresh",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -20f), new UniScalar(0.4f, 140f), 120, 32)
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
			
			_gameList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_gameList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_gameList.Slider.Bounds.Size.Y.Offset -= 2.0f;

			// todo
			// запрос списка игр с сервера и его вывод
			//
			_tempGameList = GameController.Instance.GetGameList();

			if (_tempGameList == null)
				return;

			foreach (GameDescription gameDescription in _tempGameList)
			{
				_gameList.Items.Add(gameDescription.ToString());
			}
			_gameList.SelectionMode = ListSelectionMode.Single;
			_gameList.SelectedItems.Add(4);
			_gameList.SelectedItems[0] = 0;

			Desktop.Children.Add(_createGameButton);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_joinGameButton);
			Desktop.Children.Add(_mapLabel);
			Desktop.Children.Add(_gameList);
			Desktop.Children.Add(_refreshButton);

			_backButton.Pressed += BackButtonPressed;
			_joinGameButton.Pressed += JoinGameButtonPressed;
			_createGameButton.Pressed += CreateGameButtonPressed;
			_refreshButton.Pressed += RefreshButtonPressed;
		}

		public override void UnloadContent()
		{
			Desktop.Children.Remove(_createGameButton);
			Desktop.Children.Remove(_backButton);
			Desktop.Children.Remove(_joinGameButton);
			Desktop.Children.Remove(_mapLabel);
			Desktop.Children.Remove(_gameList);
			Desktop.Children.Remove(_refreshButton);			
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MainMenuScreen;
		}

		private void JoinGameButtonPressed(object sender, EventArgs args)
		{
			_tempGameList = GameController.Instance.GetGameList();

			if (_tempGameList == null)
				return;

			if (_gameList.Items.Count != 0)
			{
				if (!GameController.Instance.JoinGame(_tempGameList[_gameList.SelectedItems[0]]))
				{
					Trace.WriteLine("Join game failed");
				}
				else
				{
					WaitScreen.Tile = _tempGameList[_gameList.SelectedItems[0]].UsedTileSet + "";
					WaitScreen.GameMode = _tempGameList[_gameList.SelectedItems[0]].GameType + "";
					WaitScreen.MaxPlayers = _tempGameList[_gameList.SelectedItems[0]].MaximumPlayersAllowed + "";
					WaitScreen.GameId = _tempGameList[_gameList.SelectedItems[0]].GameId;

					ScreenManager.Instance.ActiveScreen =
						ScreenManager.ScreenEnum.WaitScreen;
				}
			}
		}

		private void CreateGameButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.CreateGameScreen;
		}

		private void RefreshButtonPressed(object sender, EventArgs args)
		{
			_gameList.Items.Clear();
			_tempGameList = GameController.Instance.GetGameList();

			if (_tempGameList == null)
				return;

			foreach (GameDescription gameDescription in _tempGameList)
			{
				_gameList.Items.Add(gameDescription.ToString());
			}
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
			
			base.Draw(gameTime);
			_gui.Draw(gameTime);
		}
	}
}
