using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Network;

namespace SkyShoot.Game.Screens
{
	internal class MultiplayerScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _mapLabel;

		private ButtonControl _backButton;
		private ButtonControl _createGameButton;
		private ButtonControl _joinGameButton;
		private ButtonControl _refreshButton;

		private ListControl _gameList;

		private GameDescription[] _gameDescriptions;

		public MultiplayerScreen()
		{
			CreateControls();
			InititalizeControls();
		}

		public override void OnShow()
		{
			base.OnShow();

			_gameDescriptions = ConnectionManager.Instance.GetGameList();

			// todo ?
			if (_gameDescriptions == null)
				return;

			_gameList.Items.Clear();
			foreach (GameDescription gameDescription in _gameDescriptions)
			{
				_gameList.Items.Add(gameDescription.ToString());
			}

			// _gameList.SelectedItems[0] = 0;
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/screens/screen_05_fix");
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			// CreateGame Button
			_createGameButton = new ButtonControl
									{
										Text = "Create Game",
										Bounds =
											new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -160f), 120, 32)
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
										new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -120f), 120, 32)
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

			_gameList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_gameList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_gameList.Slider.Bounds.Size.Y.Offset -= 2.0f;

			_gameList.SelectionMode = ListSelectionMode.Single;

			// Refresh Button
			_refreshButton = new ButtonControl
								{
									Text = "Refresh",
									Bounds =
										new UniRectangle(new UniScalar(0.5f, -20f), new UniScalar(0.4f, 140f), 120, 32)
								};
		}

		private void InititalizeControls()
		{
			Desktop.Children.Add(_createGameButton);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_joinGameButton);
			Desktop.Children.Add(_mapLabel);
			Desktop.Children.Add(_gameList);
			Desktop.Children.Add(_refreshButton);

			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_joinGameButton, JoinGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_createGameButton, CreateGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_refreshButton, RefreshButtonPressed);
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}

		private void JoinGameButtonPressed(object sender, EventArgs args)
		{
			if (_gameList.Items.Count == 0)
				return;

			if (!ConnectionManager.Instance.JoinGame(_gameDescriptions[_gameList.SelectedItems[0]]))
			{
				Trace.WriteLine("Join game failed");
			}
			else
			{
				WaitScreen.Tile = _gameDescriptions[_gameList.SelectedItems[0]].UsedTileSet.ToString();
				WaitScreen.GameMode = _gameDescriptions[_gameList.SelectedItems[0]].GameType.ToString();
				WaitScreen.MaxPlayers =
					_gameDescriptions[_gameList.SelectedItems[0]].MaximumPlayersAllowed.ToString(CultureInfo.InvariantCulture);
				WaitScreen.GameId = _gameDescriptions[_gameList.SelectedItems[0]].GameId;

				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.WaitScreen);
			}
		}

		private void CreateGameButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.CreateGameScreen);
		}

		private void RefreshButtonPressed(object sender, EventArgs args)
		{
			_gameList.Items.Clear();
			_gameDescriptions = ConnectionManager.Instance.GetGameList();

			if (_gameDescriptions == null)
				return;

			foreach (GameDescription gameDescription in _gameDescriptions)
			{
				_gameList.Items.Add(gameDescription.ToString());
			}
		}
	}
}
