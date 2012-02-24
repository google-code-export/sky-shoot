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
		private ListControl _maxPlayersList;
		private ListControl _tileList;
		private ListControl _gameModList;

		private LabelControl _maxPlayersLabel;
		private LabelControl _tileLabel;
		private LabelControl _gameModeLabel;
		private LabelControl _maxPlayers;
		private LabelControl _tile;
		private LabelControl _gameMode;

		private ButtonControl _backButton;
		private ButtonControl _createButton;

		private static Texture2D _texture;

		private readonly ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public CreateGameScreen()
		{
			CreateControls();
			InitializeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

		private void CreateControls()
		{
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

		private void InitializeControls()
		{
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

			ScreenManager.Instance.Controller.AddListener(_createButton, CreateButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			// кол-во игроков
			_maxPlayersList.SelectedItems[0] = 0;

			// карта
			_tileList.SelectedItems[0] = 0;

			// мод
			_gameModList.SelectedItems[0] = 0;
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		public override void Update(GameTime gameTime)
		{
			if (_maxPlayersList == null) return;

			_maxPlayers.Text = _maxPlayersList.Items[_maxPlayersList.SelectedItems[0]];
			_tile.Text = _tileList.Items[_tileList.SelectedItems[0]];
			_gameMode.Text = _gameModList.Items[_gameModList.SelectedItems[0]];
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(typeof (MultiplayerScreen)); // = ScreenManager.ScreenEnum.MultiplayerScreen;
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

			ScreenManager.Instance.SetActiveScreen(typeof (WaitScreen));
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