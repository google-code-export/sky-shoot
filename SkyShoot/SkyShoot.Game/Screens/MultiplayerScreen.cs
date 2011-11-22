using System;

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
    internal class MultiplayerScreen : GameScreen
    {
        private GuiManager _gui;
        private ButtonControl _backButton;
        private ButtonControl _createGameButton;
        private ButtonControl _joinGameButton;
        private ButtonControl _refreshButton;
        private ListControl _gameList;
        private Screen _mainScreen;
        private LabelControl _mapLabel;
        private GameDescription[] tempGameList;

        public MultiplayerScreen()
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

            // CreateGame Button
            _createGameButton = new ButtonControl
            {
                Text = "Create Game",
                Bounds = new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -160f), 120, 32)
            };
            _createGameButton.Pressed += CreateGameButtonPressed;
            _mainScreen.Desktop.Children.Add(_createGameButton);

            // Back Button
            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds = new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -80f), 120, 32)
            };
            _backButton.Pressed += BackButtonPressed;
            _mainScreen.Desktop.Children.Add(_backButton);

            // JoinGame Button
            _joinGameButton = new ButtonControl
            {
                Text = "Join Game",
                Bounds = new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -120f), 120, 32)
            };
            _joinGameButton.Pressed += JoinGameButtonPressed;
            _mainScreen.Desktop.Children.Add(_joinGameButton);

            //Label of maps
            _mapLabel = new LabelControl
            {
                Bounds = new UniRectangle(300.0f, -30.0f, 200.0f, 24.0f), 
                Text = "Games"
            };
            _mainScreen.Desktop.Children.Add(_mapLabel);

            //Games List
            _gameList = new ListControl
            {
                Bounds = new UniRectangle(300f, -10f, 200f, 300f)
            };
            _gameList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _gameList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _gameList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            //
            // запрос списка игр с сервера и его вывод
            //
            tempGameList = GameController.Instance.GetGameList();
            for (int i = 0; i < tempGameList.Length; i++)
            {
                _gameList.Items.Add(tempGameList[i].ToString());
            }
            _gameList.SelectionMode = ListSelectionMode.Single;
            _gameList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_gameList);
            

            // Refresh Button
            _refreshButton = new ButtonControl
            {
                Text = "Refresh",
                Bounds = new UniRectangle(new UniScalar(0.5f, -20f), new UniScalar(0.4f, 140f), 120, 32)
            };
            _refreshButton.Pressed += RefreshPressed;
            _mainScreen.Desktop.Children.Add(_refreshButton);

        }

        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        private void JoinGameButtonPressed(object sender, EventArgs args)
        {
            //todo setActive
            ExitScreen();
            ScreenManager.AddScreen(new WaitScreen(GameController.Instance.GetGameList()[_gameList.SelectedItems[0]].UsedTileSet + "",
                                                   GameController.Instance.GetGameList()[_gameList.SelectedItems[0]].GameType + "",
                                                   GameController.Instance.GetGameList()[_gameList.SelectedItems[0]].MaximumPlayersAllowed + "",
                                                   GameController.Instance.GetGameList()[_gameList.SelectedItems[0]].GameId
                                                   )
                                                   );

            //todo temporary
            GameController.Instance.JoinGame(GameController.Instance.GetGameList()[_gameList.SelectedItems[0]]);

        }

        private void CreateGameButtonPressed(object sender, EventArgs args)
        {
            //todo setActive
            //foreach (GameScreen screen in ScreenManager.GetScreens()) screen.ExitScreen();

            ExitScreen();

            ScreenManager.AddScreen(new CreateGameScreen());

        }

        private void RefreshPressed(object sender, EventArgs args)
        {
            _gameList.Items.Clear();
            tempGameList = GameController.Instance.GetGameList();
            for (int i = 0; i < tempGameList.Length; i++)
            {
                _gameList.Items.Add(tempGameList[i].ToString());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }
}
