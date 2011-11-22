using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    class CreateGameScreen : GameScreen
    {

        private GuiManager _gui;
        private Screen _mainScreen;
        private ListControl _maxPlayersList;
        private ListControl _tileList;
        private ListControl _gameModList;
        private LabelControl _maxPlaersLabel;
        private LabelControl _tileLabel;
        private LabelControl _gameModeLabel;
        private LabelControl _maxPlayers;
        private LabelControl _tile;
        private LabelControl _gameMode;
        private ButtonControl _backButton;
        private ButtonControl _createButton;
        private int gameId;

        public CreateGameScreen()
        {
            
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
                );

            // выбора максимального кол-ва игроков
            _maxPlaersLabel = new LabelControl
            {
                Bounds = new UniRectangle(-60f, -34f, 100f, 24f),
                Text = "Max Players"
            };
            _mainScreen.Desktop.Children.Add(_maxPlaersLabel);

            _maxPlayersList = new ListControl
            {
                Bounds = new UniRectangle(-60f, -4f, 100f, 300f)
            };
            for (int i = 1; i < 11; i++)
            {
                _maxPlayersList.Items.Add(i + "");    
            }
            _maxPlayersList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _maxPlayersList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _maxPlayersList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            _maxPlayersList.SelectionMode = ListSelectionMode.Single;
            _maxPlayersList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_maxPlayersList);

            // выбор карты
            _tileLabel = new LabelControl
            {
                Bounds = new UniRectangle(60f, -34f, 150f, 24f),
                Text = "Map"
            };
            _mainScreen.Desktop.Children.Add(_tileLabel);

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
            _mainScreen.Desktop.Children.Add(_tileList);

            // выбор режима игры
            _gameModeLabel = new LabelControl
            {
                Bounds = new UniRectangle(230f, -34f, 150f, 24f),
                Text = "Mode"
            };
            _mainScreen.Desktop.Children.Add(_gameModeLabel);

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
            _mainScreen.Desktop.Children.Add(_gameModList);

            // выбор игрока

            // кол-во игроков
            _maxPlayersList.SelectedItems[0] = 0;
            _maxPlayers = new LabelControl
            {
                Bounds = new UniRectangle(500f, 50f, 150f, 24f),
                Text = _maxPlayersList.Items[_maxPlayersList.SelectedItems[0]] + ""
            };
            _mainScreen.Desktop.Children.Add(_maxPlayers);

            // карта
            _tileList.SelectedItems[0] = 0;
            _tile = new LabelControl
            {
                Bounds = new UniRectangle(500f, 80f, 150f, 24f),
                Text = _tileList.Items[_tileList.SelectedItems[0]] + ""
            };
            _mainScreen.Desktop.Children.Add(_tile);

            // мод
            _gameModList.SelectedItems[0] = 0;
            _gameMode = new LabelControl
            {
                Bounds = new UniRectangle(500f, 110f, 150f, 24f),
                Text = _gameModList.Items[_gameModList.SelectedItems[0]] + ""
            };
            _mainScreen.Desktop.Children.Add(_gameMode);

            // Create Button
            _createButton = new ButtonControl
            {
                Text = "Create",
                Bounds = new UniRectangle(new UniScalar(0.5f, 178f), new UniScalar(0.4f, -15f), 110, 32)
            };
            _createButton.Pressed += CreateButtonPressed;
            _mainScreen.Desktop.Children.Add(_createButton);
            
            // Back Button
            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds = new UniRectangle(new UniScalar(0.5f, -380f), new UniScalar(0.4f, 170f), 120, 32)
            };
            _backButton.Pressed += BackButtonPressed;
            _mainScreen.Desktop.Children.Add(_backButton);
        }

        public override void Update(GameTime gameTime, bool otherHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherHasFocus, coveredByOtherScreen);

            _maxPlayers.Text = _maxPlayersList.Items[_maxPlayersList.SelectedItems[0]];
            _tile.Text = _tileList.Items[_tileList.SelectedItems[0]];
            _gameMode.Text = _gameModList.Items[_gameModList.SelectedItems[0]];

        }

        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
            ScreenManager.AddScreen(new MultiplayerScreen());
        }

        private void CreateButtonPressed(object sender, EventArgs args)
        {
            if (_gameMode.Text == "Coop")
            {
                GameController.Instance.CreateGame(GameMode.Coop, Convert.ToInt32(_maxPlayers.Text));
                gameId = GameController.Instance.CreateGame(GameMode.Coop, Convert.ToInt32(_maxPlayers.Text)).GameId;
            }
            if (_gameMode.Text == "Deathmatch")
            {
                GameController.Instance.CreateGame(GameMode.Coop, Convert.ToInt32(_maxPlayers.Text));
                gameId = GameController.Instance.CreateGame(GameMode.Coop, Convert.ToInt32(_maxPlayers.Text)).GameId;
            }
            if (_gameMode.Text == "Campaign")
            {
                GameController.Instance.CreateGame(GameMode.Coop, Convert.ToInt32(_maxPlayers.Text));
                gameId = GameController.Instance.CreateGame(GameMode.Coop, Convert.ToInt32(_maxPlayers.Text)).GameId;
            }
            ExitScreen();
            ScreenManager.AddScreen(new WaitScreen(_tile.Text, _gameMode.Text, _maxPlayers.Text, gameId));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

        

    }
}
