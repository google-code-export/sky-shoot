using System;

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
	internal class CreateGameScreen : GameScreen
	{
		private GuiManager _gui;

		private readonly ListControl _maxPlayersList;
		private readonly ListControl _tileList;
		private readonly ListControl _gameModList;

		private readonly LabelControl _maxPlayersLabel;
		private readonly LabelControl _tileLabel;
		private readonly LabelControl _gameModeLabel;
		private readonly LabelControl _maxPlayers;
		private readonly LabelControl _tile;
		private readonly LabelControl _gameMode;

		private readonly ButtonControl _backButton;
		private readonly ButtonControl _createButton;

		private static Texture2D _texture;

		private ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public CreateGameScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;

			// выбора максимального кол-ва игроков
			_maxPlayersLabel = new LabelControl
			{
				Bounds = new UniRectangle(-60f, -34f, 100f, 24f),
				Text = "Max Players"
			};

			// выбор карты
			_tileLabel = new LabelControl
			{
				Bounds = new UniRectangle(60f, -34f, 150f, 24f),
				Text = "Map"
			};

			_tileList = new ListControl
			{
				Bounds = new UniRectangle(60f, -4f, 150f, 300f)
			};
			_tileList.Items.Add("Snow");
			_tileList.Items.Add("Desert");
			_tileList.Items.Add("Grass");
			_tileList.Items.Add("Sand");
			_tileList.Items.Add("Volcanic");
			_tileList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_tileList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_tileList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_tileList.SelectionMode = ListSelectionMode.Single;
			_tileList.SelectedItems.Add(4);

			// выбор режима игры
			_gameModeLabel = new LabelControl
			{
				Bounds = new UniRectangle(230f, -34f, 150f, 24f),
				Text = "Mode"
			};

			_maxPlayersList = new ListControl
			{
				Bounds = new UniRectangle(-60f, -4f, 100f, 300f)
			};
			_maxPlayersList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_maxPlayersList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_maxPlayersList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_maxPlayersList.SelectionMode = ListSelectionMode.Single;
			_maxPlayersList.SelectedItems.Add(4);

			for (int i = 1; i < 11; i++)
			{
				_maxPlayersList.Items.Add(i + "");
			}

			_maxPlayers = new LabelControl
			{
				Bounds = new UniRectangle(500f, 50f, 150f, 24f),
				Text = _maxPlayersList.Items[_maxPlayersList.SelectedItems[0]] + ""
			};

			_tile = new LabelControl
			{
				Bounds = new UniRectangle(500f, 80f, 150f, 24f),
				Text = _tileList.Items[_tileList.SelectedItems[0]] + ""
			};

			_gameModList = new ListControl
			{
				Bounds = new UniRectangle(230f, -4f, 150f, 300f)
			};
			_gameModList.Items.Add("Coop");
			_gameModList.Items.Add("Deathmatch");
			_gameModList.Items.Add("Campaign");
			_gameModList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_gameModList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_gameModList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_gameModList.SelectionMode = ListSelectionMode.Single;
			_gameModList.SelectedItems.Add(4);

			// кол-во игроков
			_maxPlayersList.SelectedItems[0] = 0;

			// карта
			_tileList.SelectedItems[0] = 0;

			// мод
			_gameModList.SelectedItems[0] = 0;

			_gameMode = new LabelControl
			{
				Bounds = new UniRectangle(500f, 110f, 150f, 24f),
				Text = _gameModList.Items[_gameModList.SelectedItems[0]] + ""
			};

			// Create Button
			_createButton = new ButtonControl
			{
				Text = "Create",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, 178f), new UniScalar(0.4f, -15f), 110, 32)
			};

			// Back Button
			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds =
					new UniRectangle(new UniScalar(0.5f, -380f), new UniScalar(0.4f, 170f), 120, 32)
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

			// кол-во игроков
			_maxPlayersList.SelectedItems[0] = 0;

			// карта
			_tileList.SelectedItems[0] = 0;

			// мод
			_gameModList.SelectedItems[0] = 0;

			_createButton.Pressed += CreateButtonPressed;
			_backButton.Pressed += BackButtonPressed;

			Desktop.Children.Add(_maxPlayersList);
			Desktop.Children.Add(_maxPlayersLabel);
			Desktop.Children.Add(_tileLabel);
			Desktop.Children.Add(_tileList);
			Desktop.Children.Add(_gameModeLabel);
			Desktop.Children.Add(_gameModList);
			Desktop.Children.Add(_maxPlayers);
			Desktop.Children.Add(_createButton);
			Desktop.Children.Add(_gameMode);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_tile);
		}

		public override void UnloadContent()
		{
			Desktop.Children.Remove(_maxPlayersList);
			Desktop.Children.Remove(_maxPlayersLabel);
			Desktop.Children.Remove(_tileLabel);
			Desktop.Children.Remove(_tileList);
			Desktop.Children.Remove(_gameModeLabel);
			Desktop.Children.Remove(_gameModList);
			Desktop.Children.Remove(_maxPlayers);
			Desktop.Children.Remove(_createButton);
			Desktop.Children.Remove(_gameMode);
			Desktop.Children.Remove(_backButton);
			Desktop.Children.Remove(_tile);

			_createButton.Pressed -= CreateButtonPressed;
			_backButton.Pressed -= BackButtonPressed;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (_maxPlayersList == null) return;

			_maxPlayers.Text = _maxPlayersList.Items[_maxPlayersList.SelectedItems[0]];
			_tile.Text = _tileList.Items[_tileList.SelectedItems[0]];
			_gameMode.Text = _gameModList.Items[_gameModList.SelectedItems[0]];
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MultiplayerScreen;
		}

		private void CreateButtonPressed(object sender, EventArgs args)
		{
			GameMode m;
			TileSet ts;
			switch (_gameMode.Text)
			{
				case "Coop":
					m = GameMode.Coop;
					break;
				case "Deathmatch":
					m = GameMode.Deathmatch;
					break;
				case "Campaign":
					m = GameMode.Campaign;
					break;
				default:
					m = GameMode.Coop;
					break;
			}
			switch (_tile.Text)
			{
				case "Grass":
					ts = TileSet.Grass;
					break;
				case "Snow":
					ts = TileSet.Snow;
					break;
				case "Desert":
					ts = TileSet.Desert;
					break;
				case "Sand":
					ts = TileSet.Sand;
					break;
				case "Volcanic":
					ts = TileSet.Volcanic;
					break;
				default:
					ts = TileSet.Grass;
					break;
			}
			GameDescription gameDescription = GameController.Instance.CreateGame(m, Convert.ToInt32(_maxPlayers.Text),
			                                                                     ts);
			if (gameDescription == null)
				return;
			WaitScreen.Tile = _tile.Text;
			WaitScreen.GameMode = _gameMode.Text;
			WaitScreen.MaxPlayers = _maxPlayers.Text;
			WaitScreen.GameId = GameController.Instance.GetGameList().Length;

			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.WaitScreen;
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
